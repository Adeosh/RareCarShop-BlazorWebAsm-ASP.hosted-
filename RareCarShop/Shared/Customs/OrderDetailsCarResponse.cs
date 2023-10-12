using System;

namespace RareCarShop.Shared.Customs
{
    public class OrderDetailsCarResponse
    {
        public int CarId { get; set; }
        public string Brand { get; set; }
        public string CarEquipment { get; set; }
        public string ImageUrl { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
