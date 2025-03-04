namespace TodoWeb.Application.DTOs;

public class StudentCreateViewModel
{
    public int StudentId { get; set; }
    public string? FirstName { get; set; }
    
    public string? LastName { get; set; }
    
    public DateTime DateOfBirth { get; set; }
    
    public string SchoolName { get; set; }
}