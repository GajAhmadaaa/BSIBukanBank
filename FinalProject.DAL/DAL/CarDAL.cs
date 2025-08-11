using FinalProject.BO.Models;
using FinalProject.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace FinalProject.DAL.DAL
{
    public class CarDAL : BaseDAL<Car>, ICar
    {
        public CarDAL(FinalProjectContext context) : base(context)
        {
        }

        // Contoh method khusus Car
        public async Task<IEnumerable<Car>> GetCarsByTypeAsync(string carType)
        {
            return await _dbSet.Where(c => c.CarType == carType).ToListAsync();
        }

        public async Task<IEnumerable<Car>> GetCarsByYearAsync(int year)
        {
            return await _dbSet.Where(c => c.Year == year).ToListAsync();
        }

        public async Task<IEnumerable<Car>> GetAvailableCarsAsync()
        {
            // Asumsi: mobil tersedia jika ada di DealerInventory dengan Stock > 0
            return await _context.DealerInventories
                .Where(di => di.Stock > 0)
                .Select(di => di.Car)
                .Distinct()
                .ToListAsync();
        }
    }
}