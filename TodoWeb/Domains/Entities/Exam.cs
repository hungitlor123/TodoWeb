namespace TodoWeb.Domains.Entities;

public class Exam : ISoftDelete
{
    public int Id { get; set; }
    
    public int CourseId { get; set; }
    public List<int> QuestionIds { get; set; }
    
    public DateTime ExamDate { get; set; }
    
    public int TotalQuestions => QuestionIds.Count;
    
    public int TimeLimitInMinutes { get; set; }  
    
    public string Name { get; set; }  

    public int? DeletedBy { get; set; }  
    public DateTime? DeletedAt { get; set; }  
}