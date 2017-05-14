using System;
using System.Threading;
using System.Threading.Tasks;
using Techamante.Domain.Interfaces;
using Techamante.Patterns.CQS.Interfaces;

namespace Techamante.Data.Interfaces
{
    public interface IUnitOfWorkAsync : IUnitOfWork
    {
        Task<int> SaveChangesAsync(int? userId = null);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken, int? userId = null);
        IRepositoryAsync<TEntity> RepositoryAsync<TEntity>() where TEntity : BaseEntity;

    }
}