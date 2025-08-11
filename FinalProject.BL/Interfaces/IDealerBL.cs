using FinalProject.BL.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinalProject.BL.Interfaces
{
    public interface IDealerBL
    {
        Task<IEnumerable<DealerDTO>> GetAllDealers();
        Task<DealerDTO> GetDealerById(int id);
        Task CreateDealer(DealerDTO dealer);
        Task UpdateDealer(DealerDTO dealer);
        Task DeleteDealer(int id);
    }
}