﻿using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace HiveWear.Application.Pipelines
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            ValidationContext<TRequest> context = new(request);
            List<ValidationFailure> failures = [.. _validators
                .Select(v => v.Validate(context))
                .SelectMany(result => result.Errors)
                .Where(error => error != null)];

            if (failures.Count != 0)
            {
                throw new ValidationException(failures);
            }

            return await next();
        }
    }
}
