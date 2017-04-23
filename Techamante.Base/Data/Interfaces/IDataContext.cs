using System;


namespace Techamante.Data.Interfaces
{
    public interface IDataContext : IDisposable
    {
        int SaveChanges(int? userId = null);
        void SyncObjectState<TEntity>(TEntity entity) where TEntity : class, IObjectState;
        void SyncObjectsStatePostCommit();
    }
}