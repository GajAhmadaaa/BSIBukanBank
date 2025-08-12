using FinalProject.BO.Models;
using FinalProject.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.DAL.DAL
{
    public class SalesAgreementDAL(FinalProjectContext context) : BaseDAL<SalesAgreement>(context), ISalesAgreement
    {
        public async Task AddDetailAsync(int agreementId, SalesAgreementDetail detail)
        {
            detail.SalesAgreementId = agreementId;
            await _context.SalesAgreementDetails.AddAsync(detail);
            await _context.SaveChangesAsync();
        }

        public async Task AddDetailsAsync(int agreementId, IEnumerable<SalesAgreementDetail> details)
        {
            var agreement = await _dbSet.Include(a => a.SalesAgreementDetails)
                                       .FirstOrDefaultAsync(a => a.SalesAgreementId == agreementId);
            if (agreement != null)
            {
                foreach (var detail in details)
                {
                    detail.SalesAgreementId = agreementId;
                    agreement.SalesAgreementDetails.Add(detail);
                }
                await _context.SaveChangesAsync();
            }
        }
        
        // Method untuk mendapatkan semua data dengan detail
        public async Task<IEnumerable<SalesAgreement>> GetAllWithDetailsAsync()
        {
            try
            {
                return await _dbSet
                    .Include(a => a.SalesAgreementDetails)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Gagal mengambil data: {ex.Message}", ex);
            }
        }
        
        // Override GetAllAsync untuk kembali ke perilaku default (tanpa detail)
        public new async Task<IEnumerable<SalesAgreement>> GetAllAsync()
        {
            return await base.GetAllAsync();
        }

        public async Task RemoveDetailAsync(int agreementId, int detailId)
        {
            var detail = await _context.SalesAgreementDetails
                .FirstOrDefaultAsync(d => d.SalesAgreementId == agreementId && d.SalesAgreementDetailId == detailId);
            if (detail != null)
            {
                _context.SalesAgreementDetails.Remove(detail);
                await _context.SaveChangesAsync();
            }
        }

        public async Task RemoveDetailsAsync(int agreementId, IEnumerable<int> detailIds)
        {
            var details = _context.SalesAgreementDetails
                .Where(d => d.SalesAgreementId == agreementId && detailIds.Contains(d.SalesAgreementDetailId));
            _context.SalesAgreementDetails.RemoveRange(details);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateDetailAsync(int agreementId, SalesAgreementDetail detail)
        {
            detail.SalesAgreementId = agreementId;
            _context.SalesAgreementDetails.Update(detail);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateDetailsAsync(int agreementId, IEnumerable<SalesAgreementDetail> details)
        {
            foreach (var detail in details)
            {
                detail.SalesAgreementId = agreementId;
                _context.SalesAgreementDetails.Update(detail);
            }
            await _context.SaveChangesAsync();
        }

        // Metode non-bulk untuk entity non-detail (satu per satu)
        public async Task AddWithDetailsAsync(SalesAgreement agreement, SalesAgreementDetail details)
        {
            await base.CreateAsync(agreement);

            details.SalesAgreementId = agreement.SalesAgreementId;
            await _context.SalesAgreementDetails.AddAsync(details);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateWithDetailsAsync(SalesAgreement agreement, SalesAgreementDetail details)
        {
            await base.UpdateAsync(agreement);
            
            details.SalesAgreementId = agreement.SalesAgreementId;
            _context.SalesAgreementDetails.Update(details);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteWithDetailsAsync(int agreementId)
        {
            var agreement = await _dbSet
                .Include(a => a.SalesAgreementDetails)
                .FirstOrDefaultAsync(a => a.SalesAgreementId == agreementId);
            
            if (agreement != null)
            {
                _context.SalesAgreementDetails.RemoveRange(agreement.SalesAgreementDetails);
                _dbSet.Remove(agreement);
                await _context.SaveChangesAsync();
            }
        }
        
        // Method untuk mendapatkan data berdasarkan ID dengan detail
        public async Task<SalesAgreement?> GetByIdWithDetailsAsync(int id)
        {
            try
            {
                return await _dbSet
                    .Include(a => a.SalesAgreementDetails)
                    .FirstOrDefaultAsync(a => a.SalesAgreementId == id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Gagal mengambil data berdasarkan ID: {ex.Message}", ex);
            }
        }
        
        // Override GetByIdAsync untuk kembali ke perilaku default (tanpa detail)
        public new async Task<SalesAgreement?> GetByIdAsync(int id)
        {
            return await base.GetByIdAsync(id);
        }
    }
}