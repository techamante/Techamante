using System.Threading;
using System.Threading.Tasks;

namespace Techamante.Data.Interfaces
{
    public interface IDataContextAsync : IDataContext
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken, int? userId = null);
        Task<int> SaveChangesAsync(int? userId = null);
    }
}