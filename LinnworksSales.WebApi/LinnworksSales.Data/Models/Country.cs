using LinnworksSales.WebApi.Data.Models.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LinnworksSales.Data.Models
{
    public class Country : IEntity
    {
        [Key]
        public long Id { get; set; }
        public Region Region { get; set; }
        public string Name { get; set; }
    }
}
