using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using RareCarShop.Shared.Shop;

namespace RareCarShop.Shared.Shop
{
    public class Car
    {
        public int Id { get; set; }

        [Required]
        public string Brand { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string BodyType { get; set; } = string.Empty;
        public string Drivetrain { get; set; } = string.Empty;
        public string EngineCapacity {  get; set; } = string.Empty;
        public string MaxSpeed { get; set; } = string.Empty;
        public string ImageUrl1 { get; set; } = string.Empty;
        public string ImageUrl2 { get; set; } = string.Empty;
        public string ImageUrl3 { get; set; } = string.Empty;
        public List<Image> Images { get; set; } = new List<Image>();
        public Category? Category { get; set; }
        public int CategoryId { get; set; }
        public bool Featured { get; set; } = false;
        public List<PriceVariant> Variants { get; set; } = new List<PriceVariant>();
        public bool Visible { get; set; } = true;
        public bool Deleted { get; set; } = false;

        [NotMapped]
        public bool Editing { get; set; } = false;

        [NotMapped]
        public bool IsNew { get; set; } = false;
    }
}
