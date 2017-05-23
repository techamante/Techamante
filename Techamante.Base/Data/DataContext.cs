using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading;
using System.Threading.Tasks;
using Techamante.Data.Interfaces;

namespace Techamante.Data
{
    public class DataContext : DbContext, IDataContextAsync
    {
        #region Private Fields
        private readonly Guid _instanceId;
        bool _disposed;
        #endregion Private Fields

        public Guid InstanceId { get { return _instanceId; } }


        public DataContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {
            _instanceId = Guid.NewGuid();
            Configuration.LazyLoadingEnabled = true;
            Configuration.ProxyCreationEnabled = true;
        }


        public int SaveChanges(int? userId = null)
        {
            SyncObjectsStatePreCommit();
            ProcessSave(userId);
            var changes = base.SaveChanges();
            SyncObjectsStatePostCommit();
            return changes;
        }

        public async Task<int> SaveChangesAsync(int? userId = null)
        {
            return await this.SaveChangesAsync(CancellationToken.None, userId);
        }


        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken, int? userId = null)
        {

            SyncObjectsStatePreCommit();
            ProcessSave(userId);
            var changesAsync = await base.SaveChangesAsync(cancellationToken);
            SyncObjectsStatePostCommit();
            return changesAsync;
        }

        public void SyncObjectState<TEntity>(TEntity entity) where TEntity : class, IObjectState
        {
            Entry(entity).State = StateHelper.ConvertState(entity.ObjectState);
        }

        private void SyncObjectsStatePreCommit()
        {
            foreach (var dbEntityEntry in ChangeTracker.Entries())
            {
                dbEntityEntry.State = StateHelper.ConvertState(((IObjectState)dbEntityEntry.Entity).ObjectState);
            }
        }

        public void SyncObjectsStatePostCommit()
        {
            foreach (var dbEntityEntry in ChangeTracker.Entries())
            {
                ((IObjectState)dbEntityEntry.Entity).ObjectState = StateHelper.ConvertState(dbEntityEntry.State);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // free other managed objects that implement
                    // IDisposable only
                }

                // release any unmanaged objects
                // set object references to null

                _disposed = true;
            }

            base.Dispose(disposing);
        }

        private void ProcessSave(int? userId = null)
        {
            foreach (var entry in ChangeTracker.Entries<IEntityBase>())
            {
                if (entry.State == System.Data.Entity.EntityState.Added)
                {
                    if (entry.Entity.CreatedDt == DateTime.MinValue)
                    {
                        entry.Entity.CreatedDt = DateTime.UtcNow;
                    }
                }

            }
        }
    }
}