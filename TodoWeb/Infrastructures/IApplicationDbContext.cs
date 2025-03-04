using Microsoft.EntityFrameworkCore;
using TodoWeb.Domains.Entities;

namespace TodoWeb.Infrastructures;

public interface IApplicationDbContext
{
    public DbSet<ToDo> Todos { get; set; }
    public DbSet<Student> Student { get; set; }

    public DbSet<School> School { get; set; }
    
    public DbSet<CourseStudent> CourseStudent { get; set; }

    public DbSet<Course> Course { get; set; }

    public int SaveChanges();
}