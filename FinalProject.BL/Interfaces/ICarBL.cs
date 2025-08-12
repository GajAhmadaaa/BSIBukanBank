using FinalProject.BL.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinalProject.BL.Interfaces
{
    public interface ICarBL
    {
        Task<IEnumerable<CarViewDTO>> GetAllCars();
        Task<CarViewDTO> GetCarById(int id);
        Task<CarViewDTO> CreateCar(CarInsertDTO car);
        Task<CarViewDTO> UpdateCar(CarUpdateDTO car);
        Task DeleteCar(int id);
        Task<IEnumerable<CarViewDTO>> GetCarsBySearch(string search);
    }
}