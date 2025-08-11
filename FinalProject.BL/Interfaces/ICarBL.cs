using FinalProject.BL.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinalProject.BL.Interfaces
{
    public interface ICarBL
    {
        Task<IEnumerable<CarDTO>> GetAllCars();
        Task<CarDTO> GetCarById(int id);
        Task CreateCar(CarDTO car);
        Task UpdateCar(CarDTO car);
        Task DeleteCar(int id);
    }
}