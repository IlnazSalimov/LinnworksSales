using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinnworksSales.Data.Models.Entity;
using LinnworksSales.Data.Repository.Interfaces;
using Z.BulkOperations;

namespace LinnworksSales.Data.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
    {
        private DatabaseContext DatabaseContext { get; set; }

        public Repository() { }

        public Repository(DatabaseContext context)
        {
            DatabaseContext = context;
        }

        public bool Delete(TEntity entity)
        {
            DatabaseContext.Remove(entity);
            return DatabaseContext.SaveChanges() > 0;
        }

        public async Task BulkDeleteAsync(IEnumerable<TEntity> entities)
        {
            await DatabaseContext.BulkDeleteAsync<TEntity>(entities);
        }

        public async Task<TEntity> GetAsync(int id)
        {
            return await DatabaseContext.FindAsync<TEntity>(id);
        }

        public TEntity Get(int id)
        {
            return DatabaseContext.Find<TEntity>(id);
        }

        public IQueryable<TEntity> GetAll()
        {
            return DatabaseContext.Set<TEntity>();
        }

        public async Task<bool> SaveAsync(TEntity entity)
        {
            await DatabaseContext.AddAsync(entity);
            return DatabaseContext.SaveChanges() > 0;
        }

        public async Task BulkInsertAsync(IEnumerable<TEntity> collection)
        {
            await DatabaseContext.BulkInsertAsync<TEntity>(collection);
        }

        public async Task BulkMergeAsync(IEnumerable<TEntity> collection)
        {
            await DatabaseContext.BulkMergeAsync<TEntity>(collection);
        }

        public async Task BulkMergeAsync(IEnumerable<TEntity> collection, Action<BulkOperation<TEntity>> options)
        {
            await DatabaseContext.BulkMergeAsync<TEntity>(collection, options);
        }

        public bool Update(TEntity entity)
        {
            DatabaseContext.Update(entity);
            return DatabaseContext.SaveChanges() > 0;
        }
    }
}
