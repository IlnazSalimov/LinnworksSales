using System.ComponentModel.DataAnnotations;
using LinnworksSales.Data.Models.Entity;

namespace LinnworksSales.Data.Models.Dto
{
    public class ItemTypeDto : IEntity
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
