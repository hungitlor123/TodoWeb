namespace TodoWeb.Domains.Entities;

public class Course
{
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public DateTime StartDate { get; set; }
    
    public virtual ICollection<CourseStudent> CourseStudents { get; set; }
}