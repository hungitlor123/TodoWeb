using Microsoft.AspNetCore.Mvc;
using TodoWeb.Application.DTOs;
using TodoWeb.Application.Services;

namespace TodoWeb.Controller;

[ApiController]
[Route("[controller]")]

public class SchoolController : ControllerBase
{
    private readonly ISchoolService _schoolService;

    public SchoolController(ISchoolService schoolService)
    {
        _schoolService = schoolService;
    }

    [HttpGet]
    public IEnumerable<SchoolViewModel> GetAllSchools()
    {
        return _schoolService.GetAllSchools();
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