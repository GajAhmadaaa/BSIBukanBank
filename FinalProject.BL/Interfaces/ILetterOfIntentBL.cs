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
        Task<LetterOfIntentViewDTO> UpdateAsync(int id, LetterOfIntentUpdateDTO letterOfIntent);
        Task DeleteAsync(int id);
    }
}