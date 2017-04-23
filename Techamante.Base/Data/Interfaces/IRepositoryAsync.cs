using System.Threading;
using System.Threading.Tasks;


namespace Techamante.Data.Interfaces
{
    public interface IRepositoryAsync<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        Task<TEntity> FindAsync(params object[] keyValues);
        Task<TDataTransferObject> GetById<TDataTransferObject>(int id);
        Task<TEntity> FindAsync(CancellationToken cancellationToken, params object[] keyValues);
        Task<bool> DeleteAsync(params object[] keyValues);
        Task<bool> DeleteAsync(CancellationToken cancellationToken, params object[] keyValues);
    }
}