using LinnworksSales.Data.Models;
using LinnworksSales.Data.Repository.Interfaces;

namespace LinnworksSales.Data.Repository
{
    public class RegionRepository : Repository<Region>, IRegionRepository
    {
        public RegionRepository(DatabaseContext databaseContext) : base(databaseContext) { }
    }
}
