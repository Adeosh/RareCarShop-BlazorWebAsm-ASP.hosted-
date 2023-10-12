using Microsoft.EntityFrameworkCore;
using RareCarShop.Server.Data;
using RareCarShop.Server.Services.AuthService;
using RareCarShop.Server.Services.CartService;
using RareCarShop.Shared;
using RareCarShop.Shared.Customs;

namespace RareCarShop.Server.Services.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly DataContext _context;
        private readonly ICartService _cartService;
        private readonly IAuthService _authService;

        public OrderService(DataContext context,
            ICartService cartService,
            IAuthService authService)
        {
            _context = context;
            _cartService = cartService;
            _authService = authService;
        }

        public async Task<ServiceResponse<OrderDetailsResponse>> GetOrderDetails(int orderId)
        {
            var response = new ServiceResponse<OrderDetailsResponse>();
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Cars)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.CarEquipment)
                .Where(o => o.UserId == _authService.GetUserId() && o.Id == orderId)
                .OrderByDescending(o => o.OrderDate)
                .FirstOrDefaultAsync();

            if (order == null)
            {
                response.Success = false;
                response.Message = "Заказ не найден.";
                return response;
            }

            var orderDetailsResponse = new OrderDetailsResponse
            {
                OrderDate = order.OrderDate,
                TotalPrice = order.TotalPrice,
                Cars = new List<OrderDetailsCarResponse>()
            };

            order.OrderItems.ForEach(item =>
            orderDetailsResponse.Cars.Add(new OrderDetailsCarResponse
            {
                CarId = item.CarId,
                ImageUrl = item.Cars.ImageUrl1,
                CarEquipment = item.CarEquipment.Name,
                Quantity = item.Quantity,
                Brand = item.Cars.Brand,
                TotalPrice = item.TotalPrice
            }));

            response.Data = orderDetailsResponse;

            return response;
        }

        public async Task<ServiceResponse<List<OrderOverviewResponse>>> GetOrders()
        {
            var response = new ServiceResponse<List<OrderOverviewResponse>>();
            var orders = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Cars)
                .Where(o => o.UserId == _authService.GetUserId())
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            var orderResponse = new List<OrderOverviewResponse>();
            orders.ForEach(o => orderResponse.Add(new OrderOverviewResponse
            {
                Id = o.Id,
                OrderDate = o.OrderDate,
                TotalPrice = o.TotalPrice,
                Car = o.OrderItems.Count > 1 ?
                    $"{o.OrderItems.First().Cars.Brand} и" +
                    $" {o.OrderItems.Count - 1} больше..." :
                    o.OrderItems.First().Cars.Brand,
                CarImageUrl = o.OrderItems.First().Cars.ImageUrl1
            }));

            response.Data = orderResponse;

            return response;
        }

        public async Task<ServiceResponse<bool>> PlaceOrder(int userId)
        {
            var cars = (await _cartService.GetDbCartCars(userId)).Data;
            decimal totalPrice = 0;
            cars.ForEach(car => totalPrice += car.Price * car.Quantity);

            var orderItems = new List<OrderItem>();
            cars.ForEach(car => orderItems.Add(new OrderItem
            {
                CarId = car.CarId,
                CarEquipmentId = car.CarEquipmentId,
                Quantity = car.Quantity,
                TotalPrice = car.Price * car.Quantity
            }));

            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.Now,
                TotalPrice = totalPrice,
                OrderItems = orderItems
            };

            _context.Orders.Add(order);

            _context.CartItems.RemoveRange(_context.CartItems
                .Where(ci => ci.UserId == userId));

            await _context.SaveChangesAsync();

            return new ServiceResponse<bool> { Data = true };
        }
    }
}
