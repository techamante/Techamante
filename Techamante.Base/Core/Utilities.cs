using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Techamante.Core
{
    public class Utilities
    {

        public static Type GetType(string assemblyName, string typeName)
        {
            var assembly = (from a in AppDomain.CurrentDomain.GetAssemblies()
                            where a.GetName().Name.ToLower().Contains(assemblyName.ToLower())
                            select a).FirstOrDefault();

            var myType = (from t in assembly.GetTypes()
                          where t.Name.ToLower() == typeName.ToLower()
                          select t).FirstOrDefault();

            return myType;

        }

        public static object CastPropertyValue(PropertyInfo property, string value)
        {
            if (property == null || String.IsNullOrEmpty(value))
                return null;
            if (property.PropertyType.IsEnum)
            {
                Type enumType = property.PropertyType;
                if (Enum.IsDefined(enumType, value))
                    return Enum.Parse(enumType, value);
            }
            if (property.PropertyType == typeof(bool))
                return value == "1" || value == "true" || value == "on" || value == "checked";
            else if (property.PropertyType == typeof(Uri))
                return new Uri(Convert.ToString(value));
            else
                return Convert.ChangeType(value, property.PropertyType);
        }
    }
}
