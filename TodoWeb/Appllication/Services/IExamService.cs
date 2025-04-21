using TodoWeb.Infrastructures;

namespace TodoWeb.Application.Services;

public interface IExamService
{
    
}

public class ExamService : IExamService
{
    private readonly IApplicationDbContext _dbcontext;
}