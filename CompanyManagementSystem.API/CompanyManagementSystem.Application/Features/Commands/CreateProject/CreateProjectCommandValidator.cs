using FluentValidation;

namespace CompanyManagementSystem.Application.Features.Commands.CreateProject
{
    public class CreateProjectCommandValidator : AbstractValidator<CreateProjectCommand>
    {
        public CreateProjectCommandValidator()
        {
            RuleFor(x => x.ProjectTitle)
                .NotEmpty().WithMessage("Project title is required.")
                .MaximumLength(100).WithMessage("Project title is too long.");

            RuleFor(x => x.ProjectDescription)
                .NotEmpty().WithMessage("Project description is required.")
                .MaximumLength(500).WithMessage("Project description is too long.");

            RuleFor(x => x.ProjectDocumentsUrl)
                .MaximumLength(500).WithMessage("Project documents URL is too long.")
                .Must(url => Uri.IsWellFormedUriString(url, UriKind.Absolute))
                .WithMessage("Please enter a valid URL.")
                .When(x => !string.IsNullOrWhiteSpace(x.ProjectDocumentsUrl));

            RuleFor(x => x.ProjectOfferedBudget)
                .GreaterThan(0).WithMessage("Project offered budget must be greater than zero.");
        }
    }
}
