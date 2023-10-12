using RareCarShop.Shared;
using RareCarShop.Shared.Shop;
using System.Net.Http.Json;

namespace RareCarShop.Client.Services.CarEquipmentService
{
    public class CarEquipmentService : ICarEquipmentService
    {
        private readonly HttpClient _httpClient;

        public CarEquipmentService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public List<CarEquipment> CarsEquipment { get; set; } = new List<CarEquipment>();

        public event Action OnChange;

        public async Task AddCarEquipment(CarEquipment carEquipment)
        {
            var response = await _httpClient.PostAsJsonAsync("api/carequipment", carEquipment);
            CarsEquipment = (await response.Content
                .ReadFromJsonAsync<ServiceResponse<List<CarEquipment>>>()).Data;
            OnChange.Invoke();
        }

        public CarEquipment CreateNewCarEquipment()
        {
            var newCarEquipment = new CarEquipment { IsNew = true, Editing = true };

            CarsEquipment.Add(newCarEquipment);
            OnChange.Invoke();
            return newCarEquipment;
        }

        public async Task GetCarEquipments()
        {
            var result = await _httpClient
                .GetFromJsonAsync<ServiceResponse<List<CarEquipment>>>("api/carequipment");
            CarsEquipment = result.Data;
        }

        public async Task UpdateCarEquipment(CarEquipment carEquipment)
        {
            var response = await _httpClient.PutAsJsonAsync("api/carequipment", carEquipment);
            CarsEquipment = (await response.Content
                .ReadFromJsonAsync<ServiceResponse<List<CarEquipment>>>()).Data;
            OnChange.Invoke();
        }
    }
}
