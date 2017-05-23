using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Web.Http;
using System.Threading.Tasks;
using AutoMapper;
using System;
using Techamante.Patterns.CQS.Interfaces;
using Techamante.Patterns.CQS;
using Techamante.Core;
using SimpleInjector.Extensions.ExecutionContextScoping;
using System.Net.Http;
using System.Web.Http.Results;
using System.Text;
using System.Net;

namespace Techamante.Web
{

    public abstract class BaseApiController : ApiController
    {
        protected IQueryProcessor QueryProcessor { get; }

        protected ICommandDispatcher Dispatcher { get; }

        protected IGraphQLProcessor GraphQLProcessor { get; }

        protected abstract int UserId { get; }

        public BaseApiController(IQueryProcessor queryProcessor = null, ICommandDispatcher dispatcher = null, IGraphQLProcessor graphQLProcessor = null)
        {
            QueryProcessor = queryProcessor;
            Dispatcher = dispatcher;
            GraphQLProcessor = graphQLProcessor;
        }

        protected async Task<IHttpActionResult> Dispatch<TCommand>(TCommand cmd) where TCommand : class, ICommand
        {
            if (Dispatcher == null) throw new NotSupportedException("Command Dispatcher is not supported by default");
            cmd.UserId = UserId;
            cmd.TimeStamp = DateTimeOffset.Now;
            try
            {
                await Dispatcher.DispatchCommandAsync(cmd);
                return Ok();
            }
            catch (Exception ex)
            {
                //ModelState.AddModelError("ValidationError", ex.Message);
                return BadRequest(ModelState);
            }
        }

        protected async Task<IHttpActionResult> Dispatch<TCommand, TCommandResult>(TCommand cmd)
            where TCommand : class, ICommand<TCommandResult>
            where TCommandResult : ICommandResult
        {
            if (Dispatcher == null) throw new NotSupportedException("Command Dispatcher is not supported by default");
            cmd.UserId = UserId;
            cmd.TimeStamp = DateTimeOffset.Now;
            try
            {
                var result = await Dispatcher.DispatchCommandAsync<TCommandResult>(cmd);
                if (result.IsSucceeded)
                {
                    return Ok(result);
                }
                else
                {
                    result.Errors.ToList().ForEach(err => ModelState.AddModelError("Error", err.Message));
                    return BadRequest(ModelState);
                }
            }
            catch (Exception ex)
            {
                //ModelState.AddModelError("ValidationError", ex.Message);
                return BadRequest(ModelState);
            }

        }

        protected async Task<IHttpActionResult> Query<TResult>(BaseQuery<TResult> query, QueryOptions options = null)
        {
            if (QueryProcessor == null) throw new NotSupportedException("Query processing is not supported by default");
            var queryOptions = options == null ? new Patterns.CQS.QueryOptions() : Mapper.Map<Patterns.CQS.QueryOptions>(options);
            query.Options = queryOptions;
            query.UserId = UserId;
            query.TimeStamp = DateTimeOffset.Now;
            try
            {
                var result = await QueryProcessor.ProcessAsync(query);
                return Ok(result);
            }
            catch (AppException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return InternalServerError();
            }
        }

        protected async Task<HttpResponseMessage> QueryGraphQLAsync(GraphQLQuery query)
        {
            if (GraphQLProcessor == null) throw new NotSupportedException("Graph QL processing is not supported by default");
            var request = ControllerContext.Request;
            var context = new GraphQLContext(UserId);
            var result = await GraphQLProcessor.ExecuteAsync(context, query);

            var httpResult = result.Errors?.Count > 0
               ? HttpStatusCode.BadRequest
               : HttpStatusCode.OK;

            var response = request.CreateResponse(httpResult);
            response.Content = new StringContent(GraphQLProcessor.ExtractContent(result), Encoding.UTF8, "application/json");
            return response;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
