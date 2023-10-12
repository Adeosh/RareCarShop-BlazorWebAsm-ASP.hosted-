using RareCarShop.Shared;
using RareCarShop.Shared.Shop;

namespace RareCarShop.Server.Services.CarService
{
    public interface ICarService
    {
        Task<ServiceResponse<List<Car>>> GetCarsAsync();
        Task<ServiceResponse<Car>> GetCarAsync(int carId);
        Task<ServiceResponse<List<Car>>> GetCarsByCategory(string categoryUrl);
        Task<ServiceResponse<Search>> SearchCars(string searchText, int page);
        Task<ServiceResponse<List<string>>> GetCarSearchSuggestions(string searchText);
        Task<ServiceResponse<List<Car>>> GetFeaturedCars();
        Task<ServiceResponse<List<Car>>> GetAdminCars();
        Task<ServiceResponse<Car>> CreateCar(Car car);
        Task<ServiceResponse<Car>> UpdateCar(Car car);
        Task<ServiceResponse<bool>> DeleteCar(int carId);
    }
}
