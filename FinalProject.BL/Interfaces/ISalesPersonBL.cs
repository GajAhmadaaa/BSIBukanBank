using FinalProject.BL.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinalProject.BL.Interfaces
{
    public interface ISalesPersonBL
    {
        Task<IEnumerable<SalesPersonViewDTO>> GetAllAsync();
        Task<SalesPersonViewDTO?> GetByIdAsync(int id);
        Task<SalesPersonViewDTO> CreateAsync(SalesPersonInsertDTO salesPerson);
        Task<SalesPersonViewDTO> UpdateAsync(int id, SalesPersonUpdateDTO salesPerson);
        Task DeleteAsync(int id);
    }
}