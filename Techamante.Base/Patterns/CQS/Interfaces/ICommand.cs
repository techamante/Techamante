using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Techamante.Patterns.CQS.Interfaces
{

    public interface ICommand
    {
        int UserId { get; set; }

        DateTimeOffset TimeStamp { get; set; }

    }

    public interface ICommand<TResult> : ICommand where TResult : ICommandResult
    {
        TResult Result { get; }
    }
}
