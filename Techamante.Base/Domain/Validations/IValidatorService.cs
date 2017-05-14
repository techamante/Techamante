using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Techamante.Domain.Validations
{
    public interface IValidationService
    {
        ValidationResult Validate<T>(T entity) where T : class;
    }
}
