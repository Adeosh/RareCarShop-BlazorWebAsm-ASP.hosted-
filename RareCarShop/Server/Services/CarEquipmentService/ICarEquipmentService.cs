using RareCarShop.Shared;
using RareCarShop.Shared.Shop;

namespace RareCarShop.Server.Services.CarEquipmentService
{
    public interface ICarEquipmentService
    {
        Task<ServiceResponse<List<CarEquipment>>> GetCarEquipments();
        Task<ServiceResponse<List<CarEquipment>>> AddCarEquipment(CarEquipment carEquipment);
        Task<ServiceResponse<List<CarEquipment>>> UpdateCarEquipment(CarEquipment carEquipment);
    }
}
