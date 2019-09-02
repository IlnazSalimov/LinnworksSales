using LinnworksSales.Data.Models;
using LinnworksSales.Data.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace LinnworksSales.Data.Data.Repository.Interfaces
{
    public interface ISaleRepository : IRepository<Sale>, IFilterableRepository<Sale> { }
}
