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
    
    public DbSet<StudentGrade> StudentGrades { get; set; } // Sử dụng số nhiều để nhất quán



    public DbSet<School> School { get; set; }
    


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
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
        return base.SaveChanges();
    }
}