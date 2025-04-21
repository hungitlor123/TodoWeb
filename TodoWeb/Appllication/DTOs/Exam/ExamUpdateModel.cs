namespace TodoWeb.Application.DTOs.Exam;

public class ExamUpdateModel
{
    public int CourseId { get; set; }
    public List<int> QuestionIds { get; set; }
    
    public DateTime ExamDate { get; set; }
    
    public int TotalQuestions => QuestionIds.Count;
    
    public int TimeLimitInMinutes { get; set; }  
    
    public string Name { get; set; }  
}