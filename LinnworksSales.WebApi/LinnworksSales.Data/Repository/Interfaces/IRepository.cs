using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinnworksSales.Data.Models.Entity;
using Z.BulkOperations;

namespace LinnworksSales.Data.Repository.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class, IEntity
    {
        /// <summary>
        /// Get entity by ID
        /// </summary>
        /// <param name="id">Entity ID</param>
        /// <returns>Entity with the specified Id, if exists. Else, null.</returns>
        Task<TEntity> GetAsync(int id);

        TEntity Get(int id);

        /// <summary>
        /// Loading all objects of this entity
        /// </summary>
        /// <returns>Unordered list of all objects</returns>
        IQueryable<TEntity> GetAll();

        /// <summary>
        /// Save entity object
        /// </summary>
        /// <param name="entity">Saved object</param>
        Task<bool> SaveAsync(TEntity entity);

        /// <summary>
        /// The EF extension method let you insert a large number of entities in your database.
        /// </summary>
        /// <param name="collection">Entities collection</param>
        Task BulkInsertAsync(IEnumerable<TEntity> collection);

        /// <summary>
        /// The EF extension method let you merge (insert or update/Upsert) a large number of entities in database.
        /// </summary>
        /// <param name="collection">Entities collection</param>
        Task BulkMergeAsync(IEnumerable<TEntity> collection);

        /// <summary>
        /// The EF extension method let you merge (insert or update/Upsert) a large number of entities in database.
        /// </summary>
        /// <param name="collection">Entities collection</param>
        /// <param name="options">Let you use a lambda expression to customize the way entities are inserted or updated</param>
        Task BulkMergeAsync(IEnumerable<TEntity> collection, Action<BulkOperation<TEntity>> options);

        /// <summary>
        /// Update entity object
        /// </summary>
        /// <param name="entity">Updated object</param>
        bool Update(TEntity entity);

        /// <summary>
        /// Delete entity object
        /// </summary>
        /// <param name="entity">Deleted object</param>
        bool Delete(TEntity entity);

        /// <summary>
        /// Bulk delete entity objects
        /// </summary>
        /// <param name="entities">Deleted objects</param>
        Task BulkDeleteAsync(IEnumerable<TEntity> entities);
    }
}
