using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Techamante.Domain;
using Techamante.Domain.Interfaces;
using Techamante.Patterns.CQS.Interfaces;

namespace Techamante.Patterns.CQS
{
    public class CommandResult : ICommandResult
    {
        public CommandResult()
        {
            Errors = new List<Error>();
        }
        public IList<Error> Errors { get; }

        public bool IsSucceeded => !Errors.Any();
    }
}
