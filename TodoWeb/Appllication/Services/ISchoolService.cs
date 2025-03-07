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

    SchoolStudentViewModel GetSchoolDetail(int schoolId);
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
            Id = s.Id,
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
            Id = school.Id,
            Name = school.Name,
            Address = school.Address,
        };
    }

    public SchoolViewModel CreateSchool(SchoolCreateModel schoolCreateModel)
    {
        var School = new School
        {
            
            Name = schoolCreateModel.Name,
            Address = schoolCreateModel.Address,

        };
        var state = _dbcontext.Entry(School).State;
        
        _dbcontext.School.Add(School);
        
        state = _dbcontext.Entry(School).State;

        _dbcontext.SaveChanges();

        state = _dbcontext.Entry(School).State;


        return new SchoolViewModel
        {
            Id =  School.Id,
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
            Id = school.Id,
            Name = school.Name,
            Address = school.Address,
        };
    }

    public bool DeleteSchool(int id)
    {
        var school = _dbcontext.School.FirstOrDefault(s => s.Id == id);
        if (school == null) return false;
        _dbcontext.School.Remove(school);
        _dbcontext.SaveChanges();
        return true;
    }

    public SchoolStudentViewModel GetSchoolDetail(int schoolId)
    {
        var school = _dbcontext.School.Find(schoolId);
        if (school == null)
        {
            return null;
        }
        
        _dbcontext.Entry(school).Collection(s => s.Students).Load();
        
        var student = school.Students;


        return new SchoolStudentViewModel
        {
            Id = school.Id,
            Name = school.Name,
            Address = school.Address,
            Students = student.Select(x => new StudentViewModel
            {
                Id = x.Id,
                FullName = x.FirstName + " " + x.LastName,
                Age = x.Age,
                SchoolName = x.School.Name,
            }).ToList()
        };

    }


}