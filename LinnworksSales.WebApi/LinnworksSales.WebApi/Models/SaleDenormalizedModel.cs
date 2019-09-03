namespace LinnworksSales.WebApi.Models
{
    public class SaleDenormalizedModel
    {
        public SaleDenormalizedModel(string[] lineData)
        {
                Region = lineData[0];
                Country = lineData[1];
                ItemType = lineData[2];
                SalesChanel = lineData[3];
                OrderPriority = lineData[4];
                OrderDate = lineData[5];
                OrderId = lineData[6];
                ShipDate = lineData[7];
                UnitsSold = lineData[8];
                UnitPrice = lineData[9];
                UnitCost = lineData[10];
        }

        
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
