#region

using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.Data.Common;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Threading;
using System.Threading.Tasks;
using Techamante.Core.Interfaces;
using Techamante.Data.Interfaces;



#endregion

namespace Techamante.Data
{
    public class UnitOfWork : IUnitOfWorkAsync
    {
        #region Private Fields

        private IDataContextAsync _dataContext;
        private bool _disposed;
        private ObjectContext _objectContext;
        private DbTransaction _transaction;
        private readonly IObjectFactory _objectFactory;

        #endregion Private Fields

        #region Constuctor/Dispose

        public UnitOfWork(IDataContextAsync dataContext, IObjectFactory objectFactory)
        {
            _dataContext = dataContext;
            _objectFactory = objectFactory;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                // free other managed objects that implement
                // IDisposable only

                try
                {
                    if (_objectContext != null && _objectContext.Connection.State == ConnectionState.Open)
                    {
                        _objectContext.Connection.Close();
                    }
                }
                catch (ObjectDisposedException)
                {
                    // do nothing, the objectContext has already been disposed
                }

                if (_dataContext != null)
                {
                    _dataContext.Dispose();
                    _dataContext = null;
                }
            }

            // release any unmanaged objects
            // set the object references to null

            _disposed = true;
        }

        #endregion Constuctor/Dispose

        public int SaveChanges(int? userId = null)
        {
            return _dataContext.SaveChanges(userId);
        }

        public IRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            //if (ServiceLocator.IsLocationProviderSet)
            //{
            //    return ServiceLocator.Current.GetInstance<IRepository<TEntity>>();
            //}

            return RepositoryAsync<TEntity>();
        }

        public Task<int> SaveChangesAsync(int? userId = null)
        {
            return _dataContext.SaveChangesAsync(userId);
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken, int? userId = null)
        {
            return _dataContext.SaveChangesAsync(cancellationToken, userId);
        }

        public IRepositoryAsync<TEntity> RepositoryAsync<TEntity>() where TEntity : BaseEntity
        {
            return _objectFactory.Get<IRepositoryAsync<TEntity>>();
        }

        #region Unit of Work Transactions

        public void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.Unspecified)
        {
            _objectContext = ((IObjectContextAdapter)_dataContext).ObjectContext;
            if (_objectContext.Connection.State != ConnectionState.Open)
            {
                _objectContext.Connection.Open();
            }

            _transaction = _objectContext.Connection.BeginTransaction(isolationLevel);
        }

        public bool Commit()
        {
            _transaction.Commit();
            return true;
        }

        public void Rollback()
        {
            _transaction.Rollback();
            _dataContext.SyncObjectsStatePostCommit();
        }

        #endregion
    }
}