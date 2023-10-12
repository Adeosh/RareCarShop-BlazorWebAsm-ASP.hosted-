using System;

namespace RareCarShop.Shared.Customs
{
    public class OrderOverviewResponse
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public string Car { get; set; }
        public string CarImageUrl { get; set; }
    }
}
