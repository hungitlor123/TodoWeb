using TodoWeb.Application.DTOs.UserModel;
using TodoWeb.Domains.Entities;

namespace TodoWeb.Application.Services.Users;

public interface IUserService 
{
    int Register(UserCreateModel userCreateModel);

    User? Login(UserLoginModel userLoginModel);
}