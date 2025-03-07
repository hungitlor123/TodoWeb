using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using TodoWeb.Domains.Entities;
using TodoWeb.Infrastructures.DatabaseMapping;

namespace TodoWeb.Infrastructures;

public class ApplicationDbContext : DbContext, IApplicationDbContext

{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<ToDo> Todos { get; set; }

    public DbSet<Student> Student { get; set; }
    
    public DbSet<CourseStudent> CourseStudent { get; set; }

    public DbSet<Course> Course { get; set; }
    
    public DbSet<StudentGrade> StudentGrades { get; set; } // Sử dụng số nhiều để nhất quán

    public DbSet<AuditLog> AuditLog { get; set; }

    public DbSet<School> School { get; set; }
    


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLazyLoadingProxies();
        optionsBuilder.UseSqlServer("Server=HUNGNEEE\\HUNGIT;Initial Catalog=ToDoApp;Persist Security Info=False;User ID=sa;Password=12345;MultipleActiveResultSets=True;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
        modelBuilder.Entity<CourseStudent>()
            .HasKey(cs => new { cs.CourseId, cs.StudentId });

        
        modelBuilder.Entity<StudentGrade>()
            .HasOne(sg => sg.CourseStudent)
            .WithOne(cs => cs.StudentGrade)
            .HasForeignKey<StudentGrade>(sg => new { sg.CourseId, sg.StudentId }) 
            .OnDelete(DeleteBehavior.Cascade);

        
        modelBuilder.Entity<Student>()
            .Property(x => x.Age)
            .HasComputedColumnSql("DATEDIFF(YEAR, DateOfBirth, GETDATE())");

        modelBuilder.Entity<Student>()
            .HasMany(student => student.CourseStudents)
            .WithOne(cs => cs.Student)
            .HasForeignKey(cs => cs.StudentId);

        modelBuilder.Entity<Course>()
            .HasMany(course => course.CourseStudents)
            .WithOne(cs => cs.Course)
            .HasForeignKey(cs => cs.CourseId);

        
        modelBuilder.ApplyConfiguration(new CourseMapping());

        base.OnModelCreating(modelBuilder);
    }    
        

    public int SaveChanges()
    {
       var auditLogs = new List<AuditLog>();
        foreach (var entity in ChangeTracker.Entries()) {
            var log = new AuditLog
            {
                EntityName = entity.Entity.GetType().Name,
                CreateAt = DateTime.Now,
                Action = entity.State.ToString(),
                
            };
        if (entity.State == EntityState.Added)
        {
        log.NewValue = JsonSerializer.Serialize(entity.CurrentValues.ToObject());
        }
        if (entity.State == EntityState.Modified)
        {
        log.OldValue = JsonSerializer.Serialize(entity.OriginalValues.ToObject());
        log.NewValue = JsonSerializer.Serialize(entity.CurrentValues.ToObject());
        }
        if (entity.State == EntityState.Deleted)
        {
        log.OldValue = JsonSerializer.Serialize(entity.OriginalValues.ToObject());
        }
        auditLogs.Add(log);

    }
        AuditLog.AddRange(auditLogs); //state Addede

        return base.SaveChanges();
    }

    public EntityEntry<T> Entry<T>(T entity) where T : class
    {
            return base.Entry(entity);
    }
    
}