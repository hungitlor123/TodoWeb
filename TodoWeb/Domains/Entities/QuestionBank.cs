namespace TodoWeb.Domains.Entities;

public class QuestionBank : ISoftDelete
{
    public int Id { get; set; }
    
    public string QuestionText { get; set; }
    
    public string OptionA { get; set; }
    
    public string OptionB { get; set; }
    
    public string OptionC { get; set; }
    
    public string OptionD { get; set; }
    
    public string CorrectAnswer { get; set; }
    
    public int? DeletedBy { get; set; }
    public DateTime? DeletedAt { get; set; }
}