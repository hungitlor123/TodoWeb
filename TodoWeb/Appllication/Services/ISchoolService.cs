using TodoWeb.Application.DTOs;
using TodoWeb.Domains.Entities;
using TodoWeb.Infrastructures;

namespace TodoWeb.Application.Services;

public interface ISchoolService
{
    IEnumerable<SchoolViewModel> GetAllSchools();

    SchoolViewModel GetSchoolById(int id);

    SchoolViewModel CreateSchool(SchoolCreateModel schoolCreateModel);

    SchoolViewModel UpdateSchool(int id, SchoolUpdateModel schoolUpdateModel);

    public bool DeleteSchool(int id);
}

public class SchoolService : ISchoolService
{
    private readonly IApplicationDbContext _dbcontext;

    public SchoolService(IApplicationDbContext dbcontext)
    {
        _dbcontext = dbcontext;
    }

    public IEnumerable<SchoolViewModel> GetAllSchools()
    {
        var schools = _dbcontext.School.Select(s => new SchoolViewModel
        {
            Name = s.Name,
            Address = s.Address,
        }).ToList();
        
        return schools;
    }


    public SchoolViewModel GetSchoolById(int id)
    {
        var school = _dbcontext.School.FirstOrDefault(s => s.Id == id);

        return new SchoolViewModel
        {
            Name = school.Name,
            Address = school.Address,
        };
    }

    public SchoolViewModel CreateSchool(SchoolCreateModel schoolCreateModel)
    {
        var School = new School
        {
            Id = schoolCreateModel.Id,
            Name = schoolCreateModel.Name,
            Address = schoolCreateModel.Address,

        };
        _dbcontext.School.Add(School);
        _dbcontext.SaveChanges();
        return new SchoolViewModel
        {
            Name = School.Name,
            Address = School.Address,
        };
    }

    public SchoolViewModel UpdateSchool(int id, SchoolUpdateModel schoolUpdateModel)
    {
        var school = _dbcontext.School.FirstOrDefault(s => s.Id == id);
        school.Name = schoolUpdateModel.Name;
        school.Address = schoolUpdateModel.Address;
        _dbcontext.SaveChanges();

        return new SchoolViewModel
        {
            Name = school.Name,
            Address = school.Address,
        };
    }

    public bool DeleteSchool(int id)
    {
        var school = _dbcontext.School.FirstOrDefault(s => s.Id == id);
        if (school != null) return false;
        _dbcontext.School.Remove(school);
        _dbcontext.SaveChanges();
        return true;
    }

}