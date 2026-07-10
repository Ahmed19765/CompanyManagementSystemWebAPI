using FluentValidation;

namespace CompanyManagementSystem.Application.Features.Commands.UpdateProject
{
    public class UpdateProjectCommandValidator : AbstractValidator<UpdateProjectCommand>
    {
        public UpdateProjectCommandValidator()
        {
            RuleFor(x => x.ProjectId)
                .GreaterThan(0).WithMessage("Project id is required.");

            RuleFor(x => x.ProjectTitle)
                .NotEmpty().WithMessage("Project title is required.")
                .MaximumLength(100).WithMessage("Project title is too long.");

            RuleFor(x => x.ProjectDescription)
                .MaximumLength(500).WithMessage("Project description is too long.");

            RuleFor(x => x.ProjectDocumentsUrl)
                .MaximumLength(500).WithMessage("Documents URL is too long.")
                .Must(url => Uri.IsWellFormedUriString(url, UriKind.Absolute))
                .WithMessage("Please enter a valid URL.")
                .When(x => !string.IsNullOrWhiteSpace(x.ProjectDocumentsUrl));

            RuleFor(x => x.ProjectOfferedBudget)
                .GreaterThan(0).WithMessage("Offered budget must be greater than zero.");
        }
    }
}
