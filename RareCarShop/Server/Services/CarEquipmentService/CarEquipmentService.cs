using Microsoft.EntityFrameworkCore;
using RareCarShop.Server.Data;
using RareCarShop.Shared;
using RareCarShop.Shared.Shop;

namespace RareCarShop.Server.Services.CarEquipmentService
{
    public class CarEquipmentService : ICarEquipmentService
    {
        private readonly DataContext _context;

        public CarEquipmentService(DataContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse<List<CarEquipment>>> AddCarEquipment(CarEquipment carEquipment)
        {
            carEquipment.Editing = carEquipment.IsNew = false;
            _context.CarsEquipment.Add(carEquipment);
            await _context.SaveChangesAsync();

            return await GetCarEquipments();
        }

        public async Task<ServiceResponse<List<CarEquipment>>> GetCarEquipments()
        {
            var carEquipments = await _context.CarsEquipment.ToListAsync();
            return new ServiceResponse<List<CarEquipment>> { Data = carEquipments };
        }

        public async Task<ServiceResponse<List<CarEquipment>>> UpdateCarEquipment(CarEquipment carEquipment)
        {
            var uCarEquipment = await _context.CarsEquipment.FindAsync(carEquipment.Id);
            if (uCarEquipment == null)
            {
                return new ServiceResponse<List<CarEquipment>>
                {
                    Success = false,
                    Message = "Автомобильная комплектация не найдена"
                };
            }

            uCarEquipment.Name = carEquipment.Name;
            await _context.SaveChangesAsync();

            return await GetCarEquipments();
        }
    }
}
