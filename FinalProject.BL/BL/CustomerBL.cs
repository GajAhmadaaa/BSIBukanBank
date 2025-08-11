using FinalProject.BL.DTO;
using FinalProject.BL.Interfaces;
using FinalProject.BO.Models;
using FinalProject.DAL.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.BL.BL
{
    public class CustomerBL : ICustomerBL
    {
        private readonly ICustomer _customerDAL;

        public CustomerBL(ICustomer customerDAL)
        {
            _customerDAL = customerDAL;
        }

        public async Task CreateCustomer(CustomerDTO customer)
        {
            var newCustomer = new Customer
            {
                Name = customer.Name,
                PhoneNumber = customer.PhoneNumber,
                Email = customer.Email,
                Address = customer.Address
            };
            await _customerDAL.CreateAsync(newCustomer);
        }

        public async Task DeleteCustomer(int id)
        {
            await _customerDAL.DeleteAsync(id);
        }

        public async Task<IEnumerable<CustomerDTO>> GetAllCustomers()
        {
            var customers = await _customerDAL.GetAllAsync();
            return customers.Select(c => new CustomerDTO
            {
                CustomerId = c.CustomerId,
                Name = c.Name,
                PhoneNumber = c.PhoneNumber,
                Email = c.Email,
                Address = c.Address
            });
        }

        public async Task<CustomerDTO> GetCustomerById(int id)
        {
            var customer = await _customerDAL.GetByIdAsync(id);
            if (customer == null)
            {
                return null;
            }
            return new CustomerDTO
            {
                CustomerId = customer.CustomerId,
                Name = customer.Name,
                PhoneNumber = customer.PhoneNumber,
                Email = customer.Email,
                Address = customer.Address
            };
        }

        public async Task UpdateCustomer(CustomerDTO customer)
        {
            var existingCustomer = await _customerDAL.GetByIdAsync(customer.CustomerId);
            if (existingCustomer != null)
            {
                existingCustomer.Name = customer.Name;
                existingCustomer.PhoneNumber = customer.PhoneNumber;
                existingCustomer.Email = customer.Email;
                existingCustomer.Address = customer.Address;
                await _customerDAL.UpdateAsync(existingCustomer);
            }
        }
    }
}