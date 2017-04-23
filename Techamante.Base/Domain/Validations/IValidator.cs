using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Techamante.Data;

namespace Techamante.Domain.Validations
{
    public interface IValidator<T>
    {

        ValidationResult Validate(T entity);
    }
}
