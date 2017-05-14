using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Techamante.Domain.Interfaces
{
    public interface IDomainEventDispatcher
    {
        void Dispatch(IEnumerable<IDomainEvent> events);
    }
}
