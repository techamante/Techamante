using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using Techamante.Patterns.CQS.Interfaces;

namespace Techamante.Web
{
    public class CommandActionValueBinder : DefaultActionValueBinder
    {
        protected override HttpParameterBinding GetParameterBinding(HttpParameterDescriptor parameter)
        {
            return typeof(ICommand).IsAssignableFrom(parameter.ParameterType)
                ? parameter.BindWithModelBinding(new CommandModelBinder())
                : base.GetParameterBinding(parameter);
        }
    }
}
