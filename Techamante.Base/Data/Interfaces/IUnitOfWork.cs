using System;
using System.Data;


namespace Techamante.Data.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        int SaveChanges(int? userId = null);
        void Dispose(bool disposing);
        IRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity;
        void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.Unspecified);
        bool Commit();
        void Rollback();
    }
}