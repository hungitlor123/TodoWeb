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

    public CourseService(IApplicationDbContext dbcontext)
    {
        _dbcontext = dbcontext;
    }

    public IEnumerable<CourseViewModel> GetAllCourses()
    {
        var course = _dbcontext.Course.Select(c => new CourseViewModel
        {
            CourseId = c.Id,
            CourseName = c.Name,
            StartDate = c.StartDate,
        }).ToList();
        return course;
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
            .Where(c => c.Id == id)
            .Include(c => c.CourseStudents)
            .ThenInclude(cs => cs.Student)
            .FirstOrDefault();

        return new CourseReponseModel
        {
            Id = course.Id,
            Name = course.Name,
            Students = course.CourseStudents.Select(cs => new StudentViewModel
            {
                Id = cs.Student.Id,
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
        var course = new Course
        {
            Name = courseCreateModel.Name,
            StartDate = courseCreateModel.StartDate,
        };
        _dbcontext.Course.Add(course);
        _dbcontext.SaveChanges();
        return new CourseViewModel
        {
            CourseId = course.Id,
            CourseName = course.Name,
            StartDate = DateTime.Now
        };
    }

    public CourseViewModel UpdateCourse(int id,CourseUpdateModel courseUpdateModel)
    {
        var course = _dbcontext.Course.FirstOrDefault(c => c.Id == id);
        course.Name = courseUpdateModel.Name;
        
        _dbcontext.SaveChanges();
        return new CourseViewModel
        {
            CourseName = course.Name,
        };
    }

    public bool DeleteCourse(int id)
    {
        var course = _dbcontext.Course.FirstOrDefault(c => c.Id == id);
        _dbcontext.Course.Remove(course);
        _dbcontext.SaveChanges();
        return true;
    }
    
}