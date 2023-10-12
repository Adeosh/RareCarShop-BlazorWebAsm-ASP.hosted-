using RareCarShop.Shared.Customs;
using RareCarShop.Shared;

namespace RareCarShop.Server.Services.CartService
{
    public interface ICartService
    {
        Task<ServiceResponse<List<CartCarResponse>>> GetCartCars(List<CartItem> cartItems);
        Task<ServiceResponse<List<CartCarResponse>>> StoreCartItems(List<CartItem> cartItems);
        Task<ServiceResponse<int>> GetCartItemsCount();
        Task<ServiceResponse<List<CartCarResponse>>> GetDbCartCars(int? userId = null);
        Task<ServiceResponse<bool>> AddToCart(CartItem cartItem);
        Task<ServiceResponse<bool>> UpdateQuantity(CartItem cartItem);
        Task<ServiceResponse<bool>> RemoveItemFromCart(int carId, int carEquipId);
    }
}
