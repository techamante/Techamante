using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Techamante.Domain.Interfaces;

namespace Techamante.Domain
{
    public class DomainEvents
    {
        [ThreadStatic]
        private static List<IDomainEvent> _domainevents;
        private static List<IDomainEvent> Events
        {
            get
            {
                if (_domainevents == null)
                {
                    _domainevents = new List<IDomainEvent>();
                }
                return _domainevents;
            }
        }


        public static void Register<T>(T eventArgs) where T : IDomainEvent
        {
            Events.Add(eventArgs);
        }

        public async static Task PublishAsync()
        {
            var mediator = Core.BaseObjectFactory.Instance.Get<IMediator>();
            foreach (var @event in Events)
            {
                await mediator.Publish(@event);
            }
            Flush();
        }


        public static void Flush()
        {
            Events.Clear();
        }

    }
}