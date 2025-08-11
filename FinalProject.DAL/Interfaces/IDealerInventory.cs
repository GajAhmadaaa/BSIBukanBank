using FinalProject.BO.Models;

namespace FinalProject.DAL.Interfaces
{
    public interface IDealerInventory : ICrud<DealerInventory>
    {
        Task<IEnumerable<DealerInventory>> GetInventoryByDealerAsync(int dealerId);
        Task<IEnumerable<DealerInventory>> GetAvailableInventoryAsync();
        Task<DealerInventory?> GetInventoryByDealerAndCarAsync(int dealerId, int carId);
    }
}