using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Techamante.Patterns.CQS.Interfaces
{
    public interface ICommandHandler<in TCommand>
            where TCommand : ICommand
    {

        Task ExecuteAsync(TCommand command);
    }
}
