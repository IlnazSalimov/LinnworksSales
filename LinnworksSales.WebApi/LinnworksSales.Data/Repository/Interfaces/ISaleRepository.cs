using LinnworksSales.Data.Models;

namespace LinnworksSales.Data.Repository.Interfaces
{
    public interface ISaleRepository : IRepository<Sale>, IFilterableRepository<Sale> { }
}
