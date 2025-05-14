using FluentValidation;
using TodoWeb.Application.DTOs.UserModel;
using TodoWeb.Domains.Entities;

namespace TodoWeb.Appllication.StudentValidator;

public class UserValidator : AbstractValidator<UserCreateModel>
{
    public UserValidator()
    {
        RuleFor(x => x.EmailAddress).EmailAddress().WithMessage("Email is not valid");
        RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required");
        RuleFor(x => x.FullName).NotEmpty().WithMessage("Fullname is required");
        RuleFor(x => x.Role).IsInEnum().WithMessage("Role is required");
    }
    
}