using Microsoft.EntityFrameworkCore;
using ProjectManagement.API.Models;

namespace ProjectManagement.API.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Project> Projects { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<ProjectManagement.API.Models.Task> Tasks { get; set; }
    public DbSet<Activity> Activities { get; set; }
    public DbSet<ProjectMember> ProjectMembers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Project>()
            .Property(p => p.StartDate)
            .HasColumnType("date");

        modelBuilder.Entity<Project>()
            .Property(p => p.DueDate)
            .HasColumnType("date");

        modelBuilder.Entity<Project>()
            .Property(p => p.CreatedAt)
            .HasColumnType("timestamp with time zone");
    }
}