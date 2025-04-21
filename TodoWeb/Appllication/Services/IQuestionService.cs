using AutoMapper;
using TodoWeb.Application.DTOs.Question;
using TodoWeb.Appllication.Common;
using TodoWeb.Appllication.Params;
using TodoWeb.Domains.Entities;
using TodoWeb.Infrastructures;

namespace TodoWeb.Application.Services;

public interface IQuestionService
{
    QuestionBank CreateQuestion(QuestionCreateModel question);

    void DeleteQuestion(int id);

    QuestionViewModel GetQuestion(int id);

    PagaResult<QuestionViewModel> GetQuestions(QuestionQueryParameters queryParameters);

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
    public PagaResult<QuestionViewModel> GetQuestions(QuestionQueryParameters queryParameters)
    {
        var query = _dbContext.QuestionBank.AsQueryable();

        // Tìm kiếm theo từ khóa trong nội dung câu hỏi
        if (!string.IsNullOrWhiteSpace(queryParameters.Keyword))
        {
            query = query.Where(q => q.QuestionText.Contains(queryParameters.Keyword));
        }

        // Sắp xếp theo trường chỉ định
        query = queryParameters.SortBy.ToLower() switch
        {
            "questiontext" => queryParameters.SortDesc ? query.OrderByDescending(q => q.QuestionText) : query.OrderBy(q => q.QuestionText),
            _ => queryParameters.SortDesc ? query.OrderByDescending(q => q.Id) : query.OrderBy(q => q.Id)
        };

        var totalItems = query.Count();

        var items = _mapper
            .ProjectTo<QuestionViewModel>(query
                .Skip((queryParameters.PageIndex - 1) * queryParameters.PageSize)
                .Take(queryParameters.PageSize))
            .ToList();

        return new PagaResult<QuestionViewModel>
        {
            TotalItems = totalItems,
            PageIndex = queryParameters.PageIndex,
            PageSize = queryParameters.PageSize,
            TotalPages = (int)Math.Ceiling(totalItems / (double)queryParameters.PageSize),
            Items = items
        };
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