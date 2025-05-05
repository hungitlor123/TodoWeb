using Microsoft.AspNetCore.Mvc;
using TodoWeb.Application.DTOs;
using TodoWeb.Application.Services;
using TodoWeb.Appllication.Common;
using TodoWeb.Appllication.Params;
using TodoWeb.Domains.Entities;

namespace TodoWeb.Controller;
[ApiController]
[Route("[controller]")]
public class CourseController : ControllerBase
{
    private readonly ICourseService _courseService;
    private readonly ILogger<CourseController> _logger; 
    public CourseController(ICourseService courseService, ILogger<CourseController> logger)
    {
        _courseService = courseService;
        _logger = logger;
    }

    [HttpGet]
    public ActionResult<PagaResult<CourseViewModel>> GetAllCourses([FromQuery] CourseQueryParameters queryParameters)
    {
        var result = _courseService.GetAllCourses(queryParameters);
        return Ok(result);
    }
    [HttpGet("{id}")]
    public ActionResult<CourseViewModel> GetCourseById(int id)
    {
        _logger.LogInformation($"Get Course ID: {id}");

        if (id == 10)
        {
            _logger.LogInformation($"Warninggggg: {id}");
        }
        
        if (id <= 0)
        {
            _logger.LogWarning($"Error: {id} can not be less than 0");
            throw new Exception("Error: ID cannot be less than 0");
        }
        
        var course = _courseService.GetCourseById(id);

        if (course == null)
        {
            _logger.LogWarning($"Course not found with ID: {id}");
            return NotFound();
        }
        
        return Ok(course);
    }



    [HttpGet("detail/{id}")]
    public CourseReponseModel Details(int id)
    {
        return _courseService.CourseDetail(id);
    }

    [HttpPost]
    public CourseViewModel AddCourse(CourseCreateModel courseCreateModel)
    {
        return _courseService.CreateCourse(courseCreateModel);
    }

    [HttpPut("{id}")]
    public CourseViewModel UpdateCourse(int id, CourseUpdateModel courseUpdateModel)
    {
        return _courseService.UpdateCourse(id, courseUpdateModel);
    }

    [HttpDelete("{id}")]
    public bool DeleteCourse(int id)
    {
        return _courseService.DeleteCourse(id);
    }

    [HttpPost("assign-course")]
    public bool AssignCourseToStudent(int courseId, int studentId)
    {
        return _courseService.AssignCourseToStudent(courseId, studentId);
    }

}