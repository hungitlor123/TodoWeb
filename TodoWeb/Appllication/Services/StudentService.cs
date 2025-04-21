using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TodoWeb.Application.DTOs;
using TodoWeb.Appllication.Common;
using TodoWeb.Appllication.Params;
using TodoWeb.Domains.Entities;
using TodoWeb.Infrastructures;

namespace TodoWeb.Application.Services;

 public interface IStudentService
{
    PagaResult<StudentViewModel> GetStudents(StudentQueryParameters queryParameters);

     StudentCourseViewModel GetStudentDetails(int id);

     StudentViewModel GetStudent(int studentId);

     StudentViewModel CreateStudent(StudentCreateViewModel studentCreateViewModel);

     StudentViewModel UpdateStudent(int id, StudentUpdateViewModel studentUpdateViewModel);

     bool DeleteStudent(int id);
}

public class StudentService : IStudentService
{
    private readonly IApplicationDbContext _dbcontext;
    private readonly IMapper _mapper;

    public StudentService(IApplicationDbContext dbcontext, IMapper mapper)
    {
        _dbcontext = dbcontext;
        _mapper = mapper;
    }

    public PagaResult<StudentViewModel> GetStudents(StudentQueryParameters queryParameters)
    {
        var query = _dbcontext.Student
            .Include(s => s.School)
            .AsQueryable();

        // Lọc theo SchoolId nếu có
        if (queryParameters.SchoolId != null)
        {
            query = query.Where(s => s.School.Id == queryParameters.SchoolId);
        }

        // Tìm kiếm theo FullName
        if (!string.IsNullOrWhiteSpace(queryParameters.Keyword))
        {
            query = query.Where(s =>
                (s.FirstName + " " + s.LastName).Contains(queryParameters.Keyword));
        }

        // Sắp xếp theo SortBy
        query = queryParameters.SortBy.ToLower() switch
        {
            "fullname" => queryParameters.SortDesc
                ? query.OrderByDescending(s => s.FirstName + " " + s.LastName)
                : query.OrderBy(s => s.FirstName + " " + s.LastName),
            "age" => queryParameters.SortDesc ? query.OrderByDescending(s => s.Age) : query.OrderBy(s => s.Age),
            "balance" => queryParameters.SortDesc ? query.OrderByDescending(s => s.Balance) : query.OrderBy(s => s.Balance),
            _ => queryParameters.SortDesc ? query.OrderByDescending(s => s.Id) : query.OrderBy(s => s.Id)
        };

        var totalItems = query.Count();

        var students = query
            .Skip((queryParameters.PageIndex - 1) * queryParameters.PageSize)
            .Take(queryParameters.PageSize)
            .ToList();

        var viewModels = students.Select(s => new StudentViewModel
        {
            Id = s.Id,
            FullName = s.FirstName + " " + s.LastName,
            Age = s.Age,
            Balance = s.Balance,
            SchoolName = s.School.Name
        }).ToList();

        return new PagaResult<StudentViewModel>
        {
            TotalItems = totalItems,
            PageIndex = queryParameters.PageIndex,
            PageSize = queryParameters.PageSize,
            TotalPages = (int)Math.Ceiling(totalItems / (double)queryParameters.PageSize),
            Items = viewModels
        };
    }


    public StudentViewModel GetStudent(int studentId)
    {
        var student = _dbcontext.Student
            .Include(student => student.School)
            .FirstOrDefault(student => student.Id == studentId);
        if (student == null)
        {
            return null;
        }

        return new StudentViewModel
        {
            Id = student.Id,
            FullName = student.FirstName + " " + student.LastName,
            Age = student.Age,
            SchoolName = student.School.Name,
        };
    }

    public StudentViewModel CreateStudent(StudentCreateViewModel studentCreateViewModel)
    {
        var school = _dbcontext.School
            .AsNoTracking()
            .FirstOrDefault(s => s.Id == studentCreateViewModel.SchoolId);

        if (school == null)
        {
            throw new ArgumentException("School not found");
        }

        bool isIdExists = _dbcontext.Student
            .Any(s => s.Id == studentCreateViewModel.StudentId);

        if (isIdExists)
        {
            throw new ArgumentException("Student ID already exists!");
        }

        var newStudent = new Student
        {
            Id = studentCreateViewModel.StudentId,
            FirstName = studentCreateViewModel.FirstName,
            LastName = studentCreateViewModel.LastName,
            DateOfBirth = studentCreateViewModel.DateOfBirth,
            SId = school.Id // Chỉ gán ID, không gán trực tiếp đối tượng School
        };

        _dbcontext.Student.Add(newStudent);
        _dbcontext.SaveChanges();

        return new StudentViewModel
        {
            Id = newStudent.Id,
            FullName = $"{newStudent.FirstName} {newStudent.LastName}",
            Age = newStudent.Age,
            SchoolName = school.Name, // Dùng school trực tiếp, tránh truy xuất lại từ newStudent
        };
    }


    public StudentViewModel UpdateStudent(int id ,StudentUpdateViewModel studentUpdateViewModel)
    {
        var student = _dbcontext.Student.Include(sc => sc.School) .FirstOrDefault(s => s.Id == id);
        var school = _dbcontext.School.FirstOrDefault(s => s.Name == studentUpdateViewModel.SchoolName);
        if (student == null)
        {
            throw new ArgumentException("Student not found");
        }

        if (school == null)
        {
            throw new ArgumentException("School not found");
        }

        if (!string.IsNullOrEmpty(studentUpdateViewModel.FirstName))
        {
            student.FirstName = studentUpdateViewModel.FirstName;
        }

        if (!string.IsNullOrEmpty(studentUpdateViewModel.LastName))
        {
            student.LastName = studentUpdateViewModel.LastName;
        }

        if (studentUpdateViewModel.DateOfBirth != default(DateTime))
        {
            student.DateOfBirth = studentUpdateViewModel.DateOfBirth;
        }
        student.Balance = studentUpdateViewModel.Balance;
        student.School = school;
        
        _dbcontext.SaveChanges();
        return new StudentViewModel
        {
            Id = student.Id,
            FullName = student.FirstName + " " + student.LastName,
            Age = student.Age,
            Balance = student.Balance,  
            SchoolName = student.School.Name,
            
            
        };
    }
    

    public bool DeleteStudent(int id)
    {
        var student = _dbcontext.Student.FirstOrDefault(s => s.Id == id);
        if (student == null) return false;
        _dbcontext.Student.Remove(student);
        _dbcontext.SaveChanges();
        return true;
    }

    public StudentCourseViewModel GetStudentDetails(int id)
    {
        var student = _dbcontext.Student.
            Include(s => s.CourseStudents).
            ThenInclude(cs => cs.Course).FirstOrDefault(s => s.Id == id);

        if (student == null)
        {
            return null;
        }
        
        return _mapper.Map<StudentCourseViewModel>(student);
    }
    
}

