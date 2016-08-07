using System.Collections.Specialized;

namespace Techamante.Base
{
    public static class NameValueCollectionExtensions
    {
        public static string GetOrThrowUnexpectedResponse(this NameValueCollection collection, string key)
        {
            var value = collection[key];
            if (value.IsEmpty())
            {
                throw new UnexpectedResponseException(key);
            }
            return value;
        }
    }
}