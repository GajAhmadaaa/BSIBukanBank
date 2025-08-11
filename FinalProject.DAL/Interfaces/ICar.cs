using FinalProject.BO.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinalProject.DAL.Interfaces
{
    public interface ICar : ICrud<Car>
    {
        Task<IEnumerable<Car>> GetCarsByTypeAsync(string carType);
        Task<IEnumerable<Car>> GetCarsByYearAsync(int year);
        Task<IEnumerable<Car>> GetAvailableCarsAsync();
    }
}