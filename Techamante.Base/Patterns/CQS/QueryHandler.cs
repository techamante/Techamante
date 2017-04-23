using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Techamante.Data;
using Techamante.Data;
using Techamante.Data.Interfaces;

namespace Techamante.Patterns.CQS
{
    public abstract class QueryHandler<TEntity> where TEntity : BaseEntity
    {
        protected IRepositoryAsync<TEntity> Repo { get; set; }

        public QueryHandler(IRepositoryAsync<TEntity> repo)
        {
            Repo = repo;
        }
    }
}
