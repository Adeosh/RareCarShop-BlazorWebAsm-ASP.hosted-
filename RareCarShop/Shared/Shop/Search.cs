using System;

namespace RareCarShop.Shared.Shop
{
    public class Search
    {
        public List<Car> Cars { get; set; } = new List<Car>();
        public int Pages { get; set; }
        public int CurrentPage { get; set; }
    }
}
