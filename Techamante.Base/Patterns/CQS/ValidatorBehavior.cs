using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Techamante.Patterns.CQS
{
    public class ValidatorBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {

        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidatorBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next)
        {
            var context = new ValidationContext(request);

            var validatorsTasks = _validators
                .Select(async v => await v.ValidateAsync(context));

            var failures = Task.WhenAll(validatorsTasks).Result.SelectMany(x => x.Errors);

            if (failures.Any())
                throw new ValidationException(failures);

            return next();
        }
    }
}
