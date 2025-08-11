using FinalProject.BO.Models;
using FinalProject.BL.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinalProject.BL.Interfaces
{
    public interface ISalesPersonBL
    {
        Task<IEnumerable<SalesPerson>> GetAllAsync();
        Task<SalesPerson> GetByIdAsync(int id);
        Task CreateAsync(SalesPersonDTO salesPerson);
        Task UpdateAsync(int id, SalesPersonDTO salesPerson);
        Task DeleteAsync(int id);
    }
}