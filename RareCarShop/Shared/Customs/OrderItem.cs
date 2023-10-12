using RareCarShop.Shared.Shop;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace RareCarShop.Shared.Customs
{
    public class OrderItem
    {
        public Order Order { get; set; }
        public int OrderId { get; set; }
        public Car Cars { get; set; }
        public int CarId { get; set; }
        public CarEquipment CarEquipment { get; set; }
        public int CarEquipmentId { get; set; }
        public int Quantity { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice { get; set; }
    }
}
