using System.Collections.Generic;
using Techamante.Domain;

namespace Techamante.Patterns.CQS.Interfaces
{
    public interface ICommandResult
    {
        bool IsSucceeded { get; }

        IList<Error> Errors { get; }
    }
}