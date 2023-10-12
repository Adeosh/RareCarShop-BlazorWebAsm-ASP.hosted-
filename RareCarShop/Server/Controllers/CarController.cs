using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RareCarShop.Server.Services.CarService;
using RareCarShop.Shared;
using RareCarShop.Shared.Shop;

namespace RareCarShop.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarController : ControllerBase
    {
        private readonly ICarService _carService;

        public CarController(ICarService carService)
        {
            _carService = carService;
        }

        [HttpGet("admin"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<List<Car>>>> GetAdminCars()
        {
            var result = await _carService.GetAdminCars();
            return Ok(result);
        }

        [HttpPost, Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<Car>>> CreateCar(Car car)
        {
            var result = await _carService.CreateCar(car);
            return Ok(result);
        }

        [HttpPut, Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<Car>>> UpdateCar(Car car)
        {
            var result = await _carService.UpdateCar(car);
            return Ok(result);
        }

        [HttpDelete("{id}"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<bool>>> DeleteCar(int id)
        {
            var result = await _carService.DeleteCar(id);
            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<Car>>>> GetCars()
        {
            var result = await _carService.GetCarsAsync();
            return Ok(result);
        }

        [HttpGet("{carId}")]
        public async Task<ActionResult<ServiceResponse<Car>>> GetCar(int carId)
        {
            var result = await _carService.GetCarAsync(carId);
            return Ok(result);
        }

        [HttpGet("category/{categoryUrl}")]
        public async Task<ActionResult<ServiceResponse<List<Car>>>> GetCarsByCategory(string categoryUrl)
        {
            var result = await _carService.GetCarsByCategory(categoryUrl);
            return Ok(result);
        }

        [HttpGet("search/{searchText}/{page}")]
        public async Task<ActionResult<ServiceResponse<Search>>> SearchCars(string searchText, int page = 1)
        {
            var result = await _carService.SearchCars(searchText, page);
            return Ok(result);
        }

        [HttpGet("searchsuggestions/{searchText}")]
        public async Task<ActionResult<ServiceResponse<List<Car>>>> GetCarSearchSuggestions(string searchText)
        {
            var result = await _carService.GetCarSearchSuggestions(searchText);
            return Ok(result);
        }

        [HttpGet("featured")]
        public async Task<ActionResult<ServiceResponse<List<Car>>>> GetFeaturedCars()
        {
            var result = await _carService.GetFeaturedCars();
            return Ok(result);
        }
    }
}
