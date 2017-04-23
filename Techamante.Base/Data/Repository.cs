using Techamante.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Techamante.Domain;
using Techamante.Data.Interfaces;
using AutoMapper.QueryableExtensions;

namespace Techamante.Data
{
    public class Repository<TEntity> : IRepositoryAsync<TEntity> where TEntity : BaseEntity
    {
        #region Private Fields

        private readonly IReadableContext _context;
        private readonly DbSet<TEntity> _dbSet;


        #endregion Private Fields

        public Repository(IReadableContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }


        #region READ OPERATIONS

        public virtual TEntity Find(params object[] keyValues)
        {
            return _dbSet.Find(keyValues);
        }

        public virtual IQueryable<TEntity> SelectQuery(string query, params object[] parameters)
        {
            return _dbSet.SqlQuery(query, parameters).AsQueryable();
        }

        public IQueryFluent<TEntity> Query()
        {
            return new QueryFluent<TEntity>(this);
        }

        public virtual IQueryFluent<TEntity> Query(IQueryObject<TEntity> queryObject)
        {
            return new QueryFluent<TEntity>(this, queryObject);
        }

        public virtual IQueryFluent<TEntity> Query(Expression<Func<TEntity, bool>> query)
        {
            return new QueryFluent<TEntity>(this, query);
        }

        public IEnumerable<TEntity> Query(string query, params object[] parameters)
        {
            throw new NotImplementedException();
        }

        public IQueryable<TEntity> Queryable()
        {
            return _dbSet;
        }

        public virtual async Task<TEntity> FindAsync(params object[] keyValues)
        {
            return await _dbSet.FindAsync(keyValues);
        }

        public virtual async Task<TEntity> FindAsync(CancellationToken cancellationToken, params object[] keyValues)
        {
            return await _dbSet.FindAsync(cancellationToken, keyValues);
        }


        public async Task<TDataTransferObject> GetById<TDataTransferObject>(int id)
        {
            return await Queryable().Where(entity => entity.Id == id).ProjectTo<TDataTransferObject>().FirstOrDefaultAsync();
        }


        public IQueryable<TEntity> Select(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            List<Expression<Func<TEntity, object>>> includes = null,
            int? page = null,
            int? pageSize = null)
        {
            IQueryable<TEntity> query = _dbSet;

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }
            if (orderBy != null)
            {
                query = orderBy(query);
            }
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (page != null && pageSize != null)
            {
                query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
            }
            return query;
        }

        public async Task<IEnumerable<TEntity>> SelectAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            List<Expression<Func<TEntity, object>>> includes = null,
            int? page = null,
            int? pageSize = null)
        {
            return await Select(filter, orderBy, includes, page, pageSize).ToListAsync();
        }

        #endregion

        #region ADD UPDATE DELETE OPERATIONS

        public virtual void Insert(TEntity entity)
        {
            entity.ObjectState = ObjectState.Added;
            _dbSet.Attach(entity);
        }

        public virtual void InsertRange(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                Insert(entity);
            }
        }

        public virtual void InsertGraphRange(IEnumerable<TEntity> entities)
        {
            _dbSet.AddRange(entities);
        }

        public virtual void Update(TEntity entity)
        {
            entity.ObjectState = ObjectState.Modified;
            _dbSet.Attach(entity);
        }

        public virtual void Delete(object id)
        {
            var entity = _dbSet.Find(id);
            Delete(entity);
        }

        public virtual void Delete(TEntity entity)
        {
            entity.ObjectState = ObjectState.Deleted;
            _dbSet.Attach(entity);
        }

        public virtual async Task<bool> DeleteAsync(params object[] keyValues)
        {
            return await DeleteAsync(CancellationToken.None, keyValues);
        }

        public virtual async Task<bool> DeleteAsync(CancellationToken cancellationToken, params object[] keyValues)
        {
            var entity = await FindAsync(cancellationToken, keyValues);

            if (entity == null)
            {
                return false;
            }

            entity.ObjectState = ObjectState.Deleted;
            _dbSet.Attach(entity);

            return true;
        }


        public virtual void InsertOrUpdateGraph(TEntity entity)
        {
            SyncObjectGraph(entity);
            _entitesChecked = null;
            _dbSet.Attach(entity);
        }

        HashSet<object> _entitesChecked; // tracking of all process entities in the object graph when calling SyncObjectGraph

        private void SyncObjectGraph(object entity) // scan object graph for all 
        {
            if (_entitesChecked == null)
                _entitesChecked = new HashSet<object>();

            if (_entitesChecked.Contains(entity))
                return;

            _entitesChecked.Add(entity);

            var objectState = entity as IObjectState;

            if (objectState != null && objectState.ObjectState == ObjectState.Added)
                //_context.SyncObjectState((IObjectState)entity);

                // Set tracking state for child collections
                foreach (var prop in entity.GetType().GetProperties())
                {
                    // Apply changes to 1-1 and M-1 properties
                    var trackableRef = prop.GetValue(entity, null) as IObjectState;
                    if (trackableRef != null)
                    {
                        if (trackableRef.ObjectState == ObjectState.Added)
                            //_context.SyncObjectState((IObjectState)entity);

                            SyncObjectGraph(prop.GetValue(entity, null));
                    }

                    // Apply changes to 1-M properties
                    var items = prop.GetValue(entity, null) as IEnumerable<IObjectState>;
                    if (items == null) continue;

                    Debug.WriteLine("Checking collection: " + prop.Name);

                    foreach (var item in items)
                        SyncObjectGraph(item);
                }
        }

        public void Add(TEntity entity)
        {
            this.Insert(entity);
        }

        #endregion

    }
}
