using LinnworksSales.Data.Data.Models.Entity;
using LinnworksSales.Data.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LinnworksSales.Data.Models
{
    [Table("Sales")]
    public class Sale : IEntity
    {
        [Key]
        public int Id { get; set; }
        public Country Country { get; set; }
        public ItemType ItemType { get; set; }
        public SalesChanel SalesChanel { get; set; }
        public OrderPriority OrderPriority { get; set; }
        public long OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime ShipDate { get; set; }
        public int UnitsSold { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal UnitCost { get; set; }
        [NotMapped]
        public decimal TotalRevenue => UnitPrice * UnitsSold;
        [NotMapped]
        public decimal TotalCost => UnitCost * UnitsSold;
        [NotMapped]
        public decimal TotalProfit => TotalRevenue - TotalCost;
    }
}