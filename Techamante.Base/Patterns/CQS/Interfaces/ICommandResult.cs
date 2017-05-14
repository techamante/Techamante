using System.Collections.Generic;
using Techamante.Domain;

namespace Techamante.Patterns.CQS.Interfaces
{
    public interface ICommandResult
    {
        bool IsSucceeded { get; }

        List<Error> Errors { get; }
    }
}