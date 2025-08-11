using FinalProject.BO.Models;
using FinalProject.BL.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinalProject.BL.Interfaces
{
    public interface ILetterOfIntentBL
    {
        Task<IEnumerable<LetterOfIntent>> GetAllAsync();
        Task<LetterOfIntent> GetByIdAsync(int id);
        Task CreateAsync(LetterOfIntentDTO letterOfIntent);
        Task UpdateAsync(int id, LetterOfIntentDTO letterOfIntent);
        Task DeleteAsync(int id);
    }
}