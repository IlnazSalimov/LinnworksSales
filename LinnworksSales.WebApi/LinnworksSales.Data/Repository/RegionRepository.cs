using System;
using System.Collections.Generic;
using System.Text;
using LinnworksSales.Data.Data.Repository.Interfaces;
using LinnworksSales.Data;
using LinnworksSales.Data.Models;

namespace LinnworksSales.Data.Data.Repository
{
    public class RegionRepository : Repository<Region>, IRegionRepository
    {
        public RegionRepository(DatabaseContext databaseContext) : base(databaseContext) { }
    }
}
