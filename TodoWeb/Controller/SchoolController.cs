using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using TodoWeb.Application.DTOs;
using TodoWeb.Application.Services;
using TodoWeb.Appllication.Common;
using TodoWeb.Appllication.Params;
using TodoWeb.Constants.Enums;

namespace TodoWeb.Controller;

[ApiController]
[Route("[controller]")]
[TypeFilter(typeof(AuthorizeFilter), Arguments = [$"{nameof(Role.Admin)},{nameof(Role.Student)}"])]

public class SchoolController : ControllerBase
{
    private readonly ISchoolService _schoolService;

    public SchoolController(ISchoolService schoolService)
    {
        _schoolService = schoolService;
    }


    [HttpGet]
    public ActionResult<PagaResult<SchoolViewModel>> GetAllSchools([FromQuery] SchoolQueryParameters queryParameters)
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null)
        {
            return Unauthorized();
        }
        
        var schools = _schoolService.GetAllSchools(queryParameters);
        
        return Ok(schools);
    }
    
 


    [HttpGet("{id}")]
    public SchoolViewModel GetSchoolById(int id)
    {
        return _schoolService.GetSchoolById(id);
    }

    [HttpPost]
    public SchoolViewModel CreateSchool(SchoolCreateModel schoolCreateModel)
    {
        return _schoolService.CreateSchool(schoolCreateModel);
    }

    [HttpPut("{id}")]
    public SchoolViewModel UpdateSchool(int id, SchoolUpdateModel schoolUpdateModel)
    {
        return _schoolService.UpdateSchool(id, schoolUpdateModel);
    }

    [HttpDelete("{id}")]
    public int DeleteSchool(int id)
    {
        bool success = _schoolService.DeleteSchool(id);

        if (!success)
        {
            return -1;
        }

        return 0;
    }

    [HttpGet("{schoolId}/detail")]
    public SchoolStudentViewModel GetSchoolDetail(int schoolId)
    {
        return _schoolService.GetSchoolDetail(schoolId);
    }
}