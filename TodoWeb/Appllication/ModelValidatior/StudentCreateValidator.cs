using FluentValidation;
using TodoWeb.Application.DTOs;
using TodoWeb.Infrastructures;

namespace TodoWeb.Appllication.StudentValidator;

public class StudentCreateValidator : AbstractValidator<StudentCreateViewModel>
{
    private readonly IApplicationDbContext _dbContext;
    public StudentCreateValidator( IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
        
        RuleFor(x => x.StudentId).NotEmpty();
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.DateOfBirth).LessThan(DateTime.Today);
        RuleFor(x => x.SchoolId).GreaterThan(0);
        RuleForEach(x => x.Emails).EmailAddress().WithMessage("Email is not valid");
        RuleFor(x => x.Address)
            .SetValidator(new AddressValidatior())
            .WithMessage("Address is required");

    }
    
    private bool DoesNotExists(string fullName)
    {
        return !_dbContext.Student.Any(x => x.FirstName == fullName);
    }
    
    
}