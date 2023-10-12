using Blazored.LocalStorage;
using RareCarShop.Client.Services.AuthService;
using RareCarShop.Shared;
using RareCarShop.Shared.Customs;
using System.Net.Http.Json;

namespace RareCarShop.Client.Services.CartService
{
    public class CartService : ICartService
    {
        private readonly ILocalStorageService _localStorage;
        private readonly HttpClient _httpClient;
        private readonly IAuthService _authService;

        public CartService(ILocalStorageService localStorage, HttpClient httpClient,
            IAuthService authService)
        {
            _localStorage = localStorage;
            _httpClient = httpClient;
            _authService = authService;
        }

        public event Action OnChange;

        public async Task AddToCart(CartItem cartItem)
        {
            if (await _authService.IsUserAuthenticated())
            {
                await _httpClient.PostAsJsonAsync("api/cart/add", cartItem);
            }
            else
            {
                var cart = await _localStorage.GetItemAsync<List<CartItem>>("cart");
                if (cart == null)
                {
                    cart = new List<CartItem>();
                }

                var sameItem = cart.Find(x => x.CarId == cartItem.CarId &&
                    x.CarEquipmentId == cartItem.CarEquipmentId);
                if (sameItem == null)
                {
                    cart.Add(cartItem);
                }
                else
                {
                    sameItem.Quantity += cartItem.Quantity;
                }

                await _localStorage.SetItemAsync("cart", cart);
            }
            await GetCartItemsCount();
        }

        public async Task<List<CartCarResponse>> GetCartCars()
        {
            if (await _authService.IsUserAuthenticated())
            {
                var response = await _httpClient.GetFromJsonAsync<ServiceResponse<List<CartCarResponse>>>("api/cart");
                return response.Data;
            }
            else
            {
                var cartItems = await _localStorage.GetItemAsync<List<CartItem>>("cart");
                if (cartItems == null)
                    return new List<CartCarResponse>();
                var response = await _httpClient.PostAsJsonAsync("api/cart/cars", cartItems);
                var cartCars =
                    await response.Content.ReadFromJsonAsync<ServiceResponse<List<CartCarResponse>>>();
                return cartCars.Data;
            }
        }

        public async Task GetCartItemsCount()
        {
            if (await _authService.IsUserAuthenticated())
            {
                var result = await _httpClient.GetFromJsonAsync<ServiceResponse<int>>("api/cart/count");
                var count = result.Data;

                await _localStorage.SetItemAsync<int>("cartItemsCount", count);
            }
            else
            {
                var cart = await _localStorage.GetItemAsync<List<CartItem>>("cart");
                await _localStorage.SetItemAsync<int>("cartItemsCount", cart != null ? cart.Count : 0);
            }

            OnChange?.Invoke();
        }

        public async Task RemoveCarFromCart(int carId, int carEquipId)
        {
            if (await _authService.IsUserAuthenticated())
            {
                await _httpClient.DeleteAsync($"api/cart/{carId}/{carEquipId}");
            }
            else
            {
                var cart = await _localStorage.GetItemAsync<List<CartItem>>("cart");
                if (cart == null)
                {
                    return;
                }

                var cartItem = cart.Find(x => x.CarId == carId
                    && x.CarEquipmentId == carEquipId);
                if (cartItem != null)
                {
                    cart.Remove(cartItem);
                    await _localStorage.SetItemAsync("cart", cart);
                }
            }
        }

        public async Task StoreCartItems(bool emptyLocalCart)
        {
            var localCart = await _localStorage.GetItemAsync<List<CartItem>>("cart");
            if (localCart == null)
            {
                return;
            }

            await _httpClient.PostAsJsonAsync("api/cart", localCart);

            if (emptyLocalCart)
            {
                await _localStorage.RemoveItemAsync("cart");
            }
        }

        public async Task UpdateQuantity(CartCarResponse car)
        {
            if (await _authService.IsUserAuthenticated())
            {
                var request = new CartItem
                {
                    CarId = car.CarId,
                    Quantity = car.Quantity,
                    CarEquipmentId = car.CarEquipmentId
                };
                await _httpClient.PutAsJsonAsync("api/cart/update-quantity", request);
            }
            else
            {
                var cart = await _localStorage.GetItemAsync<List<CartItem>>("cart");
                if (cart == null)
                {
                    return;
                }

                var cartItem = cart.Find(x => x.CarId == car.CarId
                    && x.CarEquipmentId == car.CarEquipmentId);
                if (cartItem != null)
                {
                    cartItem.Quantity = car.Quantity;
                    await _localStorage.SetItemAsync("cart", cart);
                }
            }
        }
    }
}
