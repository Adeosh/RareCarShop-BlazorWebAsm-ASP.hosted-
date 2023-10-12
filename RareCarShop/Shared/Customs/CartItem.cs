using System;

namespace RareCarShop.Shared.Customs
{
    public class CartItem
    {
        public int UserId { get; set; }
        public int CarId { get; set; }
        public int CarEquipmentId { get; set; }
        public int Quantity { get; set; } = 1;
    }
}
