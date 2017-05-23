using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Techamante.Domain;

namespace Techamante.Web.GraphQL
{
    public class MutationResponse<TEntity> : ObjectGraphType<TEntity> where TEntity : IGraphType
    {
        public MutationResponse(TEntity entity, string error)
        {

            Field<TEntity>("result", null, null, resolve: ctx => entity);
            Field<StringGraphType>("error", null, null, resolve: ctx => error);
        }

    }
}
