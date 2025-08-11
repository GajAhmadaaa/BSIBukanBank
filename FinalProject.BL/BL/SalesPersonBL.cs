using FinalProject.BL.DTO;
using FinalProject.BL.Interfaces;
using FinalProject.BO.Models;
using FinalProject.DAL.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinalProject.BL.BL
{
    public class SalesPersonBL : ISalesPersonBL
    {
        private readonly ISalesPerson _salesPersonDAL;

        public SalesPersonBL(ISalesPerson salesPersonDAL)
        {
            _salesPersonDAL = salesPersonDAL;
        }

        public async Task CreateAsync(SalesPersonDTO salesPerson)
        {
            var newSalesPerson = new SalesPerson
            {
                DealerId = salesPerson.DealerId,
                Name = salesPerson.Name
            };
            await _salesPersonDAL.CreateAsync(newSalesPerson);
        }

        public async Task DeleteAsync(int id)
        {
            await _salesPersonDAL.DeleteAsync(id);
        }

        public async Task<IEnumerable<SalesPerson>> GetAllAsync()
        {
            return await _salesPersonDAL.GetAllAsync();
        }

        public async Task<SalesPerson> GetByIdAsync(int id)
        {
            var salesPerson = await _salesPersonDAL.GetByIdAsync(id);
            if (salesPerson == null)
            {
                // Return a new SalesPerson or handle as appropriate
                return new SalesPerson();
            }
            return salesPerson;
        }

        public async Task UpdateAsync(int id, SalesPersonDTO salesPerson)
        {
            var existingSalesPerson = await _salesPersonDAL.GetByIdAsync(id);
            if (existingSalesPerson != null)
            {
                existingSalesPerson.DealerId = salesPerson.DealerId;
                existingSalesPerson.Name = salesPerson.Name;
                await _salesPersonDAL.UpdateAsync(existingSalesPerson);
            }
        }
    }
}