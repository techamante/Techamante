using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using Techamante.Core;
using Techamante.Patterns.CQS.Interfaces;

namespace Techamante.Web
{
    public class CommandModelBinder : IModelBinder
    {
        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {

            if (!typeof(ICommand).IsAssignableFrom(bindingContext.ModelType)) return false;
            var type = bindingContext.ModelType;

            if (!type.IsClass)
            {
                var method = bindingContext.ValueProvider.GetValue("command");
                if (method == null) throw new AppException("Command is required");
                var command = $"{method.RawValue}Command";

                type = Utilities.GetType("Domain.applicationServices", command);
                if (type == null) throw new AppException("Suitable Command not found");

                var attribute = actionContext.ActionDescriptor.GetCustomAttributes<CommandAttribute>().FirstOrDefault();
                if (attribute == null) throw new AppException("Invalid command");
                if (!attribute.Commands.Any(c => c == type))
                {
                    bindingContext.ModelState.AddModelError("ValidationError", "Invalid command");
                    return false;
                }
            }

            var json = ExtractRequestJson(actionContext);
            var obj = JsonConvert.DeserializeObject(json, type);

            var vals = actionContext.RequestContext.RouteData.Values;
            var properties = obj.GetType().GetProperties();

            foreach (var val in vals)
            {
                var property = properties.FirstOrDefault(p => p.Name.ToLower() == val.Key.ToLower());
                property.SetValue(obj, Utilities.CastPropertyValue(property, val.Value.ToString()));
            }

            bindingContext.Model = obj;
            return true;
        }

        private static string ExtractRequestJson(HttpActionContext actionContext)
        {
            var content = actionContext.Request.Content;
            string json = content.ReadAsStringAsync().Result;
            return json;
        }


    }
}
