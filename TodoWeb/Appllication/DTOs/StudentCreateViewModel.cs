namespace TodoWeb.Application.DTOs;

public class StudentCreateViewModel
{
    public int StudentId { get; set; }
    public string? FirstName { get; set; }
    
    public string? LastName { get; set; }
    
    public DateTime DateOfBirth { get; set; }
    
    public int SchoolId { get; set; }
    
    public decimal Balance { get; set; }
    
    public List<string> Emails { get; set; } = new List<string>();
    
    public Address Address { get; set; }
    
}