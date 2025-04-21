using AutoMapper;
using TodoWeb.Application.DTOs.Question;
using TodoWeb.Domains.Entities;
using TodoWeb.Infrastructures;

namespace TodoWeb.Application.Services;

public interface IQuestionService
{
    QuestionBank CreateQuestion(QuestionCreateModel question);

    void DeleteQuestion(int id);

    QuestionViewModel GetQuestion(int id);

    IEnumerable<QuestionViewModel> GetQuestions();

    QuestionBank UpdateQuestion(int id, QuestionUpdateModel question);
}

public class QuestionService : IQuestionService
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    public QuestionService(IApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    public QuestionBank CreateQuestion(QuestionCreateModel question)
    {
        var data = _mapper.Map<QuestionBank>(question);
        _dbContext.QuestionBank.Add(data);
        _dbContext.SaveChanges();
        
        return data;
    }
    
    public void DeleteQuestion(int id)
    {
        var data = _dbContext.QuestionBank.FirstOrDefault(q => q.Id == id);
        if (data == null)
        {
            throw new Exception("Question not found");
        }
        _dbContext.QuestionBank.Remove(data);
        _dbContext.SaveChanges();
    }
    public QuestionViewModel GetQuestion(int id)
    {
        var question = _dbContext.QuestionBank.FirstOrDefault(q => q.Id == id);
        if (question == null)
        {
            return null;
        }
        return _mapper.Map<QuestionViewModel>(question);
       
    }
    public IEnumerable<QuestionViewModel> GetQuestions()
    {
        var questions = _dbContext.QuestionBank.ToList();
        return _mapper.Map<IEnumerable<QuestionViewModel>>(questions);
        
    }
    public QuestionBank UpdateQuestion(int id, QuestionUpdateModel question)
    {
        var data = _dbContext.QuestionBank.FirstOrDefault(q => q.Id == id); 

        if (data == null)
        {
            throw new KeyNotFoundException($"Question with ID {id} not found.");
        }

        _mapper.Map(question, data);
        _dbContext.SaveChanges();
        return data;
    }
}