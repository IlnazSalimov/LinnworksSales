namespace LinnworksSales.WebApi.Models
{
    public class SaleDto
    {
        public long Id { get; set; }
        public string Country { get; set; }
        public string ItemType { get; set; }
        public string Chanel { get; set; }
        public string Priority { get; set; }
        public string OrderDate { get; set; }
        public string ShipDate { get; set; }
        public int UnitsSold { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal UnitCost { get; set; }
    }
}