using FluentValidation;
using MediatR;

namespace CompanyManagementSystem.Application.Common.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            if (!_validators.Any())
            {
                return await next(cancellationToken);
            }

            var context = new ValidationContext<TRequest>(request);
            var validationResults = await Task.WhenAll(
                _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

            var errors = validationResults
                .SelectMany(result => result.Errors)
                .Where(error => error is not null)
                .Select(error => error.ErrorMessage)
                .Distinct()
                .ToList();

            if (errors.Count > 0)
            {
                throw new ValidationException(string.Join(" ", errors));
            }

            return await next(cancellationToken);
        }
    }
}
