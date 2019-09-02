using LinnworksSales.Data.Data.Models.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LinnworksSales.Data.Models
{
    public class ItemTypeDto : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
