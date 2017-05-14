using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Techamante.Patterns.CQS.Interfaces
{
    public interface IAsyncCommandHandler<in TCommand> : IAsyncRequestHandler<TCommand>
            where TCommand : ICommand
    {
    }

    public interface IAsyncCommandHandler<in TCommand, TCommandResult> : IAsyncRequestHandler<TCommand, TCommandResult>
       where TCommand : ICommand<TCommandResult>
       where TCommandResult : ICommandResult
    {

    }

}
