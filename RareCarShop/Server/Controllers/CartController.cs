using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RareCarShop.Server.Services.CartService;
using RareCarShop.Shared.Customs;
using RareCarShop.Shared;

namespace RareCarShop.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpPost("cars")]
        public async Task<ActionResult<ServiceResponse<List<CartCarResponse>>>> GetCartProducts(List<CartItem> cartItems)
        {
            var result = await _cartService.GetCartCars(cartItems);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<List<CartCarResponse>>>> StoreCartItems(List<CartItem> cartItems)
        {
            var result = await _cartService.StoreCartItems(cartItems);
            return Ok(result);
        }

        [HttpPost("add")]
        public async Task<ActionResult<ServiceResponse<bool>>> AddToCart(CartItem cartItem)
        {
            var result = await _cartService.AddToCart(cartItem);
            return Ok(result);
        }

        [HttpPut("update-quantity")]
        public async Task<ActionResult<ServiceResponse<bool>>> UpdateQuantity(CartItem cartItem)
        {
            var result = await _cartService.UpdateQuantity(cartItem);
            return Ok(result);
        }

        [HttpDelete("{carId}/{carEquipmentId}")]
        public async Task<ActionResult<ServiceResponse<bool>>> RemoveItemFromCart(int carId, int carEquipmentId)
        {
            var result = await _cartService.RemoveItemFromCart(carId, carEquipmentId);
            return Ok(result);
        }

        [HttpGet("count")]
        public async Task<ActionResult<ServiceResponse<int>>> GetCartItemsCount()
        {
            return await _cartService.GetCartItemsCount();
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<CartCarResponse>>>> GetDbCartProducts()
        {
            var result = await _cartService.GetDbCartCars();
            return Ok(result);
        }
    }
}
