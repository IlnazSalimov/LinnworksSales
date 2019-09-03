using System.Linq;
using LinnworksSales.Data.Models;
using LinnworksSales.Data.Repository.Interfaces;

namespace LinnworksSales.Data.Repository
{
    public class SaleRepository : Repository<Sale>, ISaleRepository
    {
        public SaleRepository(DatabaseContext databaseContext) : base(databaseContext) { }

        public IQueryable<Sale> FilterBy(IQueryable<Sale> entities, params Filter[] filters)
        {
            foreach(Filter filter in filters)
            {
                if(filter.Type == typeof(Country))
                {
                    if(filter.HasSelectedValue && int.TryParse(filter.SelectedValue, out var selectdCountryId))
                    {
                        entities = entities.Where(s => s.Country.Id == selectdCountryId);
                    }
                }
            }

            return entities;
        }
    }
}
