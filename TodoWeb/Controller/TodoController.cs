using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoWeb.Application.DTOs;
using TodoWeb.Application.Services;
using TodoWeb.Domains.Entities;
using TodoWeb.Infrastructures;

namespace TodoWeb.Controller;

[ApiController]
[Route("[controller]")]
public class ToDoController : ControllerBase
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IToDoService _toDoService;
    private readonly IGuidGenerator _guidGenerator;
    public ToDoController(IApplicationDbContext dbContext, IToDoService toDoService , IGuidGenerator guidGenerator)
    {
        _dbContext = dbContext;
        _toDoService = toDoService;
        _guidGenerator = guidGenerator;
    }

    [HttpGet("guid")]
    public Guid GetGuild()
    {
        return _guidGenerator.Generate();
    }

    [HttpGet]

    public IEnumerable<ToDoViewModel> Get(bool IsCompeleted)
    {
        var data = _dbContext.Todos.Where( x => x.IsCompeleted == IsCompeleted)
            .Select(x =>
                new ToDoViewModel
                {
                    Description = x.Description,
                    IsCompeleted = x.IsCompeleted
                })
            .ToList();
        return data;
    }

    [HttpPost]
    public int Post (ToDoCreatedModel toDo)
    {
        return _toDoService.Post(toDo);
    }

    [HttpPut]
    public int Put (ToDo toDo)
    {
        var data = _dbContext.Todos.Find(toDo.Id);
        if( data == null)
        {
            return -1;
        }
        data.Description = toDo.Description;
        data.IsCompeleted = toDo.IsCompeleted;
        _dbContext.SaveChanges();
        return toDo.Id;
    }
        
    [HttpDelete]
    public int Delete (int id)
    {
        var data = _dbContext.Todos.Find(id);
        if (data == null)
        {
            return -1;
        }
        _dbContext.Todos.Remove(data);
        _dbContext.SaveChanges();
        return 0;
    }
}
