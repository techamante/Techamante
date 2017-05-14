using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Techamante.Patterns.CQS.Interfaces;

namespace Techamante.Patterns.CQS
{
    public class MediatorCommandDispatcher : ICommandDispatcher
    {

        private IMediator _mediator;
        public MediatorCommandDispatcher(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task<TResponse> DispatchCommandAsync<TResponse>(ICommand<TResponse> command) where TResponse : ICommandResult
        {
            return await _mediator.Send<TResponse>(command);
        }

        public async Task DispatchCommandAsync<T>(T command) where T : class, ICommand
        {
            await _mediator.Send(command);
        }
    }
}
