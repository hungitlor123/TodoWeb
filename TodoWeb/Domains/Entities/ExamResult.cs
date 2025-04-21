namespace TodoWeb.Domains.Entities;

public class ExamResult
{
    public int Id { get; set; }
    
    public int CourseId { get; set; }
    
    public int StudentId { get; set; }
    
    public decimal Score { get; set; }
    
    public DateTime DateCalculated { get; set; }
}