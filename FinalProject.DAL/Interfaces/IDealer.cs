using System.Collections.Generic;
using System.Threading.Tasks;
using FinalProject.BO.Models;

namespace FinalProject.DAL.Interfaces
{
    public interface IDealer : ICrud<Dealer>
    {
        Task<Dealer?> GetDealerByIdAsync(int dealerId);
        Task<IEnumerable<Dealer>> GetDealersByCityAsync(string city);
        Task<Dealer?> GetDealerWithInventoryAsync(int dealerId);
        Task<IEnumerable<Dealer>> GetAllDealersAsync();
    }
}