using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Techamante.Patterns.CQS.Interfaces
{

    public interface ICommand : IRequest
    {
        int UserId { get; set; }

        DateTimeOffset TimeStamp { get; set; }

    }

    public interface ICommand<out TResult> : IRequest<TResult>, ICommand
        where TResult : ICommandResult
    {
        //TResult Result { get; }
    }
}
