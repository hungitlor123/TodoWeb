using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using TodoWeb.Domains.Entities;

namespace TodoWeb.Infrastructures;

public interface IApplicationDbContext
{
    public DbSet<ToDo> Todos { get; set; }
    public DbSet<Student> Student { get; set; }

    public DbSet<School> School { get; set; }
    
    public DbSet<CourseStudent> CourseStudent { get; set; }
    
     public DbSet<StudentGrade> StudentGrades { get; set; } 
    
     public DbSet<Exam> Exam { get; set; }
    
     public DbSet<ExamResult> ExamResult { get; set; }
    
     public DbSet<QuestionBank> QuestionBank { get; set; }

    public DbSet<Course> Course { get; set; }
    
    public DbSet<AuditLog> AuditLog { get; set; }
    
    public DbSet<User> Users { get; set; }
    
    public EntityEntry<T> Entry<T>(T entity) where T : class;

    public int SaveChanges();

    public Task<int> SaveChangesAsync();
}