using AutoMapper;
using TodoWeb.Application.DTOs.UserModel;
using TodoWeb.Appllication.Helper;
using TodoWeb.Domains.Entities;
using TodoWeb.Infrastructures;

namespace TodoWeb.Application.Services.Users;

public class UserService : IUserService

{
    private readonly IApplicationDbContext _dbContext;
    private readonly IMapper _mapper;
    public UserService(IApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public int Register(UserCreateModel userCreateModel)
    {

        var user = _mapper.Map<User>(userCreateModel);

        var salting = HashHelper.GenerateRamdomString(100);
         var password = userCreateModel.Password + salting;
        
         
        user.Salting = salting;
        user.Password = HashHelper.HashPassword(password);
        
        _dbContext.Users.Add(user);
        _dbContext.SaveChanges();
        
        return user.Id;
    }

    public User? Login(UserLoginModel userLoginModel)
    {
        var user = _dbContext.Users
            .FirstOrDefault(x => x.EmailAddress == userLoginModel.EmailAddress);

        if (user == null)
        {
            return null;
        }
        var password = userLoginModel.Password + user.Salting;

        if (!HashHelper.VerifyPassword(password, user.Password))
        {
            return null;
        }

        return user;

    }
}