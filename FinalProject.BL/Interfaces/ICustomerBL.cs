using FinalProject.BL.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinalProject.BL.Interfaces
{
    public interface ICustomerBL
    {
        Task<IEnumerable<CustomerDTO>> GetAllCustomers();
        Task<CustomerDTO> GetCustomerById(int id);
        Task CreateCustomer(CustomerDTO customer);
        Task UpdateCustomer(CustomerDTO customer);
        Task DeleteCustomer(int id);
    }
}