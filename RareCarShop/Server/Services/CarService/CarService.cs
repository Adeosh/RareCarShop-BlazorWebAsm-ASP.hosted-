using Microsoft.EntityFrameworkCore;
using RareCarShop.Server.Data;
using RareCarShop.Shared;
using RareCarShop.Shared.Shop;

namespace RareCarShop.Server.Services.CarService
{
    public class CarService : ICarService
    {
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CarService(DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        private async Task<List<Car>> FindCarsBySearchText(string searchText)
        {
            return await _context.Cars
                                .Where(c => c.Brand.ToLower().Contains(searchText.ToLower()) ||
                                    c.Description.ToLower().Contains(searchText.ToLower()) &&
                                    c.Visible && !c.Deleted)
                                .Include(v => v.Variants)
                                .ToListAsync();
        }

        public async Task<ServiceResponse<Car>> CreateCar(Car car)
        {
            foreach (var variant in car.Variants)
            {
                variant.CarEquipment = null;
            }
            _context.Cars.Add(car);
            await _context.SaveChangesAsync();
            return new ServiceResponse<Car> { Data = car };
        }

        public async Task<ServiceResponse<bool>> DeleteCar(int carId)
        {
            var dbCar = await _context.Cars.FindAsync(carId);
            if (dbCar == null)
            {
                return new ServiceResponse<bool>
                {
                    Success = false,
                    Data = false,
                    Message = "Машина не найдена."
                };
            }

            dbCar.Deleted = true;

            await _context.SaveChangesAsync();
            return new ServiceResponse<bool> { Data = true };
        }

        public async Task<ServiceResponse<List<Car>>> GetAdminCars()
        {
            var response = new ServiceResponse<List<Car>>
            {
                Data = await _context.Cars
                    .Where(c => !c.Deleted)
                    .Include(c => c.Variants.Where(v => !v.Deleted))
                    .ThenInclude(v => v.CarEquipment)
                    .Include(c => c.Images)
                    .ToListAsync()
            };

            return response;
        }

        public async Task<ServiceResponse<Car>> GetCarAsync(int carId)
        {
            var response = new ServiceResponse<Car>();
            Car car = null;

            if (_httpContextAccessor.HttpContext.User.IsInRole("Admin"))
            {
                car = await _context.Cars
                    .Include(c => c.Variants.Where(v => !v.Deleted))
                    .ThenInclude(v => v.CarEquipment)
                    .Include(c => c.Images)
                    .FirstOrDefaultAsync(c => c.Id == carId && !c.Deleted);
            }
            else
            {
                car = await _context.Cars
                    .Include(c => c.Variants.Where(v => v.Visible && !v.Deleted))
                    .ThenInclude(v => v.CarEquipment)
                    .Include(c => c.Images)
                    .FirstOrDefaultAsync(c => c.Id == carId && !c.Deleted && c.Visible);
            }

            if (car == null)
            {
                response.Success = false;
                response.Message = "Извините, но этой машины не существует.";
            }
            else
            {
                response.Data = car;
            }

            return response;
        }

        public async Task<ServiceResponse<List<Car>>> GetCarsAsync()
        {
            var response = new ServiceResponse<List<Car>>
            {
                Data = await _context.Cars
                    .Where(c => c.Visible && !c.Deleted)
                    .Include(c => c.Variants.Where(v => v.Visible && !v.Deleted))
                    .Include(c => c.Images)
                    .ToListAsync()
            };

            return response;
        }

        public async Task<ServiceResponse<List<Car>>> GetCarsByCategory(string categoryUrl)
        {
            var response = new ServiceResponse<List<Car>>
            {
                Data = await _context.Cars
                    .Where(c => c.Category.Url.ToLower().Equals(categoryUrl.ToLower()) &&
                        c.Visible && !c.Deleted)
                    .Include(c => c.Variants.Where(v => v.Visible && !v.Deleted))
                    .Include(c => c.Images)
                    .ToListAsync()
            };

            return response;
        }

        public async Task<ServiceResponse<List<string>>> GetCarSearchSuggestions(string searchText)
        {
            var cars = await FindCarsBySearchText(searchText);

            List<string> result = new List<string>();

            foreach (var car in cars)
            {
                if (car.Brand.Contains(searchText, StringComparison.OrdinalIgnoreCase))
                {
                    result.Add(car.Brand);
                }

                if (car.Description != null)
                {
                    var punctuation = car.Description.Where(char.IsPunctuation)
                        .Distinct().ToArray();
                    var words = car.Description.Split()
                        .Select(s => s.Trim(punctuation));

                    foreach (var word in words)
                    {
                        if (word.Contains(searchText, StringComparison.OrdinalIgnoreCase)
                            && !result.Contains(word))
                        {
                            result.Add(word);
                        }
                    }
                }
            }

            return new ServiceResponse<List<string>> { Data = result };
        }

        public async Task<ServiceResponse<List<Car>>> GetFeaturedCars()
        {
            var response = new ServiceResponse<List<Car>>
            {
                Data = await _context.Cars
                    .Where(c => c.Featured && c.Visible && !c.Deleted)
                    .Include(c => c.Variants.Where(v => v.Visible && !v.Deleted))
                    .Include(c => c.Images)
                    .ToListAsync()
            };

            return response;
        }

        public async Task<ServiceResponse<Search>> SearchCars(string searchText, int page)
        {
            var pageResults = 2f;
            var pageCount = Math.Ceiling((await FindCarsBySearchText(searchText)).Count / pageResults);
            var cars = await _context.Cars
                                .Where(c => c.Brand.ToLower().Contains(searchText.ToLower()) ||
                                    c.Description.ToLower().Contains(searchText.ToLower()) &&
                                    c.Visible && !c.Deleted)
                                .Include(c => c.Variants)
                                .Include(c => c.Images)
                                .Skip((page - 1) * (int)pageResults)
                                .Take((int)pageResults)
                                .ToListAsync();

            var response = new ServiceResponse<Search>
            {
                Data = new Search
                {
                    Cars = cars,
                    CurrentPage = page,
                    Pages = (int)pageCount
                }
            };

            return response;
        }

        public async Task<ServiceResponse<Car>> UpdateCar(Car car)
        {
            var uCar = await _context.Cars
                .Include(c => c.Images)
                .FirstOrDefaultAsync(c => c.Id == car.Id);

            if (uCar == null)
            {
                return new ServiceResponse<Car>
                {
                    Success = false,
                    Message = "Машина не найдена."
                };
            }

            uCar.Brand = car.Brand;
            uCar.Description = car.Description;
            uCar.BodyType = car.BodyType;
            uCar.Drivetrain = car.Drivetrain;
            uCar.EngineCapacity = car.EngineCapacity;
            uCar.MaxSpeed = car.MaxSpeed;
            uCar.ImageUrl1 = car.ImageUrl1;
            uCar.ImageUrl2 = car.ImageUrl2;
            uCar.ImageUrl3 = car.ImageUrl3;
            uCar.CategoryId = car.CategoryId;
            uCar.Visible = car.Visible;
            uCar.Featured = car.Featured;

            var carImages = uCar.Images;
            _context.Images.RemoveRange(carImages);

            uCar.Images = car.Images;

            foreach (var variant in car.Variants)
            {
                var dbVariant = await _context.PriceVariants
                    .SingleOrDefaultAsync(v => v.CarId == variant.CarId &&
                        v.CarEquipmentId == variant.CarEquipmentId);
                if (dbVariant == null)
                {
                    variant.CarEquipment = null;
                    _context.PriceVariants.Add(variant);
                }
                else
                {
                    dbVariant.CarEquipmentId = variant.CarEquipmentId;
                    dbVariant.Price = variant.Price;
                    dbVariant.OriginalPrice = variant.OriginalPrice;
                    dbVariant.Visible = variant.Visible;
                    dbVariant.Deleted = variant.Deleted;
                }
            }

            await _context.SaveChangesAsync();
            return new ServiceResponse<Car> { Data = car };
        }
    }
}
