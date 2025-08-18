using FinalProject.BO.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinalProject.DAL.Interfaces
{
    public interface ILetterOfIntent : ICrud<LetterOfIntent>
    {
        // Method untuk memuat detail
        Task<IEnumerable<LetterOfIntent>> GetAllWithDetailsAsync();
        Task<LetterOfIntent?> GetByIdWithDetailsAsync(int id);

        // Operasi satuan untuk detail LOI
        Task AddDetailAsync(int loiId, LetterOfIntentDetail detail);
        Task UpdateDetailAsync(int loiId, LetterOfIntentDetail detail);
        Task RemoveDetailAsync(int loiId, int detailId);
        
        // Bulk operation untuk detail LOI (bulk order)
        Task AddDetailsAsync(int loiId, IEnumerable<LetterOfIntentDetail> details);
        Task UpdateDetailsAsync(int loiId, IEnumerable<LetterOfIntentDetail> details);
        Task RemoveDetailsAsync(int loiId, IEnumerable<int> detailIds);

        // Metode non-bulk untuk entity non-detail (satu per satu)
        Task AddWithDetailsAsync(LetterOfIntent loi, LetterOfIntentDetail details);
        Task UpdateWithDetailsAsync(LetterOfIntent loi, LetterOfIntentDetail details);
        Task DeleteWithDetailsAsync(int loiId);
        
        // Method untuk cart/order berdasarkan customer
        Task<IEnumerable<LetterOfIntent>> GetByCustomerIdAsync(int customerId);
        Task<IEnumerable<LetterOfIntent>> GetPendingByCustomerIdAsync(int customerId);
        Task<IEnumerable<LetterOfIntent>> GetUnpaidByCustomerIdAsync(int customerId);
        Task<IEnumerable<LetterOfIntent>> GetPaidByCustomerIdAsync(int customerId);
    }
}