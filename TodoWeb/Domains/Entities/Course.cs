namespace TodoWeb.Domains.Entities;

public class Course : ISoftDelete
{
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public DateTime StartDate { get; set; }
    
    public virtual ICollection<CourseStudent> CourseStudents { get; set; }
    
    public int CreatedBy { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public int UpdatedBy { get; set; }
    
    public DateTime UpdatedAt { get; set; }
    
    public int? DeletedBy { get; set; }
    public DateTime? DeletedAt { get; set; }
}