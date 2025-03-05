using Microsoft.AspNetCore.Mvc;
using TodoWeb.Application.DTOs;
using TodoWeb.Application.Services;

namespace TodoWeb.Controller;

[ApiController]
[Route("[controller]")]
public class StudentGradeController : ControllerBase
{
    private readonly IStudentGradeService _studentGradeService;

    public StudentGradeController(IStudentGradeService studentGradeService)
    {
        _studentGradeService = studentGradeService;
    }

    /// <summary>
    /// Tạo điểm số mới cho học sinh
    /// </summary>
    [HttpPost("CreateGrade")]
    public ActionResult<StudentGradeViewModel> CreateStudentGrade([FromBody] StudentGradeCreateModel studentGradeCreateModel)
    {
        var createdGrade = _studentGradeService.CreateStudentGrade(studentGradeCreateModel);
        return Created("", createdGrade);
    }

    /// <summary>
    /// Cập nhật điểm số của học sinh
    /// </summary>
    [HttpPut("UpdateGrade")]
    public ActionResult<StudentGradeViewModel> UpdateStudentGrade([FromBody] StudentGradeUpdateModel studentGradeUpdateModel)
    {
        var updatedGrade = _studentGradeService.UpdateStudentGrade(studentGradeUpdateModel);
        return Ok(updatedGrade);
    }

    /// <summary>
    /// Lấy danh sách môn học và điểm số của một học sinh
    /// </summary>
    [HttpGet("GetGrades/{studentId}")]
    public ActionResult<IEnumerable<StudentGradeViewModel>> GetStudentGrades(int studentId)
    {
        var grades = _studentGradeService.GetStudentGrades(studentId);
        return Ok(grades);
    }

    /// <summary>
    /// Tính điểm trung bình của một học sinh
    /// </summary>
    [HttpGet("GetAverage/{studentId}/{courseId}")]
    public ActionResult<double> GetStudentAverageScore(int studentId, int courseId)
    {
        var averageScore = _studentGradeService.GetStudentAverageScore(studentId, courseId);
        return Ok(new { StudentId = studentId, CourseId = courseId, AverageScore = averageScore });
    }
}
