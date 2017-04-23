using System.Collections.Generic;
using System.Linq;

namespace Techamante.Core.Extensions
{
    public static class EnumerableStringExtensions
    {
        public static string Concatenate(this IEnumerable<string> enumerable, string separator)
        {
            return string.Join(separator, enumerable.ToArray());
        }
    }
}
