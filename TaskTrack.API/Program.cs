using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using TaskTrack.Repo.Data;
using TaskTrack.Repo.UnitOfWork;
using TaskTrack.Service.Services;

var builder = WebApplication.CreateBuilder(args);

// ─── Database ─────────────────────────────────────────────────────────────────
builder.Services.AddDbContext<TaskTrackDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// ─── Unit of Work & Repositories ─────────────────────────────────────────────
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// ─── Application Services ─────────────────────────────────────────────────────
builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<ITagService, TagService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAccountService, AccountService>();

// ─── JWT Authentication & Authorization ───────────────────────────────────────
var jwtKey = builder.Configuration["Jwt:Key"]
             ?? Environment.GetEnvironmentVariable("JWT_SECRET")
             ?? "TaskTrackSuperSecretKeyForJwtAuthentication2026!PRN232Ass2";

var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "TaskTrackAPI";
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? "TaskTrackClient";

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
        ValidateIssuer = false, // flexible for cloud deployment
        ValidateAudience = false,
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization();

// ─── Controllers + JSON Options ───────────────────────────────────────────────
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// ─── Swagger / OpenAPI with JWT Bearer support ────────────────────────────────
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "TaskTrack API",
        Version = "v1",
        Description = "Task & Team Management API — ASP.NET Core (.NET 8) with JWT Authentication & RBAC"
    });

    // Add JWT Bearer definition to Swagger UI
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// ─── CORS ─────────────────────────────────────────────────────────────────────
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// ─── Auto-ensure SystemAccount table and Seed Admin ───────────────────────────
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<TaskTrackDbContext>();
    try
    {
        dbContext.Database.ExecuteSqlRaw(@"
            CREATE TABLE IF NOT EXISTS ""SystemAccount"" (
                ""AccountID"" SERIAL PRIMARY KEY,
                ""FullName"" VARCHAR(200) NOT NULL,
                ""Email"" VARCHAR(200) NOT NULL UNIQUE,
                ""PasswordHash"" VARCHAR(500) NOT NULL,
                ""Role"" INT NOT NULL DEFAULT 0,
                ""CreatedDate"" TIMESTAMPTZ NOT NULL DEFAULT NOW()
            );
            ALTER TABLE ""Task"" ADD COLUMN IF NOT EXISTS ""CreatedByID"" INT REFERENCES ""SystemAccount""(""AccountID"");
        ");

        var adminExists = dbContext.SystemAccounts.Any(a => a.Role == 1 || a.Email == "admin@tasktrack.com");
        if (!adminExists)
        {
            var admin = new TaskTrack.Repo.Models.SystemAccount
            {
                FullName = "System Administrator",
                Email = "admin@tasktrack.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123456"),
                Role = 1, // Admin role
                CreatedDate = DateTime.UtcNow
            };
            dbContext.SystemAccounts.Add(admin);
            dbContext.SaveChanges();
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[DB Init Warning] {ex.Message}");
    }
}

// ─── Swagger (enabled in ALL environments, including Production on Render) ────
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "TaskTrack API v1");
    c.RoutePrefix = "swagger";
});

// ─── Middleware pipeline ──────────────────────────────────────────────────────
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// Auto-redirect root URL to Swagger UI
app.MapGet("/", () => Results.Redirect("/swagger"));

app.Run();
