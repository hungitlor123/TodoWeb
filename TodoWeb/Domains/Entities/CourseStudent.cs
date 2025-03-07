using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoWeb.Domains.Entities;

public class CourseStudent
{
    [Key]
    [Column(Order = 1)]
    public int CourseId { get; set; }
    public virtual Course Course { get; set; }

    [Key]
    [Column(Order = 2)]
    public int StudentId { get; set; }
    public virtual Student Student { get; set; }

    public virtual StudentGrade StudentGrade { get; set; }
}
