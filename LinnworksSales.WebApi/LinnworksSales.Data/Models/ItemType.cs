using LinnworksSales.WebApi.Data.Models.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace LinnworksSales.Data.Models
{
    public class ItemType : IEntity
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }
}
