using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Techamante.Web
{
    public class GraphQLContext
    {

        public GraphQLContext(int userId)
        {
            UserId = userId;
            TimeStamp = DateTimeOffset.UtcNow;
        }

        public int UserId { get; }

        public DateTimeOffset TimeStamp { get; }

    }
}
