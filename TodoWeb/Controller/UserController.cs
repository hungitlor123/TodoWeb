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
        var isExist = _userService.Login(userLoginModel);
        if (isExist)
        {
            return Ok("Login success");
        }
        return BadRequest("Login failed");
    }
    // public IActionResult Logout()
    // {
    //     return Ok();
    // }
}