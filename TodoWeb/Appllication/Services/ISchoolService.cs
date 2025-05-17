using TodoWeb.Application.DTOs;
using TodoWeb.Appllication.Common;
using TodoWeb.Appllication.Params;
using TodoWeb.Domains.Entities;
using TodoWeb.Infrastructures;

namespace TodoWeb.Application.Services;

public interface ISchoolService
{
    PagaResult<SchoolViewModel> GetAllSchools(SchoolQueryParameters queryParameters);

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

    public PagaResult<SchoolViewModel> GetAllSchools(SchoolQueryParameters queryParameters)
    {
        var query = _dbcontext.School.AsQueryable();

        // Tìm kiếm theo tên hoặc địa chỉ
        if (!string.IsNullOrWhiteSpace(queryParameters.Keyword))
        {
            query = query.Where(s => s.Name.Contains(queryParameters.Keyword)
                                     || s.Address.Contains(queryParameters.Keyword));
        }

        // Sắp xếp
        query = queryParameters.SortBy.ToLower() switch
        {
            "name" => queryParameters.SortDesc ? query.OrderByDescending(s => s.Name) : query.OrderBy(s => s.Name),
            "address" => queryParameters.SortDesc ? query.OrderByDescending(s => s.Address) : query.OrderBy(s => s.Address),
            _ => queryParameters.SortDesc ? query.OrderByDescending(s => s.Id) : query.OrderBy(s => s.Id)
        };

        var totalItems = query.Count();

        var items = query
            .Skip((queryParameters.PageIndex - 1) * queryParameters.PageSize)
            .Take(queryParameters.PageSize)
            .Select(s => new SchoolViewModel
            {
                Id = s.Id,
                Name = s.Name,
                Address = s.Address
            }).ToList();

        return new PagaResult<SchoolViewModel>
        {
            TotalItems = totalItems,
            PageIndex = queryParameters.PageIndex,
            PageSize = queryParameters.PageSize,
            TotalPages = (int)Math.Ceiling(totalItems / (double)queryParameters.PageSize),
            Items = items
        };
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