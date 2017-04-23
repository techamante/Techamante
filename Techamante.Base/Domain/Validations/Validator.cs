using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Techamante.Core;

namespace Techamante.Domain.Validations
{
    public static class Validator
    {
        private static Dictionary<Type, object> _validators = new Dictionary<Type, object>();

        public static void RegisterValidatorFor<T>(T entity, IValidator<T> validator)
            where T : IValidatable<T>
        {
            _validators.Add(entity.GetType(), validator);
        }

        public static IValidator<T> GetValidatorFor<T>(T entity)
            where T : IValidatable<T>
        {
            return _validators[entity.GetType()] as IValidator<T>;
        }

        public static bool Validate<T>(this T entity, out IEnumerable<string> bRules)
            where T : IValidatable<T>
        {

            var allBrokenRules = new List<string>();
            var validators = BaseObjectFactory.Instance.GetAll<IValidator<T>>();


            foreach(var validator in validators)
            {
                IEnumerable<string> borkenRules = null;
                entity.Validate(validator, out brokenRules);
            }




            return entity.Validate(validator, out brokenRules);
        }
    }
}
