using FinalProject.BO.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinalProject.DAL.Interfaces
{
    public interface ICustomer : ICrud<Customer>
    {
        Task<IEnumerable<Customer>> GetCustomersByNameAsync(string name);
        Task<Customer?> GetCustomerByEmailAsync(string email);
    }
}