using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Techamante.Domain.Interfaces
{
    public interface IServiceResult
    {
        bool IsSucceeded { get; }

        IEnumerable<Error> Errors { get; }
    }

    public interface IServiceResult<TEntity> : IServiceResult
    {
        TEntity Entity { get; }
    }
}
