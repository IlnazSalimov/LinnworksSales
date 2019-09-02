using LinnworksSales.Data.Models;
using System.Linq;

namespace LinnworksSales.Data.Repository.Interfaces
{
    public interface IFilterableRepository<TEntity>
    {
        IQueryable<TEntity> FilterBy(IQueryable<TEntity> entities, params Filter[] filters);
    }
}
