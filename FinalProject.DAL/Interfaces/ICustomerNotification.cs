using FinalProject.BO.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinalProject.DAL.Interfaces
{
    public interface ICustomerNotification : ICrud<CustomerNotification>
    {
        Task<IEnumerable<CustomerNotification>> GetByCustomerIdAsync(int customerId);
        Task<IEnumerable<CustomerNotification>> GetUnreadByCustomerIdAsync(int customerId);
        Task MarkAsReadAsync(int notificationId);
        Task MarkAllAsReadAsync(int customerId);
    }
}