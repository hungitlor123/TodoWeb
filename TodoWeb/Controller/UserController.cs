using Microsoft.AspNetCore.Mvc;
using TodoWeb.Application.DTOs.UserModel;
using TodoWeb.Application.Services.Users;

namespace TodoWeb.Controller;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }
    // GET
    [HttpPost("Register")]
    public IActionResult Register(UserCreateModel userCreateModel)
    {
        var userId = _userService.Register(userCreateModel);

        if (userId > 0)
        {
            return Ok(userId);
        }
        return BadRequest("Something is wrong");
    }
    
    [HttpPost("Login")]
    public IActionResult Login(UserLoginModel userLoginModel)
    {
        var user =  _userService.Login(userLoginModel);
        
        if (user == null)
        {
            return BadRequest("Email or password is wrong");
        }
        
        HttpContext.Session.SetInt32("UserId", user.Id);
        HttpContext.Session.SetString("Role", user.Role.ToString());

        return Ok("Login success");
    }
    [HttpPost("Logout")]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return Ok();
    }

    public IActionResult TranserMoney(int userId, decimal money)
    {
        return Ok();
    }
}