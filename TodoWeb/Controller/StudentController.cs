using Microsoft.AspNetCore.Mvc;
using TodoWeb.Application.DTOs;
using TodoWeb.Application.Services;
using TodoWeb.Appllication.Common;
using TodoWeb.Appllication.Params;

namespace TodoWeb.Controller;


[ApiController]
[Route("[controller]")]
public class StudentController : ControllerBase
{
    private readonly IStudentService _studentService;

    public StudentController(IStudentService studentService)
    {
        _studentService = studentService;
    }
    
    [HttpGet("GetStudents")]
    public StudentCourseViewModel GetStudentDetails(int id)
    {
        return _studentService.GetStudentDetails(id);
    }
    
    [HttpGet("GetStudents2")]
    public IEnumerable<StudentViewModel> GetStudent2()
    {
        return _studentService.GetStudents2();
    }
    
    
    [HttpGet]
    public ActionResult<PagaResult<StudentViewModel>> GetStudents([FromQuery] StudentQueryParameters queryParameters)
    {
        var result = _studentService.GetStudents(queryParameters);
        return Ok(result);
    }


    [HttpGet("{id}")]
    public StudentViewModel? GetStudent(int id)
    {
        return _studentService.GetStudent(id);
    }

    [HttpPost]
    public IActionResult CreateStudent(StudentCreateViewModel studentCreateViewModel)
    {
        var result = _studentService.CreateStudent(studentCreateViewModel);
        return Ok(result);
    }

    [HttpPut("{id}")]
    public StudentViewModel UpdateStudent(int id, StudentUpdateViewModel studentUpdateViewModel)
    {
        return _studentService.UpdateStudent(id, studentUpdateViewModel);
    }

    [HttpDelete("{id}")]
    public int DeleteStudent(int id)
    {
        bool isDeleted = _studentService.DeleteStudent(id);
    
        if (!isDeleted)
        {
            return -1;
        }

        return 0;
    }

    
}