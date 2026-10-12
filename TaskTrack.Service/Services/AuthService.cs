using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TaskTrack.Repo.Models;
using TaskTrack.Repo.UnitOfWork;
using TaskTrack.Service.DTOs;

namespace TaskTrack.Service.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _uow;
        private readonly IConfiguration _config;

        public AuthService(IUnitOfWork uow, IConfiguration config)
        {
            _uow = uow;
            _config = config;
        }

        public async Task<AccountReadDto> RegisterStaffAsync(RegisterDto dto)
        {
            var existing = await _uow.Accounts.GetByEmailAsync(dto.Email);
            if (existing != null)
            {
                throw new InvalidOperationException("Email is already registered. Please use another email.");
            }

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            var account = new SystemAccount
            {
                FullName = dto.FullName.Trim(),
                Email = dto.Email.Trim().ToLower(),
                PasswordHash = passwordHash,
                Role = 0, // Staff role only via registration
                CreatedDate = DateTime.UtcNow
            };

            await _uow.Accounts.AddAsync(account);
            await _uow.SaveChangesAsync();

            return MapToReadDto(account);
        }

        public async Task<AuthResponseDto?> LoginAsync(LoginDto dto)
        {
            var account = await _uow.Accounts.GetByEmailAsync(dto.Email);
            if (account == null)
            {
                return null;
            }

            bool isValid = BCrypt.Net.BCrypt.Verify(dto.Password, account.PasswordHash);
            if (!isValid)
            {
                return null;
            }

            var token = GenerateJwtToken(account);

            return new AuthResponseDto
            {
                Token = token,
                Account = MapToReadDto(account)
            };
        }

        private string GenerateJwtToken(SystemAccount account)
        {
            var secretKey = _config["Jwt:Key"]
                            ?? Environment.GetEnvironmentVariable("JWT_SECRET")
                            ?? "TaskTrackSuperSecretKeyForJwtAuthentication2026!PRN232Ass2";

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var roleName = account.Role == 1 ? "Admin" : "Staff";

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, account.AccountID.ToString()),
                new Claim(ClaimTypes.Email, account.Email),
                new Claim(ClaimTypes.Name, account.FullName),
                new Claim(ClaimTypes.Role, roleName),
                new Claim("AccountID", account.AccountID.ToString()),
                new Claim("Role", account.Role.ToString())
            };

            var issuer = _config["Jwt:Issuer"] ?? "TaskTrackAPI";
            var audience = _config["Jwt:Audience"] ?? "TaskTrackClient";

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(24),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private static AccountReadDto MapToReadDto(SystemAccount a) => new()
        {
            AccountID = a.AccountID,
            FullName = a.FullName,
            Email = a.Email,
            Role = a.Role,
            CreatedDate = a.CreatedDate
        };
    }
}
