namespace TodoWeb.Application.DTOs;

public class StudentCourseViewModel
{
    public int StudentId { get; set; }
    
    public string StudentName { get; set; }
    
    public List<CourseViewModel> Courses { get; set; }
}