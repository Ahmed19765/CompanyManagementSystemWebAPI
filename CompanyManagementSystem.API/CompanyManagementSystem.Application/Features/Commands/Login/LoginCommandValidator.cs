using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;

namespace CompanyManagementSystem.Application.Features.Commands.Login
{
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator() 
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.")
                .MinimumLength(8).WithMessage("Invalid Email Size")
                .MaximumLength(60).WithMessage("Invalid Email Size")
                .Matches(@"^[^@]+@gmail\.com$").WithMessage("Only Gmail addresses are allowed!");
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(8).WithMessage("Invalid Password Size")
                .MaximumLength(60).WithMessage("Invalid Password Size");
        }
    }
}
