using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TodoWeb.Application.DTOs.Exam;
using TodoWeb.Application.DTOs.ExamResult;
using TodoWeb.Domains.Entities;
using TodoWeb.Infrastructures;

namespace TodoWeb.Application.Services;

public interface IExamService
{
    void CreateExam(ExamCreateModel examCreateModel);

    void DeleteExam(int id);

    ExamViewModel GetExam(int id);

    IEnumerable<ExamViewModel> GetExams();

    Exam UpdateExam(int id, ExamUpdateModel examUpdateModel);

    Task<ExamResult> SubmitExam(StudentExamSubmission submission);
}

public class ExamService : IExamService
{
    private readonly IApplicationDbContext _dbcontext;
    private readonly IMapper _mapper;

    public ExamService(IApplicationDbContext dbcontext, IMapper mapper)
    {
        _dbcontext = dbcontext;
        _mapper = mapper;
    }
    public void CreateExam(ExamCreateModel examCreateModel)
    {
        var data = _mapper.Map<Exam>(examCreateModel);
        _dbcontext.Exam.Add(data);
        _dbcontext.SaveChanges();
    }

    public void DeleteExam(int id)
    {
        var data = _dbcontext.Exam.FirstOrDefault(x => x.Id == id);
        if (data == null)
        {
            throw new Exception("Exam Not Found");
        }
        _dbcontext.Exam.Remove(data);
        _dbcontext.SaveChanges();
    }

    public ExamViewModel GetExam(int id)
    {
        var data = _dbcontext.Exam.FirstOrDefault(x => x.Id == id);
        if (data == null)
        {
            throw new Exception("Exam Not Found");
        }
        return _mapper.Map<ExamViewModel>(data);
    }

    public IEnumerable<ExamViewModel> GetExams()
    {
        var exams = _dbcontext.Exam.ToList();
        return _mapper.Map<IEnumerable<ExamViewModel>>(exams);
    }
    public Exam UpdateExam(int id, ExamUpdateModel examUpdateModel)
    {
        var data = _dbcontext.Exam.FirstOrDefault(x => x.Id == id);
        if (data == null)
        {
            throw new Exception("Exam Not Found");
        }
        _mapper.Map(examUpdateModel, data);
        _dbcontext.SaveChanges();
        return data;
    }
    public async Task<ExamResult> SubmitExam(StudentExamSubmission submission)
    {
        var exam = await _dbcontext.Exam
            .Where(e => e.Id == submission.ExamId)  
            .FirstOrDefaultAsync(); 
        if (exam == null)
        {
            throw new KeyNotFoundException($"Exam with ID {submission.ExamId} not found.");
        }

        var questions = await _dbcontext.QuestionBank
            .Where(q => exam.QuestionIds.Contains(q.Id))  
            .ToListAsync();

        if (questions.Count != submission.StudentAnswers.Count)
        {
            throw new ArgumentException("The number of answers provided does not match the number of questions in the exam.");
        }

        int correctAnswers = 0;

        for (int i = 0; i < questions.Count; i++)
        {
            if (questions[i].CorrectAnswer == submission.StudentAnswers[i])
            {
                correctAnswers++;
            }
        }

        decimal score = 10 * (decimal)correctAnswers / questions.Count;

        var examResult = new ExamResult
        {
            StudentId = submission.StudentId,
            CourseId = exam.CourseId,
            Score = score,
            DateCalculated = DateTime.UtcNow
        };

        _dbcontext.ExamResult.Add(examResult); 
        _dbcontext.SaveChanges();

        return examResult;
    }
}