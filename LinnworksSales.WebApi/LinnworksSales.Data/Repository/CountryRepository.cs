using LinnworksSales.Data.Models;
using LinnworksSales.Data.Repository.Interfaces;

namespace LinnworksSales.Data.Repository
{
    public class CountryRepository : Repository<Country>, ICountryRepository
    {
        public CountryRepository(DatabaseContext databaseContext) : base(databaseContext) { }
    }
}
