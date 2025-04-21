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

    public CourseController(ICourseService courseService)
    {
        _courseService = courseService;
    }

    [HttpGet]
    public ActionResult<PagaResult<CourseViewModel>> GetAllCourses([FromQuery] CourseQueryParameters queryParameters)
    {
        var result = _courseService.GetAllCourses(queryParameters);
        return Ok(result);
    }
    [HttpGet("{id}")]
    public CourseViewModel GetCourseById(int id)
    {
        return _courseService.GetCourseById(id);
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