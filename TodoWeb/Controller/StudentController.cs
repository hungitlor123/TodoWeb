using Microsoft.AspNetCore.Mvc;
using TodoWeb.Application.DTOs;
using TodoWeb.Application.Services;

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

    [HttpGet]
    public IEnumerable<StudentViewModel> GetStudents(int? schoolId)
    {
        return _studentService.GetStudents(schoolId);
    }

    [HttpGet("{id}")]
    public StudentViewModel? GetStudent(int id)
    {
        return _studentService.GetStudent(id);
    }

    [HttpPost]
    public StudentViewModel CreateStudent(StudentCreateViewModel studentCreateViewModel)
    {
        return _studentService.CreateStudent(studentCreateViewModel);
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