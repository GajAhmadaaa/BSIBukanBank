using FinalProject.BO.Models;
using FinalProject.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace FinalProject.DAL.DAL
{
    public class LetterOfIntentDAL : BaseDAL<LetterOfIntent>, ILetterOfIntent
    {
        public LetterOfIntentDAL(FinalProjectContext context) : base(context)
        {
        }

        // Bulk operation untuk detail LOI
        public async Task AddDetailsAsync(int loiId, IEnumerable<LetterOfIntentDetail> details)
        {
            var loi = await _dbSet.Include(l => l.LetterOfIntentDetails)
                                  .FirstOrDefaultAsync(l => l.Loiid == loiId);
            if (loi != null)
            {
                foreach (var detail in details)
                {
                    loi.LetterOfIntentDetails.Add(detail);
                }
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateDetailsAsync(int loiId, IEnumerable<LetterOfIntentDetail> details)
        {
            foreach (var detail in details)
            {
                _context.LetterOfIntentDetails.Update(detail);
            }
            await _context.SaveChangesAsync();
        }

        public async Task RemoveDetailsAsync(int loiId, IEnumerable<int> detailIds)
        {
            var details = _context.LetterOfIntentDetails
                .Where(d => d.Loiid == loiId && detailIds.Contains(d.LoidetailId));
            _context.LetterOfIntentDetails.RemoveRange(details);
            await _context.SaveChangesAsync();
        }

        // Operasi satuan untuk detail LOI
        public async Task AddDetailAsync(int loiId, LetterOfIntentDetail detail)
        {
            detail.Loiid = loiId;
            await _context.LetterOfIntentDetails.AddAsync(detail);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateDetailAsync(int loiId, LetterOfIntentDetail detail)
        {
            detail.Loiid = loiId;
            _context.LetterOfIntentDetails.Update(detail);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveDetailAsync(int loiId, int detailId)
        {
            var detail = await _context.LetterOfIntentDetails
                .FirstOrDefaultAsync(d => d.Loiid == loiId && d.LoidetailId == detailId);
            if (detail != null)
            {
                _context.LetterOfIntentDetails.Remove(detail);
                await _context.SaveChangesAsync();
            }
        }

        // Metode non-bulk untuk entity non-detail (satu per satu)
        public async Task AddWithDetailsAsync(LetterOfIntent loi, LetterOfIntentDetail details)
        {
            await base.CreateAsync(loi);

            details.Loiid = loi.Loiid;
            await _context.LetterOfIntentDetails.AddAsync(details);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateWithDetailsAsync(LetterOfIntent loi, LetterOfIntentDetail details)
        {
            await base.UpdateAsync(loi);
            
            details.Loiid = loi.Loiid;
            _context.LetterOfIntentDetails.Update(details);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteWithDetailsAsync(int loiId)
        {
            var loi = await _dbSet
                .Include(l => l.LetterOfIntentDetails)
                .FirstOrDefaultAsync(l => l.Loiid == loiId);
            
            if (loi != null)
            {
                _context.LetterOfIntentDetails.RemoveRange(loi.LetterOfIntentDetails);
                _dbSet.Remove(loi);
                await _context.SaveChangesAsync();
            }
        }
        
        // Method untuk mendapatkan semua data dengan detail
        public async Task<IEnumerable<LetterOfIntent>> GetAllWithDetailsAsync()
        {
            try
            {
                return await _dbSet
                    .Include(l => l.LetterOfIntentDetails)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Gagal mengambil data: {ex.Message}", ex);
            }
        }
        
        // Override GetAllAsync untuk kembali ke perilaku default (tanpa detail)
        public new async Task<IEnumerable<LetterOfIntent>> GetAllAsync()
        {
            return await base.GetAllAsync();
        }
        
        // Method untuk mendapatkan data berdasarkan ID dengan detail
        public async Task<LetterOfIntent?> GetByIdWithDetailsAsync(int id)
        {
            try
            {
                return await _dbSet
                    .Include(l => l.LetterOfIntentDetails)
                    .FirstOrDefaultAsync(l => l.Loiid == id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Gagal mengambil data berdasarkan ID: {ex.Message}", ex);
            }
        }
        
        // Override GetByIdAsync untuk kembali ke perilaku default (tanpa detail)
        public new async Task<LetterOfIntent?> GetByIdAsync(int id)
        {
            return await base.GetByIdAsync(id);
        }
    }
}