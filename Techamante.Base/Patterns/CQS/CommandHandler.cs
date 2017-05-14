using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Techamante.Patterns.CQS.Interfaces;
using Techamante.Data.Interfaces;
using Techamante.Domain.Interfaces;

namespace Techamante.Patterns.CQS
{
    public abstract class CommandHandler
    {

        protected IUnitOfWorkAsync UOW;

        public CommandHandler(IUnitOfWorkAsync uow)
        {
            UOW = uow;
        }

        [Logging.Log]
        protected async Task<TCommandResult> Execute<TCommandResult>(ICommand<TCommandResult> command, Func<Task<IServiceResult>> action)
            where TCommandResult : ICommandResult, new()
        {
            var commandResult = new TCommandResult();
            try
            {
                UOW.BeginTransaction();
                var result = await action();
                if (result.IsSucceeded)
                {
                    await UOW.SaveChangesAsync(command.UserId);

                }
                else
                {
                    UOW.Rollback();
                }
                return commandResult;
            }
            catch (Exception ex)
            {
                UOW.Rollback();
                throw ex;
            }
        }
    }
}
