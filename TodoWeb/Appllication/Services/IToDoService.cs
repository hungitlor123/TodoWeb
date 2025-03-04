using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoWeb.Application.DTOs;
using TodoWeb.Domains.Entities;
using TodoWeb.Infrastructures;

namespace TodoWeb.Application.Services
{
    public interface IToDoService
    {
        int Post(ToDoCreatedModel toDo);
    }

    public class ToDoService : IToDoService
    {
        private readonly IApplicationDbContext _dbContext;

        public ToDoService(IApplicationDbContext _dbContext)
        {
            _dbContext = _dbContext;
        }

        [HttpPost]
        public int Post(ToDoCreatedModel toDo)
        {
            var data = new ToDo()
            {
                Description = toDo.Description
            };

            _dbContext.Todos.Add(data);

            _dbContext.SaveChanges();

            return data.Id;
        }
    }
}
