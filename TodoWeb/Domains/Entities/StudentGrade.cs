using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoWeb.Domains.Entities;

public class StudentGrade
{
    [Key]
    public int Id { get; set; } // ID duy nhất cho điểm số

    // Dùng cả 2 khóa ngoại thay vì 1
    public int CourseId { get; set; }
    public int StudentId { get; set; }

    [ForeignKey("CourseId, StudentId")]
    public virtual CourseStudent CourseStudent { get; set; }

    public double AssignmentScore { get; set; }
    public double PracticalScore { get; set; }
    public double FinalScore { get; set; }
}


