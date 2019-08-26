using LinnworksSales.WebApi.Data.Models.Entity;
using System.ComponentModel.DataAnnotations;

namespace LinnworksSales.Data.Models
{
    public class Region : IEntity
    {
        [Key]
        public long Id { get; set; }
        public string Name { get; set; }
    }
}
