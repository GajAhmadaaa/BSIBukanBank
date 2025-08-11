using FinalProject.BO.Models;
using FinalProject.BL.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinalProject.BL.Interfaces
{
    public interface IDealerInventoryBL
    {
        Task<IEnumerable<DealerInventory>> GetAllAsync();
        Task<DealerInventory> GetByIdAsync(int id);
        Task CreateAsync(DealerInventoryDTO dealerInventory);
        Task UpdateAsync(int id, DealerInventoryDTO dealerInventory);
        Task DeleteAsync(int id);
    }
}