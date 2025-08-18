using FinalProject.BL.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinalProject.BL.Interfaces
{
    public interface ILetterOfIntentBL
    {
        Task<IEnumerable<LetterOfIntentViewDTO>> GetAllAsync();
        Task<LetterOfIntentViewDTO?> GetByIdAsync(int id);
        Task<LetterOfIntentViewDTO> CreateAsync(LetterOfIntentInsertDTO letterOfIntent);
        Task<LetterOfIntentViewDTO> CreateWithDetailsAsync(LetterOfIntentWithDetailsInsertDTO letterOfIntentWithDetails);
        Task<LetterOfIntentViewDTO> UpdateAsync(int id, LetterOfIntentUpdateDTO letterOfIntent);
        Task DeleteAsync(int id);
        
        // Method tambahan untuk cart/order
        Task<IEnumerable<LetterOfIntentViewDTO>> GetByCustomerIdAsync(int customerId);
        Task<IEnumerable<LetterOfIntentViewDTO>> GetPendingByCustomerIdAsync(int customerId);
        Task<IEnumerable<LetterOfIntentViewDTO>> GetUnpaidByCustomerIdAsync(int customerId);
        Task<IEnumerable<LetterOfIntentViewDTO>> GetPaidByCustomerIdAsync(int customerId);
    }
}