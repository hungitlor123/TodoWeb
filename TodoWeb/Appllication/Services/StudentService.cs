using Microsoft.EntityFrameworkCore;
using TodoWeb.Application.DTOs;
using TodoWeb.Domains.Entities;
using TodoWeb.Infrastructures;

namespace TodoWeb.Application.Services;

 public interface IStudentService
{
     public IEnumerable<StudentViewModel> GetStudents(int? schoolId);

     StudentCourseViewModel GetStudentDetails(int id);

     StudentViewModel GetStudent(int studentId);

     StudentViewModel CreateStudent(StudentCreateViewModel studentCreateViewModel);

     StudentViewModel UpdateStudent(int id, StudentUpdateViewModel studentUpdateViewModel);

     bool DeleteStudent(int id);
}

public class StudentService : IStudentService
{
    private readonly IApplicationDbContext _dbcontext;

    public StudentService(IApplicationDbContext dbcontext)
    {
        _dbcontext = dbcontext;
    }

    public IEnumerable<StudentViewModel> GetStudents(int? schoolId)
    {
        //SELECT * FORM Student
        //join School ON Student.SId = School.Id
        var query = _dbcontext.Student
            .Include(student => student.School)
            .AsQueryable();
        if (schoolId != null)
        {
            //SELECT * FORM Student
            //join School ON Student.SId = School.Id
            //WHERE School.Id = SchoolId   
            query = query.Where(s => s.School.Id == schoolId);
        }
        //SELECT * FORM Student
            //Student.Id
            //Student.FristName + Student.LastName AS FullName
            //Student.Age , Student.SchoolName AS SchoolName
        //FORM Student
            //join School ON Student.SId = School.Id
        //WHERE School.Id = SchoolId (depend if schoolId is not null)
        return query.Select(s => new StudentViewModel
            {
                Id = s.Id,
                FullName = s.FirstName + " " + s.LastName,
                Age = s.Age,
                SchoolName = s.School.Name,

            }).ToList();
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
        var student = _dbcontext.Student.Include(s => s.CourseStudents).ThenInclude(cs => cs.Course).FirstOrDefault(s => s.Id == id);

        return new StudentCourseViewModel
        {
            StudentId = student.Id,
            StudentName = student.FirstName + " " + student.LastName,
            Courses = student.CourseStudents.Select(x => new CourseViewModel
            {
                CourseId = x.CourseId,
                CourseName = x.Course.Name,
                StartDate = x.Course.StartDate,
                
                
            }).ToList()
            
        };
    }
    
}

