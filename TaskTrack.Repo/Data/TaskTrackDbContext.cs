using Microsoft.EntityFrameworkCore;
using TaskTrack.Repo.Models;

namespace TaskTrack.Repo.Data
{
    public class TaskTrackDbContext : DbContext
    {
        public TaskTrackDbContext(DbContextOptions<TaskTrackDbContext> options) : base(options) { }

        public DbSet<Department> Departments { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Models.Task> Tasks { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<TaskTag> TaskTags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ─── Department ───────────────────────────────────────────────────────────
            modelBuilder.Entity<Department>(entity =>
            {
                entity.ToTable("Department");
                entity.HasKey(e => e.DepartmentID);
                entity.Property(e => e.DepartmentID).HasColumnName("DepartmentID");
                entity.Property(e => e.DepartmentName).IsRequired().HasMaxLength(200);
                entity.Property(e => e.DepartmentDescription).HasMaxLength(500);
                entity.Property(e => e.IsActive).HasDefaultValue(true);
            });

            // ─── Project ──────────────────────────────────────────────────────────────
            modelBuilder.Entity<Project>(entity =>
            {
                entity.ToTable("Project");
                entity.HasKey(e => e.ProjectID);
                entity.Property(e => e.ProjectID).HasColumnName("ProjectID");
                entity.Property(e => e.ProjectName).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Description).HasMaxLength(1000);
                entity.Property(e => e.Status).HasConversion<int>();
                entity.Property(e => e.IsActive).HasDefaultValue(true);
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("NOW()");

                entity.HasOne(e => e.Department)
                      .WithMany(d => d.Projects)
                      .HasForeignKey(e => e.DepartmentID)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // ─── Task ─────────────────────────────────────────────────────────────────
            modelBuilder.Entity<Models.Task>(entity =>
            {
                entity.ToTable("Task");
                entity.HasKey(e => e.TaskID);
                entity.Property(e => e.TaskID).HasColumnName("TaskID");
                entity.Property(e => e.Title).IsRequired().HasMaxLength(300);
                entity.Property(e => e.Description).HasMaxLength(1000);
                entity.Property(e => e.Status).HasConversion<int>();
                entity.Property(e => e.Priority).HasConversion<int>();
                entity.Property(e => e.IsActive).HasDefaultValue(true);
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("NOW()");

                entity.HasOne(e => e.Project)
                      .WithMany(p => p.Tasks)
                      .HasForeignKey(e => e.ProjectID)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // ─── Tag ──────────────────────────────────────────────────────────────────
            modelBuilder.Entity<Tag>(entity =>
            {
                entity.ToTable("Tag");
                entity.HasKey(e => e.TagID);
                entity.Property(e => e.TagID).HasColumnName("TagID");
                entity.Property(e => e.TagName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Color).HasMaxLength(50);
            });

            // ─── TaskTag (Join table — composite PK) ─────────────────────────────────
            modelBuilder.Entity<TaskTag>(entity =>
            {
                entity.ToTable("TaskTag");
                entity.HasKey(e => new { e.TaskID, e.TagID });

                entity.HasOne(e => e.Task)
                      .WithMany(t => t.TaskTags)
                      .HasForeignKey(e => e.TaskID)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Tag)
                      .WithMany(t => t.TaskTags)
                      .HasForeignKey(e => e.TagID)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
