using TodoWeb.Application.DTOs.UserModel;

namespace TodoWeb.Application.Services.Users;

public interface IUserService 
{
    int Register(UserCreateModel userCreateModel);
    
    bool Login (UserLoginModel userLoginModel);
}