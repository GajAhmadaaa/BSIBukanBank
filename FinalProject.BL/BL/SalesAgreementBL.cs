using FinalProject.BL.DTO;
using FinalProject.BL.Interfaces;
using FinalProject.BO.Models;
using FinalProject.DAL.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinalProject.BL.BL
{
    public class SalesAgreementBL : ISalesAgreementBL
    {
        private readonly ISalesAgreement _salesAgreementDAL;

        public SalesAgreementBL(ISalesAgreement salesAgreementDAL)
        {
            _salesAgreementDAL = salesAgreementDAL;
        }

        public async Task CreateAsync(SalesAgreementDTO salesAgreement)
        {
            var newSalesAgreement = new SalesAgreement
            {
                DealerId = salesAgreement.DealerId,
                CustomerId = salesAgreement.CustomerId,
                SalesPersonId = salesAgreement.SalesPersonId,
                Loiid = salesAgreement.Loiid,
                TransactionDate = salesAgreement.TransactionDate,
                TotalAmount = salesAgreement.TotalAmount,
                Status = salesAgreement.Status
            };
            await _salesAgreementDAL.CreateAsync(newSalesAgreement);
        }

        public async Task DeleteAsync(int id)
        {
            await _salesAgreementDAL.DeleteAsync(id);
        }

        public async Task<IEnumerable<SalesAgreement>> GetAllAsync()
        {
            return await _salesAgreementDAL.GetAllAsync();
        }

        public async Task<SalesAgreement> GetByIdAsync(int id)
        {
            var salesAgreement = await _salesAgreementDAL.GetByIdAsync(id);
            if (salesAgreement == null)
            {
                // Return a new SalesAgreement or handle as appropriate
                return new SalesAgreement();
            }
            return salesAgreement;
        }

        public async Task UpdateAsync(int id, SalesAgreementDTO salesAgreement)
        {
            var existingSalesAgreement = await _salesAgreementDAL.GetByIdAsync(id);
            if (existingSalesAgreement != null)
            {
                existingSalesAgreement.DealerId = salesAgreement.DealerId;
                existingSalesAgreement.CustomerId = salesAgreement.CustomerId;
                existingSalesAgreement.SalesPersonId = salesAgreement.SalesPersonId;
                existingSalesAgreement.Loiid = salesAgreement.Loiid;
                existingSalesAgreement.TransactionDate = salesAgreement.TransactionDate;
                existingSalesAgreement.TotalAmount = salesAgreement.TotalAmount;
                existingSalesAgreement.Status = salesAgreement.Status;
                await _salesAgreementDAL.UpdateAsync(existingSalesAgreement);
            }
        }
    }
}