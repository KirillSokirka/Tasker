using Microsoft.EntityFrameworkCore;
using Tasker.Domain.Entities.Application;
using Task = Tasker.Domain.Entities.Application.Task;
using TaskStatus = Tasker.Domain.Entities.Application.TaskStatus;

namespace Tasker.Infrastructure.Data.Application;

public class ApplicationContext : DbContext
{
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    {
    }

    public DbSet<KanbanBoard> KanbanBoards { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<Release> Releases { get; set; }
    public DbSet<Task> Tasks { get; set; }
    public DbSet<TaskStatus> TaskStatuses { get; set; }
    public DbSet<User> User { get; set; }
    
    public DbSet<AdminProjectUser> AdminProjectUsers { get; set; }
    public DbSet<AssignedProjectUser> AssignedProjectUsers { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ConfigurePrimaryKeys(modelBuilder);
        ConfigurePropertyConventions(modelBuilder);
        ConfigureRelationships(modelBuilder);
    }

    private void ConfigurePrimaryKeys(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<KanbanBoard>().HasKey(e => e.Id);
        modelBuilder.Entity<Project>().HasKey(e => e.Id);
        modelBuilder.Entity<Release>().HasKey(e => e.Id);
        modelBuilder.Entity<Task>().HasKey(e => e.Id);
        modelBuilder.Entity<TaskStatus>().HasKey(e => e.Id);
        modelBuilder.Entity<User>().HasKey(e => e.Id);
    }

    private void ConfigurePropertyConventions(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Project>()
            .Property(p => p.Title)
            .HasMaxLength(100)
            .IsRequired();

        modelBuilder.Entity<User>()
            .Property(u => u.Title)
            .HasMaxLength(100)
            .IsRequired();

        modelBuilder.Entity<KanbanBoard>()
            .Property(kb => kb.Title)
            .HasMaxLength(100)
            .IsRequired();

        modelBuilder.Entity<Release>()
            .Property(r => r.Title)
            .HasMaxLength(100)
            .IsRequired();

        modelBuilder.Entity<Release>()
            .HasIndex(t => t.CreationDate);

        modelBuilder.Entity<Task>()
            .Property(t => t.Title)
            .HasMaxLength(200)
            .IsRequired();

        modelBuilder.Entity<Task>()
            .Property(t => t.Description)
            .HasMaxLength(500)
            .IsRequired(false);

        modelBuilder.Entity<Task>()
            .HasIndex(t => t.CreationDate);
    }

    private void ConfigureRelationships(ModelBuilder modelBuilder)
    {
        // Project and KanbanBoard: One-to-Many
        modelBuilder.Entity<Project>()
            .HasMany(p => p.KanbanBoards)
            .WithOne(kb => kb.Project)
            .HasForeignKey(kb => kb.ProjectId);

        // Project and Task: One-to-Many
        modelBuilder.Entity<Project>()
            .HasMany(p => p.Tasks)
            .WithOne(t => t.Project)
            .HasForeignKey(t => t.ProjectId);
        
        // Project and Release: One-to-Many
        modelBuilder.Entity<Project>()
            .HasMany(p => p.Releases)
            .WithOne(r => r.Project)
            .HasForeignKey(r => r.ProjectId);

        // Task and TaskStatus: Many-to-One
        modelBuilder.Entity<Task>()
            .HasOne(t => t.Status)
            .WithMany(ts => ts.Tasks)
            .HasForeignKey(t => t.TaskStatusId)
            .IsRequired(false); 

        // Task and Release: Many-to-One
        modelBuilder.Entity<Task>()
            .HasOne(t => t.Release)
            .WithMany(r => r.Tasks)
            .HasForeignKey(t => t.ReleaseId)
            .IsRequired(false); // Assuming Task might not be part of a Release

        // Task and User (Assignee and Creator): Many-to-One
        modelBuilder.Entity<Task>()
            .HasOne(t => t.Assignee)
            .WithMany() // No navigation property back to User in Task for Assignee
            .HasForeignKey(t => t.AssigneeId)
            .IsRequired(false); // Assuming Task might not have an Assignee initially

        modelBuilder.Entity<Task>()
            .HasOne(t => t.Creator)
            .WithMany() // No navigation property back to User in Task for Creator
            .HasForeignKey(t => t.CreatorId)
            .IsRequired();
        
        // KanbanBoard and TaskStatus: One-to-Many
        modelBuilder.Entity<KanbanBoard>()
            .HasMany(kb => kb.Columns)
            .WithOne(ts => ts.KanbanBoard)
            .HasForeignKey(ts => ts.KanbanBoardId);
        
        // Projects and User: Many-to-Many
        modelBuilder.Entity<AdminProjectUser>()
            .HasKey(pu => new { pu.ProjectId, pu.UserId });

        modelBuilder.Entity<AdminProjectUser>()
            .HasOne(pu => pu.Project)
            .WithMany(p => p.AdminProjectUsers)
            .HasForeignKey(pu => pu.ProjectId);

        modelBuilder.Entity<AdminProjectUser>()
            .HasOne(pu => pu.User)
            .WithMany(u => u.AdminProjectUsers)
            .HasForeignKey(pu => pu.UserId);
        
        modelBuilder.Entity<AssignedProjectUser>()
            .HasKey(pu => new { pu.ProjectId, pu.UserId });

        modelBuilder.Entity<AssignedProjectUser>()
            .HasOne(pu => pu.Project)
            .WithMany(p => p.AssignedProjectUsers)
            .HasForeignKey(pu => pu.ProjectId);

        modelBuilder.Entity<AssignedProjectUser>()
            .HasOne(pu => pu.User)
            .WithMany(u => u.AssignedProjectUsers)
            .HasForeignKey(pu => pu.UserId);
        
    }
}