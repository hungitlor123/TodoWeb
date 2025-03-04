using System.ComponentModel.DataAnnotations.Schema;

namespace TodoWeb.Application.DTOs;

public class StudentUpdateViewModel
{
    public string? FirstName { get; set; }
    
    public string? LastName { get; set; }
    
    public DateTime DateOfBirth { get; set; }
    
    public decimal Balance { get; set; }
    
    public string SchoolName { get; set; }
    
    [NotMapped]
    public int Age { get => (DateTime.Now - DateOfBirth).Days / 365; } 
    
    
    
}