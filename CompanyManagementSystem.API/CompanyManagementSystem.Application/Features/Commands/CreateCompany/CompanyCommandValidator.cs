using FluentValidation;
using FluentValidation.Validators;
using System;
using System.Collections.Generic;
using System.Text;

namespace CompanyManagementSystem.Application.Features.Commands.CreateCompany
{
    public class CompanyCommandValidator : AbstractValidator<AddCompanyCommand>
    {
        public CompanyCommandValidator() 
        {
            RuleFor(x => x.CompanyName)
                .NotEmpty().WithMessage("Company name is required!")
                .MaximumLength(40).WithMessage("Is this a really company name?!");


            RuleFor(x => x.CompanyDescription)
                .NotEmpty().WithMessage("Add description what your company services provide!")
                .MaximumLength(500).WithMessage("You exceed the maximum length");
        }
    }
}
