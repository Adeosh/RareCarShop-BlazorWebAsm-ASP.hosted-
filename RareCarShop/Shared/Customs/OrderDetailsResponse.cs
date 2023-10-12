using System;

namespace RareCarShop.Shared.Customs
{
    public class OrderDetailsResponse
    {
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public List<OrderDetailsCarResponse> Cars { get; set; }
    }
}
