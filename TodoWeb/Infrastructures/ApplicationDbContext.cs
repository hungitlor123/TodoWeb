using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using TodoWeb.Domains.Entities;
using TodoWeb.Infrastructures.DatabaseMapping;
using TodoWeb.Infrastructures.Interceptors;

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
    
    public DbSet<Exam> Exam { get; set; }
    
    public DbSet<ExamResult> ExamResult { get; set; }
    
    public DbSet<QuestionBank> QuestionBank { get; set; }
    public DbSet<AuditLog> AuditLog { get; set; }

    public DbSet<School> School { get; set; }
    


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLazyLoadingProxies();
        optionsBuilder.UseSqlServer("Server=HUNGNEEE\\HUNGIT;Initial Catalog=ToDoApp;Persist Security Info=False;User ID=sa;Password=12345;MultipleActiveResultSets=True;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;");
        optionsBuilder.AddInterceptors(new SqlQueryLoggingInterceptor(), new AuditLogInterceptor());
        
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
        modelBuilder.Entity<Student>().HasQueryFilter(s => s.DeletedAt == null);
        modelBuilder.Entity<Course>().HasQueryFilter(c => c.DeletedAt == null);

        base.OnModelCreating(modelBuilder);
    }    
        

    public override int SaveChanges()
    {
        foreach (var entry in ChangeTracker.Entries<ISoftDelete>().Where(e => e.State == EntityState.Deleted))
        {
            entry.State = EntityState.Modified;

            entry.Entity.DeletedAt = DateTime.UtcNow;
            entry.Entity.DeletedBy = GetCurrentUserId();
        }

        return base.SaveChanges();
    }
    private int GetCurrentUserId()
    {
        return 1; 
    }

    public EntityEntry<T> Entry<T>(T entity) where T : class
    {
            return base.Entry(entity);
    }
    
}