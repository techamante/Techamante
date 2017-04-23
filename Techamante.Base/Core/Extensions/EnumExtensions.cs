using System;
using System.Linq;
using System.Reflection;

namespace Techamante.Core.Extensions
{
    public static class EnumExtensions
    {

        public static string GetDisplayText(this Enum enumValue)
        {
            var displayAttribute = enumValue.GetType()
                       .GetMember(enumValue.ToString())
                       .First()
                       .GetCustomAttribute<DisplayAttribute>();

            return displayAttribute.Name;

        }
    }
}
