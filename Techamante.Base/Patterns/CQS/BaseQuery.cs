
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Techamante.Data;
using Techamante.Patterns.CQS.Interfaces;

namespace Techamante.Patterns.CQS
{
    public abstract class BaseQuery<TEntity> : IQuery<TEntity>
    {
        public QueryOptions Options { get; set; }

        public int UserId { get; set; }

        public DateTimeOffset TimeStamp { get; set; }
    }
}
