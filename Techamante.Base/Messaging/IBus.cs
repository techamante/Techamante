using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Techamante.Messaging
{
    public interface IBus
    {

        Task SendAsync<TMessage>(TMessage message) where TMessage : IMessage;

        Task SendAsync<TMessage>(Queue queue, TMessage message) where TMessage : IMessage;

        Task PublishAsync<TEvent>(TEvent e) where TEvent : IEvent;
    }
}
