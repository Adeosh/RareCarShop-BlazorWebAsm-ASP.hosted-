using RareCarShop.Shared.Shop;

namespace RareCarShop.Client.Services.CarEquipmentService
{
    public interface ICarEquipmentService
    {
        event Action OnChange;
        public List<CarEquipment> CarsEquipment { get; set; }
        Task GetCarEquipments();
        Task AddCarEquipment(CarEquipment carEquipment);
        Task UpdateCarEquipment(CarEquipment carEquipment);
        CarEquipment CreateNewCarEquipment();
    }
}
