
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Techamante.Data;

namespace Techamante.Patterns.CQS
{
    public abstract class PagedQuery<TEntity> : BaseQuery<IEnumerable<TEntity>>
    {
        const int DEFAULT_PAGE_SIZE = 100;
        const int DEFAULT_PAGE_INDEX = 0;

        public int PageIndex => Options != null ? Options.PageIndex.Value : DEFAULT_PAGE_INDEX;

        public int PageSize => Options != null ? Options.PageSize.Value : DEFAULT_PAGE_SIZE;
    }
}
