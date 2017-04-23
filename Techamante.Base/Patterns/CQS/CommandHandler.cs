using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Techamante.Patterns.CQS.Interfaces;
using Techamante.Data.Interfaces;

namespace Techamante.Patterns.CQS
{
    public abstract class CommandHandler
    {

        protected IUnitOfWorkAsync UOW;

        public CommandHandler(IUnitOfWorkAsync uow)
        {
            UOW = uow;
        }

        protected async Task Execute(Action action, ICommand<ICommandResult> command) 
        {

            try
            {
                action();
                await UOW.SaveChangesAsync(command.UserId);
            }
            catch
            {
     
                UOW.Rollback();
            }
        }
    }
}
