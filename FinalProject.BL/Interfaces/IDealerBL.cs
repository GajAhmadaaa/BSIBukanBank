using FinalProject.BL.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinalProject.BL.Interfaces
{
    public interface IDealerBL
    {
        Task<IEnumerable<DealerViewDTO>> GetAllDealers();
        Task<DealerViewDTO> GetDealerById(int id);
        Task<DealerViewDTO> CreateDealer(DealerInsertDTO dealer);
        Task<DealerViewDTO> UpdateDealer(DealerUpdateDTO dealer);
        Task DeleteDealer(int id);
    }
}