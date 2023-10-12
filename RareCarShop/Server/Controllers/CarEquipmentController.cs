using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RareCarShop.Server.Services.CarEquipmentService;
using RareCarShop.Shared;
using RareCarShop.Shared.Shop;

namespace RareCarShop.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class CarEquipmentController : ControllerBase
    {
        private readonly ICarEquipmentService _carEquipmentService;

        public CarEquipmentController(ICarEquipmentService carEquipmentService)
        {
            _carEquipmentService = carEquipmentService;
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<CarEquipment>>>> CarEquipments()
        {
            var response = await _carEquipmentService.GetCarEquipments();
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<List<CarEquipment>>>> AddCarEquipment(CarEquipment carEquipment)
        {
            var response = await _carEquipmentService.AddCarEquipment(carEquipment);
            return Ok(response);
        }

        [HttpPut]
        public async Task<ActionResult<ServiceResponse<List<CarEquipment>>>> UpdateCarEquipment(CarEquipment carEquipment)
        {
            var response = await _carEquipmentService.UpdateCarEquipment(carEquipment);
            return Ok(response);
        }
    }
}
