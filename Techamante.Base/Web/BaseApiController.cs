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

namespace Techamante.Web
{

    public abstract class BaseApiController : ApiController, IController
    {
        protected IQueryProcessor QueryProcessor { get; }

        protected ICommandDispatcher Dispatcher { get; }

        protected abstract int UserId { get; }


        public BaseApiController(IQueryProcessor queryProcessor, ICommandDispatcher dispatcher)
        {
            QueryProcessor = queryProcessor;
            Dispatcher = dispatcher;
        }

        protected async Task<IHttpActionResult> Dispatch<TCommand>(TCommand cmd) where TCommand : class, ICommand
        {
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

        public async Task<IHttpActionResult> Query<TResult>(BaseQuery<TResult> query, QueryOptions options)
        {
            query.Options = Mapper.Map<Patterns.CQS.QueryOptions>(options);
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

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
