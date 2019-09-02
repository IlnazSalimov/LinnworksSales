using LinnworksSales.Data.Data.Repository.Interfaces;
using LinnworksSales.Data.Models;
using System.Linq;

namespace LinnworksSales.Data.Data.Repository
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
                    int selectdCountryId;
                    if(filter.HasSelectedValue && int.TryParse(filter.SelectedValue, out selectdCountryId))
                    {
                        entities = entities.Where(s => s.Country.Id == selectdCountryId);
                    }
                }
            }

            return entities;
        }
    }
}
