using FinalProject.BL.DTO;
using FinalProject.BL.Interfaces;
using FinalProject.BO.Models;
using FinalProject.DAL.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinalProject.BL.BL
{
    public class LetterOfIntentBL : ILetterOfIntentBL
    {
        private readonly ILetterOfIntent _letterOfIntentDAL;

        public LetterOfIntentBL(ILetterOfIntent letterOfIntentDAL)
        {
            _letterOfIntentDAL = letterOfIntentDAL;
        }

        public async Task CreateAsync(LetterOfIntentDTO letterOfIntent)
        {
            var newLetterOfIntent = new LetterOfIntent
            {
                DealerId = letterOfIntent.DealerId,
                CustomerId = letterOfIntent.CustomerId,
                SalesPersonId = letterOfIntent.SalesPersonId,
                ConsultHistoryId = letterOfIntent.ConsultHistoryId,
                TestDriveId = letterOfIntent.TestDriveId,
                Loidate = letterOfIntent.Loidate,
                PaymentMethod = letterOfIntent.PaymentMethod,
                Note = letterOfIntent.Note
            };
            await _letterOfIntentDAL.CreateAsync(newLetterOfIntent);
        }

        public async Task DeleteAsync(int id)
        {
            await _letterOfIntentDAL.DeleteAsync(id);
        }

        public async Task<IEnumerable<LetterOfIntent>> GetAllAsync()
        {
            return await _letterOfIntentDAL.GetAllAsync();
        }

        public async Task<LetterOfIntent> GetByIdAsync(int id)
        {
            var letterOfIntent = await _letterOfIntentDAL.GetByIdAsync(id);
            if (letterOfIntent == null)
            {
                // Return a new LetterOfIntent or handle as appropriate
                return new LetterOfIntent();
            }
            return letterOfIntent;
        }

        public async Task UpdateAsync(int id, LetterOfIntentDTO letterOfIntent)
        {
            var existingLetterOfIntent = await _letterOfIntentDAL.GetByIdAsync(id);
            if (existingLetterOfIntent != null)
            {
                existingLetterOfIntent.DealerId = letterOfIntent.DealerId;
                existingLetterOfIntent.CustomerId = letterOfIntent.CustomerId;
                existingLetterOfIntent.SalesPersonId = letterOfIntent.SalesPersonId;
                existingLetterOfIntent.ConsultHistoryId = letterOfIntent.ConsultHistoryId;
                existingLetterOfIntent.TestDriveId = letterOfIntent.TestDriveId;
                existingLetterOfIntent.Loidate = letterOfIntent.Loidate;
                existingLetterOfIntent.PaymentMethod = letterOfIntent.PaymentMethod;
                existingLetterOfIntent.Note = letterOfIntent.Note;
                await _letterOfIntentDAL.UpdateAsync(existingLetterOfIntent);
            }
        }
    }
}