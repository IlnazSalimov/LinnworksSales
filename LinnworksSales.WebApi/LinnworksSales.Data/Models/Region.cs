using LinnworksSales.WebApi.Data.Models.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LinnworksSales.Data.Models
{
    [Table("Regions")]
    public class Region : IEntity
    {
        [Key]
        public long Id { get; set; }
        public string Name { get; set; }
    }
}
