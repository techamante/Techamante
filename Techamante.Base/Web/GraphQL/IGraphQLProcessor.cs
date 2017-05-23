using GraphQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Techamante.Web
{
    public interface IGraphQLProcessor
    {
        Task<ExecutionResult> ExecuteAsync(GraphQLContext context, GraphQLQuery query);

        string ExtractContent(ExecutionResult result);
    }
}
