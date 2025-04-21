namespace TodoWeb.Application.DTOs.ExamResult;

public class ExamResultViewModel
{
    public int Id { get; set; }
    
    public int CourseId { get; set; }
    
    public int StudentId { get; set; }
    
    public decimal Score { get; set; }
    
    public DateTime DateCalculated { get; set; }
}