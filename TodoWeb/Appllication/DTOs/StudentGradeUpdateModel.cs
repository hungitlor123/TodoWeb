namespace TodoWeb.Application.DTOs;

public class StudentGradeUpdateModel
{
    public int StudentId { get; set; }
    public int CourseId { get; set; }
    public double AssignmentScore { get; set; }
    public double PracticalScore { get; set; }
    public double FinalScore { get; set; }
}