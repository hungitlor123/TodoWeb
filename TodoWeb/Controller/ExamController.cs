using Microsoft.AspNetCore.Mvc;
using TodoWeb.Application.DTOs.Exam;
using TodoWeb.Application.Services;
using TodoWeb.Appllication.Common;
using TodoWeb.Appllication.Params;

namespace TodoWeb.Controller;

[Route("[controller]")]
[ApiController]
public class ExamController : ControllerBase
{
    private readonly IExamService _examService;

    public ExamController(IExamService examService)
    {
        _examService = examService;
    }
    [HttpPost]
    public ActionResult CreateExam([FromBody] ExamCreateModel exam)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new { status = "error", message = "Invalid data", errors = ModelState });
        }
        _examService.CreateExam(exam);
        return Ok(exam);
    }
    
    [HttpDelete("{id}")]
    public ActionResult DeleteExam(int id)
    {
        try
        {
            _examService.DeleteExam(id);
            return Ok(new { status = "success", message = "Exam deleted" });
        }
        catch (Exception e)
        {
            return NotFound(new { status = "error", message = e.Message });
        }
    }
    [HttpGet("{id}")]
    public ActionResult GetExam(int id)
    {
        var exam = _examService.GetExam(id);
        if (exam == null)
        {
            return NotFound(new { status = "error", message = "Exam not found" });
        }
        return Ok(exam);
    }
    
    [HttpGet]
    public ActionResult<PagaResult<ExamViewModel>> GetExams([FromQuery] ExamQueryParameters queryParameters)
    {
        var result = _examService.GetExams(queryParameters);
        return Ok(result);
    }

    
    [HttpPut("{id}")]
    public ActionResult UpdateExam(int id, [FromBody] ExamUpdateModel exam)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new { status = "error", message = "Invalid data", errors = ModelState });
        }
        
        var updatedExam = _examService.UpdateExam(id, exam);
        return Ok(updatedExam);
    }
}