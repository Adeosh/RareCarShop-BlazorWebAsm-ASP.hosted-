using RareCarShop.Server.Services.AuthService;
using RareCarShop.Server.Services.CartService;
using RareCarShop.Server.Services.OrderService;
using RareCarShop.Shared;
using Stripe;
using Stripe.Checkout;

namespace RareCarShop.Server.Services.PaymentService
{
    public class PaymentService : IPaymentService
    {
        private readonly ICartService _cartService;
        private readonly IAuthService _authService;
        private readonly IOrderService _orderService;

        const string secret = "b2df07aa-1aea-4441-8a32-826385fc3ed8";

        public PaymentService(ICartService cartService,
            IAuthService authService,
            IOrderService orderService)
        {
            StripeConfiguration.ApiKey = "sk_test_51NpR8NJDQoa3gi22sFpR4kQehEnupXqEtxLluyIwcQjArTSDg5CDJIpG1NKS0US7zFDWNs3kRVDTBuZNwyo4tc8X00ZU2Nm977";

            _cartService = cartService;
            _authService = authService;
            _orderService = orderService;
        }

        public async Task<Session> CreateCheckoutSession()
        {
            var cars = (await _cartService.GetDbCartCars()).Data;
            var lineItems = new List<SessionLineItemOptions>();
            cars.ForEach(car => lineItems.Add(new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    UnitAmountDecimal = car.Price * 100,
                    Currency = "usd",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = car.Brand,
                        Images = new List<string> { car.ImageUrl }
                    }
                },
                Quantity = car.Quantity
            }));

            var options = new SessionCreateOptions
            {
                CustomerEmail = _authService.GetUserEmail(),
                ShippingAddressCollection =
                    new SessionShippingAddressCollectionOptions
                    {
                        AllowedCountries = new List<string> { "US" }
                    },
                PaymentMethodTypes = new List<string>
                {
                    "card"
                },
                LineItems = lineItems,
                Mode = "payment",
                SuccessUrl = "https://localhost:7166/order-success",
                CancelUrl = "https://localhost:7166/cart"
            };

            var service = new SessionService();
            Session session = service.Create(options);
            return session;
        }

        public async Task<ServiceResponse<bool>> FulfillOrder(HttpRequest request)
        {
            var json = await new StreamReader(request.Body).ReadToEndAsync();
            try
            {
                var stripeEvent = EventUtility.ConstructEvent(
                        json,
                        request.Headers["Stripe-Signature"],
                        secret
                    );

                if (stripeEvent.Type == Events.CheckoutSessionCompleted)
                {
                    var session = stripeEvent.Data.Object as Session;
                    var user = await _authService.GetUserByEmail(session.CustomerEmail);
                    await _orderService.PlaceOrder(user.Id);
                }

                return new ServiceResponse<bool> { Data = true };
            }
            catch (StripeException e)
            {
                return new ServiceResponse<bool> { Data = false, Success = false, Message = e.Message };
            }
        }
    }
}
