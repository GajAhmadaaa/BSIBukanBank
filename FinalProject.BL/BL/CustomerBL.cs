using AutoMapper;
using FinalProject.BL.DTO;
using FinalProject.BL.Interfaces;
using FinalProject.BO.Models;
using FinalProject.DAL.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinalProject.BL.BL
{
    public class CustomerBL : ICustomerBL
    {
        private readonly ICustomer _customerDAL;
        private readonly IMapper _mapper;

        public CustomerBL(ICustomer customerDAL, IMapper mapper)
        {
            _customerDAL = customerDAL;
            _mapper = mapper;
        }

        public async Task<CustomerViewDTO> CreateCustomer(CustomerInsertDTO customer)
        {
            var newCustomer = _mapper.Map<Customer>(customer);
            await _customerDAL.CreateAsync(newCustomer);
            return _mapper.Map<CustomerViewDTO>(newCustomer);
        }

        public async Task DeleteCustomer(int id)
        {
            await _customerDAL.DeleteAsync(id);
        }

        public async Task<IEnumerable<CustomerViewDTO>> GetAllCustomers()
        {
            var customers = await _customerDAL.GetAllAsync();
            return _mapper.Map<IEnumerable<CustomerViewDTO>>(customers);
        }

        public async Task<CustomerViewDTO> GetCustomerById(int id)
        {
            var customer = await _customerDAL.GetByIdAsync(id);
            if (customer == null)
            {
                return null;
            }
            return _mapper.Map<CustomerViewDTO>(customer);
        }

        public async Task<CustomerViewDTO> UpdateCustomer(CustomerUpdateDTO customer)
        {
            var existingCustomer = await _customerDAL.GetByIdAsync(customer.CustomerId);
            if (existingCustomer != null)
            {
                _mapper.Map(customer, existingCustomer);
                await _customerDAL.UpdateAsync(existingCustomer);
                return _mapper.Map<CustomerViewDTO>(existingCustomer);
            }
            return null;
        }
    }
}