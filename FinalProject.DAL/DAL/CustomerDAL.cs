using FinalProject.BO.Models;
using FinalProject.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace FinalProject.DAL.DAL
{
    public class CustomerDAL : BaseDAL<Customer>, ICustomer
    {
        public CustomerDAL(FinalProjectContext context) : base(context)
        {
        }

        // Contoh method khusus Customer
        public async Task<IEnumerable<Customer>> GetCustomersByNameAsync(string name)
        {
            return await _dbSet.Where(c => c.Name.Contains(name)).ToListAsync();
        }

        public async Task<Customer?> GetCustomerByEmailAsync(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(c => c.Email == email);
        }
    }
}