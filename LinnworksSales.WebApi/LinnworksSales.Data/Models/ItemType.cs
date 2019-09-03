using LinnworksSales.Data.Models.Entity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LinnworksSales.Data.Models
{
    [Table("ItemTypes")]
    public class ItemType : IEntity
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Sale> Sales { get; set; }
    }
}
