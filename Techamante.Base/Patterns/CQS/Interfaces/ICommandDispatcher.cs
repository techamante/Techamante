﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Techamante.Patterns.CQS.Interfaces
{
    public interface ICommandDispatcher
    {
        Task DispatchCommandAsync<T>(T command) where T : class, ICommand;
        Task<TCommandResult> DispatchCommandAsync<TCommandResult>(ICommand<TCommandResult> command)
            where TCommandResult : ICommandResult;


    }
}
