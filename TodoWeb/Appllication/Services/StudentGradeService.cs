using Microsoft.EntityFrameworkCore;
using TodoWeb.Application.DTOs;
using TodoWeb.Domains.Entities;
using TodoWeb.Infrastructures;

namespace TodoWeb.Application.Services;

public interface IStudentGradeService
{
    StudentGradeViewModel CreateStudentGrade(StudentGradeCreateModel studentGradeCreateModel);

    StudentGradeViewModel UpdateStudentGrade(StudentGradeUpdateModel studentGradeUpdateModel);

    IEnumerable<StudentGradeViewModel> GetStudentGrades(int studentId);

    double GetStudentAverageScore(int studentId, int courseId);

}

public class StudentGradeService : IStudentGradeService
{
    private readonly ApplicationDbContext _dbcontext;
    
    public StudentGradeService (ApplicationDbContext dbcontext){
        
        _dbcontext = dbcontext;

    }

    public StudentGradeViewModel CreateStudentGrade(StudentGradeCreateModel studentGradeCreateModel)
    {
        var courseStudent = _dbcontext.CourseStudent
            .Include(cs => cs.Course)
            .FirstOrDefault(cs => cs.CourseId == studentGradeCreateModel.CourseId);

        var newGrade = new StudentGrade
        {
            CourseId = studentGradeCreateModel.CourseId,
            StudentId = studentGradeCreateModel.StudentId,
            AssignmentScore = studentGradeCreateModel.AssignmentScore,
            PracticalScore = studentGradeCreateModel.PracticalScore,
            FinalScore = studentGradeCreateModel.FinalScore,
        };
        _dbcontext.StudentGrades.Add(newGrade);
        _dbcontext.SaveChanges();
        
        var course = _dbcontext.Course.FirstOrDefault(c => c.Id == studentGradeCreateModel.CourseId);
        var student = _dbcontext.Student.FirstOrDefault(s => s.Id == studentGradeCreateModel.StudentId);
        return new StudentGradeViewModel
        {
            StudentId = studentGradeCreateModel.StudentId,
            CourseId = studentGradeCreateModel.CourseId,
            StudentName = student.FirstName + " " + student.LastName,
            Age = student.Age,
            CourseName = course.Name,
            AssignmentScore = studentGradeCreateModel.AssignmentScore,
            PracticalScore = studentGradeCreateModel.PracticalScore,
            FinalScore = studentGradeCreateModel.FinalScore
        };
    }

    public StudentGradeViewModel UpdateStudentGrade(StudentGradeUpdateModel studentGradeUpdateModel)
    {
        var studentGrade = _dbcontext.StudentGrades
            .Include(sg => sg.CourseStudent)
            .ThenInclude(cs => cs.Course)
            .FirstOrDefault(sg => sg.StudentId == studentGradeUpdateModel.StudentId);
        
        studentGrade.AssignmentScore = studentGradeUpdateModel.AssignmentScore;
        studentGrade.PracticalScore = studentGradeUpdateModel.PracticalScore;
        studentGrade.FinalScore = studentGradeUpdateModel.FinalScore;
        
        _dbcontext.SaveChanges();
        var course = _dbcontext.Course.FirstOrDefault(c => c.Id == studentGradeUpdateModel.CourseId);
        var student = _dbcontext.Student.FirstOrDefault(s => s.Id == studentGradeUpdateModel.StudentId);
        return new StudentGradeViewModel
        {
            StudentId = studentGradeUpdateModel.StudentId,
            CourseId = studentGradeUpdateModel.CourseId,
            StudentName = student.FirstName + " " + student.LastName,
            Age = student.Age,
            CourseName = course.Name,
            AssignmentScore = studentGradeUpdateModel.AssignmentScore,
            PracticalScore = studentGradeUpdateModel.PracticalScore,
            FinalScore = studentGradeUpdateModel.FinalScore
        };
    }


    public IEnumerable<StudentGradeViewModel> GetStudentGrades(int studentId)
    {
        var courseStudent = _dbcontext.CourseStudent
            .Where(cs => cs.StudentId == studentId)
            .Include(cs => cs.Course)
            .Include(cs => cs.StudentGrade)
            .Include(cs => cs.Student)
            .ToList();

        return courseStudent.Select(cs => new StudentGradeViewModel
        {
            StudentId = cs.StudentId,
            StudentName = cs.Student.FirstName + " " + cs.Student.LastName,
            Age = cs.Student.Age,
            CourseId = cs.CourseId,
            CourseName = cs.Course.Name,
            AssignmentScore = cs.StudentGrade != null ? cs.StudentGrade.AssignmentScore : 0,
            PracticalScore = cs.StudentGrade != null ? cs.StudentGrade.PracticalScore : 0,
            FinalScore = cs.StudentGrade != null ? cs.StudentGrade.FinalScore : 0
        });
    }

    public double GetStudentAverageScore(int studentId, int courseId)
    {
        var grades = _dbcontext.StudentGrades
            .Where(sg => sg.StudentId == studentId && sg.CourseId == courseId)
            .ToList();

        if (!grades.Any()) return 0;

        return grades.Average(sg => (sg.AssignmentScore + sg.PracticalScore + sg.FinalScore) / 3);
    }
    
    
}

