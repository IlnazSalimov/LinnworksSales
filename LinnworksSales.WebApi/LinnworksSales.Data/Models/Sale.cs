using LinnworksSales.WebApi.Data.Models.Entity;
using LinnworksSales.Data.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LinnworksSales.Data.Models
{
    [Table("Orders")]
    public class Sale : IEntity
    {
        [Key]
        [Column("OrderId")]
        public long Id { get; set; }
        [ForeignKey("CountryForeignKey")]
        public Country Country { get; set; }
        [ForeignKey("ItemTypeForeignKey")]
        public ItemType ItemType { get; set; }
        public SalesChanel SalesChanel { get; set; }
        public OrderPriority OrderPriority { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime ShipDate { get; set; }
        public int UnitsSold { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal UnitCost { get; set; }
    }
}