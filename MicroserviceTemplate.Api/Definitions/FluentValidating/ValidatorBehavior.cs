using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Calabonga.OperationResults;
using FluentValidation;
using MediatR;

namespace MicroserviceTemplate.Api.Definitions.FluentValidating;

public class ValidatorBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidatorBehavior(IEnumerable<IValidator<TRequest>> validators) => _validators = validators;

    public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var failures = _validators
            .Select(x => x.Validate(new ValidationContext<TRequest>(request)))
            .SelectMany(x => x.Errors)
            .Where(x => x != null)
            .ToList();

        if (!failures.Any())
            return next();

        var type = typeof(TResponse);
        if (!type.IsSubclassOf(typeof(OperationResult)))
            throw new ValidationException(failures);

        var result = Activator.CreateInstance(type);
        ((OperationResult) result!).AddError(new ValidationException(failures));
        return Task.FromResult((TResponse) result!);
    }
}