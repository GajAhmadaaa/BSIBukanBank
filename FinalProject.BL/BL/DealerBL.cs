using FinalProject.BL.DTO;
using FinalProject.BL.Interfaces;
using FinalProject.BO.Models;
using FinalProject.DAL.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.BL.BL
{
    public class DealerBL : IDealerBL
    {
        private readonly IDealer _dealerDAL;

        public DealerBL(IDealer dealerDAL)
        {
            _dealerDAL = dealerDAL;
        }

        public async Task CreateDealer(DealerDTO dealer)
        {
            var newDealer = new Dealer
            {
                Name = dealer.Name,
                City = dealer.City,
                Address = dealer.Address,
                PhoneNumber = dealer.PhoneNumber
            };
            await _dealerDAL.CreateAsync(newDealer);
        }

        public async Task DeleteDealer(int id)
        {
            await _dealerDAL.DeleteAsync(id);
        }

        public async Task<IEnumerable<DealerDTO>> GetAllDealers()
        {
            var dealers = await _dealerDAL.GetAllAsync();
            return dealers.Select(d => new DealerDTO
            {
                DealerId = d.DealerId,
                Name = d.Name,
                City = d.City,
                Address = d.Address,
                PhoneNumber = d.PhoneNumber
            });
        }

        public async Task<DealerDTO> GetDealerById(int id)
        {
            var dealer = await _dealerDAL.GetByIdAsync(id);
            if (dealer == null)
            {
                return null;
            }
            return new DealerDTO
            {
                DealerId = dealer.DealerId,
                Name = dealer.Name,
                City = dealer.City,
                Address = dealer.Address,
                PhoneNumber = dealer.PhoneNumber
            };
        }

        public async Task UpdateDealer(DealerDTO dealer)
        {
            var existingDealer = await _dealerDAL.GetByIdAsync(dealer.DealerId);
            if (existingDealer != null)
            {
                existingDealer.Name = dealer.Name;
                existingDealer.City = dealer.City;
                existingDealer.Address = dealer.Address;
                existingDealer.PhoneNumber = dealer.PhoneNumber;
                await _dealerDAL.UpdateAsync(existingDealer);
            }
        }
    }
}