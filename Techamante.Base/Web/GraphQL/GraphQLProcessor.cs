using GraphQL;
using GraphQL.Http;
using GraphQL.Instrumentation;
using GraphQL.Types;
using GraphQL.Validation.Complexity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Techamante.Core.Interfaces;

namespace Techamante.Web
{
    public class GraphQueryProcessor : IGraphQLProcessor
    {
        private readonly ISchema _schema;
        private readonly IDocumentExecuter _executer;
        private readonly IDocumentWriter _writer;
        private readonly IDictionary<string, string> _namedQueries;
        private readonly IObjectFactory _objectFactory;

        public GraphQueryProcessor(
            IDocumentExecuter executer,
            IDocumentWriter writer,
            ISchema schema,
            IObjectFactory objectFactory)
        {
            _executer = executer;
            _writer = writer;
            _schema = schema;
            _objectFactory = objectFactory;
            _namedQueries = new Dictionary<string, string>
            {
                ["a-query"] = @"query foo { hero { name } }"
            };
        }


        public async Task<ExecutionResult> ExecuteAsync(GraphQLContext context, GraphQLQuery query)
        {
            var inputs = query.Variables.ToInputs();
            var queryToExecute = query.Query;

            if (!string.IsNullOrWhiteSpace(query.NamedQuery))
            {
                queryToExecute = _namedQueries[query.NamedQuery];
            }


            using (_objectFactory.BeginScope())
            {
                var result = await _executer.ExecuteAsync(_ =>
                 {
                     _.Schema = _schema;
                     _.Query = queryToExecute;
                     _.OperationName = query.OperationName;
                     _.Inputs = inputs;
                     _.UserContext = context;

                     _.ComplexityConfiguration = new ComplexityConfiguration { MaxDepth = 15 };
                     _.FieldMiddleware.Use<InstrumentFieldsMiddleware>();

                 }).ConfigureAwait(false);

                return result;
            }
        }

        public string ExtractContent(ExecutionResult result)
        {
            var json = _writer.Write(result);
            return json;
        }
    }
}
