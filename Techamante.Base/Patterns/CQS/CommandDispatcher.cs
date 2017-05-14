using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Techamante.Core.Interfaces;
using Techamante.Logging;
using Techamante.Patterns.CQS.Interfaces;

namespace Techamante.Patterns.CQS
{
    public class CommandDispatcher : ICommandDispatcher
    {
        private readonly IObjectFactory _objectFactory;

        public CommandDispatcher(IObjectFactory objectFactory)
        {
            _objectFactory = objectFactory;
        }

        public async Task DispatchCommandAsync<TCommand>(TCommand command) where TCommand : class, ICommand
        {
            var handlers = _objectFactory.GetAll<IAsyncCommandHandler<TCommand>>();
            var commandsTasks = new List<Task>();
            handlers.ToList().ForEach(handler => commandsTasks.Add(handler.Handle(command)));
            await Task.WhenAll(commandsTasks);
        }

        public async Task<TResponse> DispatchCommandAsync<TResponse>(ICommand<TResponse> command)
            where TResponse : ICommandResult
        {
            var  handler = _objectFactory.Get<IAsyncCommandHandler<ICommand<TResponse>,TResponse>>();
            if (handler == null) throw new NotSupportedException("Command not supported");          
            return await handler.Handle(command);
            
        }
    }
}
