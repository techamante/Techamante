using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Techamante.Domain.Validations
{
    public class FluentValidationService : IValidationService
    {
        private readonly IValidatorFactory validatorFactory;

        public FluentValidationService(IValidatorFactory validatorFactory)
        {
            this.validatorFactory = validatorFactory;
        }

        public ValidationResult Validate<T>(T entity) where T : class
        {
            var validator = this.validatorFactory.GetValidator(entity.GetType());
            var result = validator.Validate(entity);
            return result;
        }
    }
}
