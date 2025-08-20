using FinalProject.BL.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinalProject.BL.Interfaces
{
    public interface ICustomerBL
    {
        Task<IEnumerable<CustomerViewDTO>> GetAllCustomers();
        Task<CustomerViewDTO> GetCustomerById(int id);
        Task<CustomerViewDTO> GetCustomerByEmail(string email);
        Task<CustomerViewDTO> CreateCustomer(CustomerInsertDTO customer);
        Task<CustomerViewDTO> UpdateCustomer(CustomerUpdateDTO customer);
        Task DeleteCustomer(int id);
    }
}