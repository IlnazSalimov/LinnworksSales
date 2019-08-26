using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LinnworksSales.WebApi.Models
{
    public class SaleDenormalizedModel
    {
        public string OrderId { get; set; }
        public string Region { get; set; }
        public string Country { get; set; }
        public string ItemType { get; set; }
        public string SalesChanel { get; set; }
        public string OrderPriority { get; set; }
        public string OrderDate { get; set; }
        public string ShipDate { get; set; }
        public string UnitsSold { get; set; }
        public string UnitPrice { get; set; }
        public string UnitCost { get; set; }
    }
}
