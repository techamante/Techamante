using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Techamante.Patterns.CQS.Interfaces;

namespace Techamante.Patterns.CQS
{
    public abstract class BaseCommand<TResult> : ICommand<TResult> where TResult : ICommandResult, new()
    {

        //public virtual TResult Result { get; }

        public DateTimeOffset TimeStamp { get; set; }

        public int UserId { get; set; }

    }
}
