using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoWeb.Domains.Entities;

public class CourseStudent
{
    [Key]
    [Column(Order = 1)]
    public int CourseId { get; set; }
    public Course Course { get; set; }

    [Key]
    [Column(Order = 2)]
    public int StudentId { get; set; }
    public Student Student { get; set; }

    public StudentGrade StudentGrade { get; set; }
}
