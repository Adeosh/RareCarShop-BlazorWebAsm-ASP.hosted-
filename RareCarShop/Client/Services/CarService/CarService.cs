using RareCarShop.Shared;
using RareCarShop.Shared.Shop;
using System.Net.Http.Json;

namespace RareCarShop.Client.Services.CarService
{
    public class CarService : ICarService
    {
        private readonly HttpClient _httpClient;

        public CarService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public List<Car> Cars { get; set; } = new List<Car>();
        public List<Car> AdminCars { get; set; }
        public string Message { get; set; } = "...";
        public int CurrentPage { get; set; } = 1;
        public int PageCount { get; set; } = 0;
        public string LastSearchText { get; set; } = string.Empty;

        public event Action CarsChanged;

        public async Task<Car> CreateCar(Car car)
        {
            var result = await _httpClient.PostAsJsonAsync("api/car", car);
            var newCar = (await result.Content
                .ReadFromJsonAsync<ServiceResponse<Car>>()).Data;
            return newCar;
        }

        public async Task DeleteCar(Car car)
        {
            var result = await _httpClient.DeleteAsync($"api/car/{car.Id}");
        }

        public async Task GetAdminCars()
        {
            var result = await _httpClient
                .GetFromJsonAsync<ServiceResponse<List<Car>>>("api/car/admin");
            AdminCars = result.Data;
            CurrentPage = 1;
            PageCount = 0;
            if (AdminCars.Count == 0)
                Message = "Продукты не найдены.";
        }

        public async Task<ServiceResponse<Car>> GetCar(int carId)
        {
            var result = await _httpClient.GetFromJsonAsync<ServiceResponse<Car>>($"api/car/{carId}");
            return result;
        }

        public async Task GetCars(string? categoryUrl = null)
        {
            var result = categoryUrl == null ?
                await _httpClient.GetFromJsonAsync<ServiceResponse<List<Car>>>("api/car/featured") :
                await _httpClient.GetFromJsonAsync<ServiceResponse<List<Car>>>($"api/car/category/{categoryUrl}");
            if (result != null && result.Data != null)
                Cars = result.Data;

            CurrentPage = 1;
            PageCount = 0;

            if (Cars.Count == 0)
                Message = "Продукты не найдены";

            CarsChanged.Invoke();
        }

        public async Task<List<string>> GetCarSearchSuggestions(string searchText)
        {
            var result = await _httpClient
                .GetFromJsonAsync<ServiceResponse<List<string>>>($"api/car/searchsuggestions/{searchText}");
            return result.Data;
        }

        public async Task SearchCar(string searchText, int page)
        {
            LastSearchText = searchText;
            var result = await _httpClient
                 .GetFromJsonAsync<ServiceResponse<Search>>($"api/car/search/{searchText}/{page}");
            if (result != null && result.Data != null)
            {
                Cars = result.Data.Cars;
                CurrentPage = result.Data.CurrentPage;
                PageCount = result.Data.Pages;
            }
            if (Cars.Count == 0) Message = "Продукты не найдены.";
            CarsChanged?.Invoke();
        }

        public async Task<Car> UpdateCar(Car car)
        {
            var result = await _httpClient.PutAsJsonAsync($"api/car", car);
            var content = await result.Content.ReadFromJsonAsync<ServiceResponse<Car>>();
            return content.Data;
        }
    }
}
