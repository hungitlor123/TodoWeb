using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TodoWeb.Application.DTOs;
using TodoWeb.Domains.Entities;
using TodoWeb.Infrastructures;

namespace TodoWeb.Application.Services;

public interface ICourseService
{
    CourseViewModel GetCourseById(int id);

    IEnumerable<CourseViewModel> GetAllCourses();

    CourseViewModel CreateCourse(CourseCreateModel courseCreateModel);

    CourseViewModel UpdateCourse(int id, CourseUpdateModel courseUpdateModel);

    bool DeleteCourse(int id);

    CourseReponseModel CourseDetail(int id);

    bool AssignCourseToStudent(int courseId, int studentId);


}

public class CourseService : ICourseService
{
    private readonly IApplicationDbContext _dbcontext;
    private readonly IMapper _mapper;

    public CourseService(IApplicationDbContext dbcontext,IMapper mapper)
    {
        _dbcontext = dbcontext;
        _mapper = mapper;
    }

    public IEnumerable<CourseViewModel> GetAllCourses()
    {
        var query = _dbcontext.Course.AsQueryable();
        
        /*var course = _dbcontext.Course.ToList();
        var result = course.
            Select(course => _mapper.Map<CourseViewModel>(course)).
            ToList();
        var result = _mapper.Map<List<CourseViewModel>>(course);*/
        
        var result = _mapper.
            ProjectTo<CourseViewModel>(query).
            ToList();
        
        return result;
    }

    public CourseViewModel GetCourseById(int id)
    {
        var course = _dbcontext.Course.FirstOrDefault(c  => c.Id == id);

        return new CourseViewModel
        {
            CourseId = course.Id,
            CourseName = course.Name,
            StartDate = course.StartDate,
        };
    }

    public CourseReponseModel CourseDetail(int id)
    {
        var course = _dbcontext.Course.Where(c => c.Id == id).Select(c => new CourseReponseModel
        {
            Id = c.Id,
            Name = c.Name,
            Students = c.CourseStudents.Select(cs => new StudentViewModel
            {
                Id = cs.StudentId,
                FullName = cs.Student.FirstName + " " + cs.Student.LastName,
                SchoolName = cs.Student.School.Name,
                Age = cs.Student.Age,
                Balance = cs.Student.Balance,
            }).ToList()
        }).FirstOrDefault();
        
        return course;
    }
    
    /*public CourseReponseModel CourseDetail2(int id)
    {
        var course = _dbcontext.Course
            .Where(c => c.CourseId == id)
            .Include(c => c.CourseStudents)
            .ThenInclude(cs => cs.Student)
            .FirstOrDefault();

        return new CourseReponseModel
        {
            CourseId = course.CourseId,
            CourseName = course.CourseName,
            Students = course.CourseStudents.Select(cs => new StudentViewModel
            {
                CourseId = cs.Student.CourseId,
                FullName = cs.Student.FirstName + " " + cs.Student.LastName
            }).ToList() 
        };
    }*/

    public bool AssignCourseToStudent(int courseId, int studentId)
    {
        var courseStudnet = new CourseStudent
        {
            CourseId = courseId,
            StudentId = studentId
        };
            _dbcontext.CourseStudent.Add(courseStudnet);
            _dbcontext.SaveChanges();
            return true;
        
    }
    public CourseViewModel CreateCourse(CourseCreateModel courseCreateModel)
    {
        var course = _mapper.Map<Course>(courseCreateModel);
        _dbcontext.Course.Add(course);
        _dbcontext.SaveChanges();
        
        var courseViewModel = _mapper.Map<CourseViewModel>(course);
        
        return courseViewModel;
    }

    public CourseViewModel UpdateCourse(int id,CourseUpdateModel courseUpdateModel)
    {
        var course = _dbcontext.Course.FirstOrDefault(c => c.Id == id);
        if (course == null)
        {
            return null;
        }
        if (string.IsNullOrEmpty(courseUpdateModel.Name))
        {
            throw new ArgumentException("Course name cannot be null or empty.");
        }
        _mapper.Map(courseUpdateModel, course);
        
        _dbcontext.SaveChanges();
        return _mapper.Map<CourseViewModel>(course);
    }

    public bool DeleteCourse(int id)
    {
        var course = _dbcontext.Course.FirstOrDefault(c => c.Id == id);
        _dbcontext.Course.Remove(course);
        _dbcontext.SaveChanges();
        return true;
    }
    
}