using Microsoft.AspNetCore.Mvc;
using TodoWeb.Application.DTOs.ExamResult;
using TodoWeb.Application.Services;
using TodoWeb.Domains.Entities;

namespace TodoWeb.Controller;

[Route("[controller]")]
[ApiController]
public class ExamResultController : ControllerBase
{
    private readonly IExamService _examService;

    public ExamResultController(IExamService examService)
    {
        _examService = examService;
    }
    [HttpPost]
    public async Task<ActionResult<ExamResult>> SubmitExam([FromBody] StudentExamSubmission submission)
    {
        try
        {
            var examResult = await _examService.SubmitExam(submission);
            return Ok(examResult);
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(new { status = "error", message = e.Message });
        }
        catch (ArgumentException e)
        {
            return BadRequest(new { status = "error", message = e.Message });
        }
    }
}