using LinnworksSales.Data.Data.Models.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LinnworksSales.Data.Models
{
    [Table("Countries")]
    public class Country : IEntity
    {
        [Key]
        public int Id { get; set; }
        public Region Region { get; set; }
        public string Name { get; set; }

        public List<Sale> Sales { get; set; }
    }
}
