using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Techamante.Core.Interfaces;
using Techamante.Logging;
using Techamante.Patterns.CQS.Interfaces;

namespace Techamante.Patterns.CQS
{
    public sealed class QueryProcessor : IQueryProcessor
    {
        private readonly IObjectFactory _objectFactory;

        public QueryProcessor(IObjectFactory objectFactory)
        {
            _objectFactory = objectFactory;
        }

        [DebuggerStepThrough]
        public TResult Process<TResult>(IQuery<TResult> query)
        {
            var handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResult));
            dynamic handler = _objectFactory.Get(handlerType);
            var queryResult = handler.Execute((dynamic)query);

            return queryResult;
        }

        public async Task<TResult> ProcessAsync<TResult>(IQuery<TResult> query)
        {
            var handlerType = typeof(IQueryHandlerAsync<,>).MakeGenericType(query.GetType(), typeof(TResult));
            dynamic handler = _objectFactory.Get(handlerType);
            var queryResult = await handler.ExecuteAsync((dynamic)query).ConfigureAwait(false);
            return queryResult;
        }
    }
}
