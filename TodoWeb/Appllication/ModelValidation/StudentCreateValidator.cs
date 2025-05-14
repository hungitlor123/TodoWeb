using FluentValidation;
using TodoWeb.Application.DTOs;

namespace TodoWeb.Appllication.StudentValidator;

public class StudentCreateValidator : AbstractValidator<StudentCreateViewModel>
{
    public StudentCreateValidator()
    {
        
        RuleFor(x => x.StudentId).NotEmpty();
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.DateOfBirth).LessThan(DateTime.Today);
        RuleFor(x => x.SchoolId).GreaterThan(0);
        
    }
    
    
}