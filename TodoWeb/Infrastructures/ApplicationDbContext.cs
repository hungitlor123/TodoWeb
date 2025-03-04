using Microsoft.EntityFrameworkCore;
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


    public DbSet<School> School { get; set; }
    


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=HUNGNEEE\\HUNGIT;Initial Catalog=ToDoApp;Persist Security Info=False;User ID=sa;Password=12345;MultipleActiveResultSets=True;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Student>()
            .Property(x => x.Age)
            .HasComputedColumnSql("DATEDIFF(YEAR, DateOfBirth, GETDATE())");
        modelBuilder.Entity<Student>()
            .HasMany(student => student.CourseStudents)
            .WithOne(student => student.Student)
            .HasForeignKey(courseStudent => courseStudent.StudentId);
        
        modelBuilder.Entity<Course>()
            .HasMany(course => course.CourseStudents)
            .WithOne(course => course.Course)
            .HasForeignKey(courseStudent => courseStudent.CourseId);
        
        modelBuilder.Entity<CourseStudent>()
            .HasKey(courseStudent => new { courseStudent.CourseId, courseStudent.StudentId });

        modelBuilder.ApplyConfiguration(new CourseMapping());
        
        base.OnModelCreating(modelBuilder);
        
    }
    
        

    public int SaveChanges()
    {
        return base.SaveChanges();
    }
}