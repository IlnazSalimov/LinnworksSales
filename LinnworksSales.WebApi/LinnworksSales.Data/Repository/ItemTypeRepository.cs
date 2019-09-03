using LinnworksSales.Data.Models;
using LinnworksSales.Data.Repository.Interfaces;

namespace LinnworksSales.Data.Repository
{
    public class ItemTypeRepository : Repository<ItemType>, IItemTypeRepository
    {
        public ItemTypeRepository(DatabaseContext databaseContext) : base(databaseContext) { }
    }
}
