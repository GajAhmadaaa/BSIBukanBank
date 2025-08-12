using FinalProject.BL.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinalProject.BL.Interfaces
{
    public interface ISalesAgreementBL
    {
        Task<IEnumerable<SalesAgreementViewDTO>> GetAllAsync();
        Task<SalesAgreementViewDTO?> GetByIdAsync(int id);
        Task<SalesAgreementViewDTO> CreateAsync(SalesAgreementInsertDTO salesAgreement);
        Task<SalesAgreementViewDTO> CreateWithDetailsAsync(SalesAgreementWithDetailsInsertDTO salesAgreementWithDetails);
        Task<SalesAgreementViewDTO> UpdateAsync(int id, SalesAgreementUpdateDTO salesAgreement);
        Task DeleteAsync(int id);
    }
}