using FinalProject.BO.Models;
using FinalProject.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace FinalProject.DAL.DAL
{
    public class DealerDAL : BaseDAL<Dealer>, IDealer
    {
        public DealerDAL(FinalProjectContext context) : base(context)
        {
        }

        //implementasi crud
        public async Task<Dealer?> GetDealerByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<Dealer>> GetAllDealersAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<IEnumerable<Dealer>> GetDealersByCityAsync(string city)
        {
            return await _dbSet.Where(d => d.City == city).ToListAsync();
        }

        public async Task<Dealer?> GetDealerWithInventoryAsync(int dealerId)
        {
            return await _dbSet
                .Include(d => d.DealerInventories)
                .FirstOrDefaultAsync(d => d.DealerId == dealerId);
        }
    }
}