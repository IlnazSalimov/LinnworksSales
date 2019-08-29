using LinnworksSales.WebApi.Data.Models.Entity;
using LinnworksSales.WebApi.Data.Repository.Interfaces;
using LinnworksSales.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace LinnworksSales.WebApi.Data.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
    {
        private DatabaseContext DbContext { get; set; }

        public Repository() { }

        public Repository(DatabaseContext context)
        {
            DbContext = context;
        }

        public bool Delete(TEntity entity)
        {
            DbContext.Remove(entity);
            return DbContext.SaveChanges() > 0;
        }

        public async Task<TEntity> GetAsync(int id)
        {
            return await DbContext.FindAsync<TEntity>(id);
        }

        public IQueryable<TEntity> GetAll()
        {
            return DbContext.Set<TEntity>();
        }

        public async Task SaveAsync(TEntity entity)
        {
            await DbContext.AddAsync(entity);
        }

        public bool Update(TEntity entity)
        {
            DbContext.Update(entity);
            return DbContext.SaveChanges() > 0;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await DbContext.SaveChangesAsync();
        }
    }
}
