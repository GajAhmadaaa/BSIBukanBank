using FinalProject.BO.Models;

namespace FinalProject.DAL.Interfaces
{
    public interface ILetterOfIntent : ICrud<LetterOfIntent>
    {

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
    }
}