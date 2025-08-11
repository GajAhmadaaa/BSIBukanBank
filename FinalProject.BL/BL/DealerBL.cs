using AutoMapper;
using FinalProject.BL.DTO;
using FinalProject.BL.Interfaces;
using FinalProject.BO.Models;
using FinalProject.DAL.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinalProject.BL.BL
{
    public class DealerBL : IDealerBL
    {
        private readonly IDealer _dealerDAL;
        private readonly IMapper _mapper;

        public DealerBL(IDealer dealerDAL, IMapper mapper)
        {
            _dealerDAL = dealerDAL;
            _mapper = mapper;
        }

        public async Task<DealerViewDTO> CreateDealer(DealerInsertDTO dealer)
        {
            var newDealer = _mapper.Map<Dealer>(dealer);
            await _dealerDAL.CreateAsync(newDealer);
            return _mapper.Map<DealerViewDTO>(newDealer);
        }

        public async Task DeleteDealer(int id)
        {
            await _dealerDAL.DeleteAsync(id);
        }

        public async Task<IEnumerable<DealerViewDTO>> GetAllDealers()
        {
            var dealers = await _dealerDAL.GetAllAsync();
            return _mapper.Map<IEnumerable<DealerViewDTO>>(dealers);
        }

        public async Task<DealerViewDTO> GetDealerById(int id)
        {
            var dealer = await _dealerDAL.GetByIdAsync(id);
            if (dealer == null)
            {
                return null;
            }
            return _mapper.Map<DealerViewDTO>(dealer);
        }

        public async Task<DealerViewDTO> UpdateDealer(DealerUpdateDTO dealer)
        {
            var existingDealer = await _dealerDAL.GetByIdAsync(dealer.DealerId);
            if (existingDealer != null)
            {
                _mapper.Map(dealer, existingDealer);
                await _dealerDAL.UpdateAsync(existingDealer);
                return _mapper.Map<DealerViewDTO>(existingDealer);
            }
            return null;
        }
    }
}