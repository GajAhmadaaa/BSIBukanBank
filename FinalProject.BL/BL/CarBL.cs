using AutoMapper;
using FinalProject.BL.DTO;
using FinalProject.BL.Interfaces;
using FinalProject.BO.Models;
using FinalProject.DAL.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinalProject.BL.BL
{
    public class CarBL : ICarBL
    {
        private readonly ICar _carDAL;
        private readonly IMapper _mapper;

        public CarBL(ICar carDAL, IMapper mapper)
        {
            _carDAL = carDAL;
            _mapper = mapper;
        }

        public async Task<CarViewDTO> CreateCar(CarInsertDTO car)
        {
            var newCar = _mapper.Map<Car>(car);
            await _carDAL.CreateAsync(newCar);
            return _mapper.Map<CarViewDTO>(newCar);
        }

        public async Task DeleteCar(int id)
        {
            await _carDAL.DeleteAsync(id);
        }

        public async Task<IEnumerable<CarViewDTO>> GetAllCars()
        {
            var cars = await _carDAL.GetAllAsync();
            return _mapper.Map<IEnumerable<CarViewDTO>>(cars);
        }

        public async Task<CarViewDTO> GetCarById(int id)
        {
            var car = await _carDAL.GetByIdAsync(id);
            if (car == null)
            {
                return null;
            }
            return _mapper.Map<CarViewDTO>(car);
        }

        public async Task<CarViewDTO> UpdateCar(CarUpdateDTO car)
        {
            var existingCar = await _carDAL.GetByIdAsync(car.CarId);
            if (existingCar != null)
            {
                _mapper.Map(car, existingCar);
                await _carDAL.UpdateAsync(existingCar);
                return _mapper.Map<CarViewDTO>(existingCar);
            }
            return null;
        }
    }
}