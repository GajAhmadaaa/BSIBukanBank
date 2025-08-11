using FinalProject.BL.DTO;
using FinalProject.BL.Interfaces;
using FinalProject.BO.Models;
using FinalProject.DAL.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.BL.BL
{
    public class CarBL : ICarBL
    {
        private readonly ICar _carDAL;

        public CarBL(ICar carDAL)
        {
            _carDAL = carDAL;
        }

        public async Task CreateCar(CarDTO car)
        {
            var newCar = new Car
            {
                Model = car.Model,
                CarType = car.CarType,
                BasePrice = car.BasePrice,
                Year = car.Year,
                Color = car.Color
            };
            await _carDAL.CreateAsync(newCar);
        }

        public async Task DeleteCar(int id)
        {
            await _carDAL.DeleteAsync(id);
        }

        public async Task<IEnumerable<CarDTO>> GetAllCars()
        {
            var cars = await _carDAL.GetAllAsync();
            return cars.Select(c => new CarDTO
            {
                CarId = c.CarId,
                Model = c.Model,
                CarType = c.CarType,
                BasePrice = c.BasePrice,
                Year = c.Year,
                Color = c.Color
            });
        }

        public async Task<CarDTO> GetCarById(int id)
        {
            var car = await _carDAL.GetByIdAsync(id);
            if (car == null)
            {
                return null;
            }
            return new CarDTO
            {
                CarId = car.CarId,
                Model = car.Model,
                CarType = car.CarType,
                BasePrice = car.BasePrice,
                Year = car.Year,
                Color = car.Color
            };
        }

        public async Task UpdateCar(CarDTO car)
        {
            var existingCar = await _carDAL.GetByIdAsync(car.CarId);
            if (existingCar != null)
            {
                existingCar.Model = car.Model;
                existingCar.CarType = car.CarType;
                existingCar.BasePrice = car.BasePrice;
                existingCar.Year = car.Year;
                existingCar.Color = car.Color;
                await _carDAL.UpdateAsync(existingCar);
            }
        }
    }
}