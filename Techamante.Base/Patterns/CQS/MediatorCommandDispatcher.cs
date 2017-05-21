using MediatR;
using SimpleInjector.Extensions.ExecutionContextScoping;
using SimpleInjector.Lifestyles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Techamante.Core;
using Techamante.Core.Interfaces;
using Techamante.Data.Interfaces;
using Techamante.Domain;
using Techamante.Patterns.CQS.Interfaces;

namespace Techamante.Patterns.CQS
{
    public class MediatorCommandDispatcher : ICommandDispatcher
    {

        private readonly IMediator _mediator;

        public MediatorCommandDispatcher(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task<TCommandResult> DispatchCommandAsync<TCommandResult>(ICommand<TCommandResult> command) where TCommandResult : ICommandResult
        {
            return await _mediator.Send<TCommandResult>(command);
        }

        public async Task DispatchCommandAsync<T>(T command) where T : class, ICommand
        {
            await _mediator.Send(command);
        }
    }
}
