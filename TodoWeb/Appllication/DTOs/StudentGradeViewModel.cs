namespace TodoWeb.Application.DTOs;

public class StudentGradeViewModel
{
    public int StudentId { get; set; }
    
    public string StudentName { get; set; } // Thêm tên học sinh
    
    public int Age { get; set; }
    public int CourseId { get; set; }
    
    public string CourseName { get; set; }
    public double AssignmentScore { get; set; }
    public double PracticalScore { get; set; }
    public double FinalScore { get; set; }
    
    public double AverageScore => (AssignmentScore + PracticalScore + FinalScore) / 3; // Tự động tính điểm trung bình
}