using RareCarShop.Shared.Customs;

namespace RareCarShop.Client.Services.CartService
{
    public interface ICartService
    {
        event Action OnChange;
        Task AddToCart(CartItem cartItem);
        Task<List<CartCarResponse>> GetCartCars();
        Task RemoveCarFromCart(int carId, int carEquipId);
        Task UpdateQuantity(CartCarResponse car);
        Task StoreCartItems(bool emptyLocalCart);
        Task GetCartItemsCount();
    }
}
