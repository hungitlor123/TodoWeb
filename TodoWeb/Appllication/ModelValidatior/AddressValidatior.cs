using FluentValidation;
using TodoWeb.Application.DTOs;

namespace TodoWeb.Appllication.StudentValidator;

public class AddressValidatior : AbstractValidator<Address>
{
    public AddressValidatior()
    {
        RuleFor(x => x.Street).NotEmpty().WithMessage("Street is required");
        
        RuleFor(x => x.ZipCode).NotEmpty().WithMessage("ZipCode is required");
        
    }
    
}