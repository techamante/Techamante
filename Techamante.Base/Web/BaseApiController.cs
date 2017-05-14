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

namespace Techamante.Web
{

    public abstract class BaseApiController : ApiController
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
                if (cmd is ICommand<ICommandResult>)
                {
                    var command = cmd as ICommand<ICommandResult>;
                    var result = await Dispatcher.DispatchCommandAsync(command);
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
                else if (cmd is ICommand)
                {
                    await Dispatcher.DispatchCommandAsync(cmd);
                    return Ok();
                }

                return NotFound();
            }
            catch
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
