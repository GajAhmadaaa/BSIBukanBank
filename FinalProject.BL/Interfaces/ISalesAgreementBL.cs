using FinalProject.BO.Models;
using FinalProject.BL.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinalProject.BL.Interfaces
{
    public interface ISalesAgreementBL
    {
        Task<IEnumerable<SalesAgreement>> GetAllAsync();
        Task<SalesAgreement> GetByIdAsync(int id);
        Task CreateAsync(SalesAgreementDTO salesAgreement);
        Task UpdateAsync(int id, SalesAgreementDTO salesAgreement);
        Task DeleteAsync(int id);
    }
}