using System;

namespace RareCarShop.Shared.Customs
{
    public class CartCarResponse
    {
        public int CarId { get; set; }
        public string Brand { get; set; } = string.Empty;
        public int CarEquipmentId { get; set; }
        public string CarEquipment { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
