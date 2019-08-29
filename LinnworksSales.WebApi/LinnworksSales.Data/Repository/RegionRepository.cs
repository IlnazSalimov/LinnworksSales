using System;
using System.Collections.Generic;
using System.Text;
using LinnworksSales.WebApi.Data.Repository.Interfaces;
using LinnworksSales.Data;
using LinnworksSales.Data.Models;

namespace LinnworksSales.WebApi.Data.Repository
{
    public class RegionRepository : Repository<Region>, IRegionRepository
    {
        public RegionRepository(DatabaseContext databaseContext) : base(databaseContext) { }
    }
}
