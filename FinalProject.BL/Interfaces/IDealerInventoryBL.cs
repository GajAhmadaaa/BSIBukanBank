using FinalProject.BL.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinalProject.BL.Interfaces
{
    public interface IDealerInventoryBL
    {
        Task<IEnumerable<DealerInventoryViewDTO>> GetAllAsync();
        Task<DealerInventoryViewDTO?> GetByIdAsync(int id);
        Task<DealerInventoryViewDTO> CreateAsync(DealerInventoryInsertDTO dealerInventory);
        Task<DealerInventoryViewDTO> UpdateAsync(int id, DealerInventoryUpdateDTO dealerInventory);
        Task DeleteAsync(int id);
        
        // Method tambahan untuk mendapatkan harga dealer
        Task<DealerInventoryViewDTO?> GetByDealerAndCarAsync(int dealerId, int carId);
    }
}