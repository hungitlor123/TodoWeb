using TodoWeb.Constants.Enums;

namespace TodoWeb.Application.DTOs.UserModel;

public class UserCreateModel
{
    public string EmailAddress { get; set; }
    
    public string Password { get; set; }
    
    public string FullName { get; set; }
    
    public Role Role { get; set; }
}