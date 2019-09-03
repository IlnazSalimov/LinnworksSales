namespace LinnworksSales.Data.Models.Dto
{
    public class SalePutDto
    {
        public long Id { get; set; }
        public int CountryId { get; set; }
        public int ItemTypeId { get; set; }
        public string SalesChanel { get; set; }
        public string OrderPriority { get; set; }
        public long OrderId { get; set; }
        public string OrderDate { get; set; }
        public string ShipDate { get; set; }
        public int UnitsSold { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal UnitCost { get; set; }
    }
}