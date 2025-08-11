using FinalProject.BL.DTO;
using FinalProject.BL.Interfaces;
using FinalProject.BO.Models;
using FinalProject.DAL.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinalProject.BL.BL
{
    public class DealerInventoryBL : IDealerInventoryBL
    {
        private readonly IDealerInventory _dealerInventoryDAL;

        public DealerInventoryBL(IDealerInventory dealerInventoryDAL)
        {
            _dealerInventoryDAL = dealerInventoryDAL;
        }

        public async Task CreateAsync(DealerInventoryDTO dealerInventory)
        {
            var newDealerInventory = new DealerInventory
            {
                DealerId = dealerInventory.DealerId,
                CarId = dealerInventory.CarId,
                Stock = dealerInventory.Stock,
                Price = dealerInventory.Price,
                DiscountPercent = dealerInventory.DiscountPercent,
                FeePercent = dealerInventory.FeePercent
            };
            await _dealerInventoryDAL.CreateAsync(newDealerInventory);
        }

        public async Task DeleteAsync(int id)
        {
            await _dealerInventoryDAL.DeleteAsync(id);
        }

        public async Task<IEnumerable<DealerInventory>> GetAllAsync()
        {
            return await _dealerInventoryDAL.GetAllAsync();
        }

        public async Task<DealerInventory> GetByIdAsync(int id)
        {
            var dealerInventory = await _dealerInventoryDAL.GetByIdAsync(id);
            if (dealerInventory == null)
            {
                // Return a new DealerInventory or handle as appropriate
                return new DealerInventory();
            }
            return dealerInventory;
        }

        public async Task UpdateAsync(int id, DealerInventoryDTO dealerInventory)
        {
            var existingDealerInventory = await _dealerInventoryDAL.GetByIdAsync(id);
            if (existingDealerInventory != null)
            {
                existingDealerInventory.DealerId = dealerInventory.DealerId;
                existingDealerInventory.CarId = dealerInventory.CarId;
                existingDealerInventory.Stock = dealerInventory.Stock;
                existingDealerInventory.Price = dealerInventory.Price;
                existingDealerInventory.DiscountPercent = dealerInventory.DiscountPercent;
                existingDealerInventory.FeePercent = dealerInventory.FeePercent;
                await _dealerInventoryDAL.UpdateAsync(existingDealerInventory);
            }
        }
    }
}