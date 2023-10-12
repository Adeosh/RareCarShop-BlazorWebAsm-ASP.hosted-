using Microsoft.EntityFrameworkCore;
using RareCarShop.Server.Data;
using RareCarShop.Server.Services.AuthService;
using RareCarShop.Shared;
using RareCarShop.Shared.Customs;

namespace RareCarShop.Server.Services.CartService
{
    public class CartService : ICartService
    {
        private readonly DataContext _context;
        private readonly IAuthService _authService;

        public CartService(DataContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        public async Task<ServiceResponse<bool>> AddToCart(CartItem cartItem)
        {
            cartItem.UserId = _authService.GetUserId();

            var sameItem = await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.CarId == cartItem.CarId &&
                ci.CarEquipmentId == cartItem.CarEquipmentId && ci.UserId == cartItem.UserId);
            if (sameItem == null)
            {
                _context.CartItems.Add(cartItem);
            }
            else
            {
                sameItem.Quantity += cartItem.Quantity;
            }

            await _context.SaveChangesAsync();

            return new ServiceResponse<bool> { Data = true };
        }

        public async Task<ServiceResponse<int>> GetCartItemsCount()
        {
            var count = (await _context.CartItems.Where(ci => ci.UserId == _authService.GetUserId()).ToListAsync()).Count;
            return new ServiceResponse<int> { Data = count };
        }

        public async Task<ServiceResponse<List<CartCarResponse>>> GetCartCars(List<CartItem> cartItems)
        {
            var result = new ServiceResponse<List<CartCarResponse>>
            {
                Data = new List<CartCarResponse>()
            };

            foreach (var item in cartItems)
            {
                var product = await _context.Cars
                    .Where(c => c.Id == item.CarId)
                    .FirstOrDefaultAsync();

                if (product == null)
                {
                    continue;
                }

                var productVariant = await _context.PriceVariants
                    .Where(v => v.CarId == item.CarId
                        && v.CarEquipmentId == item.CarEquipmentId)
                    .Include(v => v.CarEquipment)
                    .FirstOrDefaultAsync();

                if (productVariant == null)
                {
                    continue;
                }

                var cartProduct = new CartCarResponse
                {
                    CarId = product.Id,
                    Brand = product.Brand,
                    ImageUrl = product.ImageUrl1,
                    Price = productVariant.Price,
                    CarEquipment = productVariant.CarEquipment.Name,
                    CarEquipmentId = productVariant.CarEquipmentId,
                    Quantity = item.Quantity
                };

                result.Data.Add(cartProduct);
            }

            return result;
        }

        public async Task<ServiceResponse<List<CartCarResponse>>> GetDbCartCars(int? userId = null)
        {
            if (userId == null)
                userId = _authService.GetUserId();

            return await GetCartCars(await _context.CartItems
                .Where(ci => ci.UserId == userId).ToListAsync());
        }

        public async Task<ServiceResponse<bool>> RemoveItemFromCart(int carId, int carEquipId)
        {
            var dbCartItem = await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.CarId == carId &&
                ci.CarEquipmentId == carEquipId && ci.UserId == _authService.GetUserId());
            if (dbCartItem == null)
            {
                return new ServiceResponse<bool>
                {
                    Data = false,
                    Success = false,
                    Message = "Товар в корзине не существует."
                };
            }

            _context.CartItems.Remove(dbCartItem);
            await _context.SaveChangesAsync();

            return new ServiceResponse<bool> { Data = true };
        }

        public async Task<ServiceResponse<List<CartCarResponse>>> StoreCartItems(List<CartItem> cartItems)
        {
            cartItems.ForEach(cartItem => cartItem.UserId = _authService.GetUserId());
            _context.CartItems.AddRange(cartItems);
            await _context.SaveChangesAsync();

            return await GetDbCartCars();
        }

        public async Task<ServiceResponse<bool>> UpdateQuantity(CartItem cartItem)
        {
            var dbCartItem = await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.CarId == cartItem.CarId &&
                ci.CarEquipmentId == cartItem.CarEquipmentId && ci.UserId == _authService.GetUserId());
            if (dbCartItem == null)
            {
                return new ServiceResponse<bool>
                {
                    Data = false,
                    Success = false,
                    Message = "Товар в корзине не существует."
                };
            }

            dbCartItem.Quantity = cartItem.Quantity;
            await _context.SaveChangesAsync();

            return new ServiceResponse<bool> { Data = true };
        }
    }
}
