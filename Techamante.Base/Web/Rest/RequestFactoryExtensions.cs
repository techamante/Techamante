using System;
using RestSharp;

namespace Techamante.Web.Rest
{
    public static class RequestFactoryExtensions
    {
        public static IRestClient CreateClient(this IRequestFactory factory, Uri baseUri)
        {
            var client = factory.CreateClient();
            client.BaseUrl = baseUri;
            return client;
        }

        public static IRestRequest CreateRequest(this IRequestFactory factory, string resource)
        {
            return CreateRequest(factory, resource, Method.GET);
        }

        public static IRestRequest CreateRequest(this IRequestFactory factory, string resource, Method method)
        {
            var request = factory.CreateRequest();
            request.Resource = resource;
            request.Method = method;
            return request;
        }
    }
}