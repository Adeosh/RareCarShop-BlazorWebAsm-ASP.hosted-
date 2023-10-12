using RareCarShop.Shared.Shop;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace RareCarShop.Shared.Shop
{
    public class PriceVariant
    {
        [JsonIgnore]
        public Car? Car { get; set; }
        public int CarId { get; set; }
        public CarEquipment? CarEquipment { get; set; }
        public int CarEquipmentId { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal OriginalPrice { get; set; }

        public bool Visible { get; set; } = true;
        public bool Deleted { get; set; } = false;

        [NotMapped]
        public bool Editing { get; set; } = false;

        [NotMapped]
        public bool IsNew { get; set; } = false;
    }
}
