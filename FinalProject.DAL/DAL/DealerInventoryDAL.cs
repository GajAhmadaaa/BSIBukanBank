using FinalProject.BO.Models;
using FinalProject.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace FinalProject.DAL.DAL
{
    public class DealerInventoryDAL : BaseDAL<DealerInventory>, IDealerInventory
    {
        public DealerInventoryDAL(FinalProjectContext context) : base(context)
        {
        }

        public async Task<IEnumerable<DealerInventory>> GetAvailableInventoryAsync()
        {
            return await _dbSet.Where(di => di.Stock > 0).ToListAsync();
        }

        public async Task<IEnumerable<DealerInventory>> GetInventoryByDealerAsync(int dealerId)
        {
            return await _dbSet.Where(di => di.DealerId == dealerId).ToListAsync();
        }

        public async Task<DealerInventory?> GetInventoryByDealerAndCarAsync(int dealerId, int carId)
        {
            return await _dbSet.FirstOrDefaultAsync(di => di.DealerId == dealerId && di.CarId == carId);
        }

    }
}