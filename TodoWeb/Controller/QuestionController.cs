using Microsoft.AspNetCore.Mvc;
using TodoWeb.Application.DTOs.Question;
using TodoWeb.Application.Services;
using TodoWeb.Appllication.Common;
using TodoWeb.Appllication.Params;

namespace TodoWeb.Controller;

[Route("[controller]")]
[ApiController]
public class QuestionController : ControllerBase
{
    private readonly IQuestionService _questionService;

    public QuestionController(IQuestionService questionService)
    {
        _questionService = questionService;
    }
    
    [HttpPost]
    public ActionResult CreateQuestion([FromBody] QuestionCreateModel question)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new { status = "error", message = "Invalid data", errors = ModelState });
        }
        var createdQuestion = _questionService.CreateQuestion(question);
        return Ok(createdQuestion);
    }
    [HttpDelete("{id}")]
    public ActionResult DeleteQuestion(int id)
    {
        try
        {
            _questionService.DeleteQuestion(id);
            return Ok(new { status = "success", message = "Question deleted" });
        }
        catch (Exception e)
        {
            return NotFound(new { status = "error", message = e.Message });
        }
    }
    [HttpGet("{id}")]
    public ActionResult GetQuestion(int id)
    {
        var question = _questionService.GetQuestion(id);
        if (question == null)
        {
            return NotFound(new { status = "error", message = "Question not found" });
        }
        return Ok(question);
    }
    [HttpGet]
    public ActionResult<PagaResult<QuestionViewModel>> GetQuestions([FromQuery] QuestionQueryParameters queryParameters)
    {
        var questions = _questionService.GetQuestions(queryParameters);
        return Ok(questions);
    }
    [HttpPut("{id}")]
    public ActionResult UpdateQuestion(int id, [FromBody] QuestionUpdateModel question)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new { status = "error", message = "Invalid data", errors = ModelState });
        }
        try
        {
            var updatedQuestion = _questionService.UpdateQuestion(id, question);
            return Ok(updatedQuestion);
        }
        catch (Exception e)
        {
            return NotFound(new { status = "error", message = e.Message });
        }
    }

}