using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Techamante.Domain.Interfaces;
using Techamante.Patterns.CQS.Interfaces;

namespace Techamante.Patterns.CQS
{
    public static class CommandExtensions
    {

        public static void FromServiceResult(this ICommandResult commandResult, IServiceResult result)
        {
            commandResult.Errors.AddRange(result.Errors);
        }
    }
}
