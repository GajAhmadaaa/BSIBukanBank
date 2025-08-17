using FinalProject.BL.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinalProject.BL.Interfaces
{
    public interface ICustomerNotificationBL
    {
        Task<IEnumerable<CustomerNotificationViewDTO>> GetAllAsync();
        Task<CustomerNotificationViewDTO> GetByIdAsync(int id);
        Task<CustomerNotificationViewDTO> CreateAsync(CustomerNotificationInsertDTO customerNotification);
        Task<CustomerNotificationViewDTO> UpdateAsync(int id, CustomerNotificationUpdateDTO customerNotification);
        Task DeleteAsync(int id);
        
        // Method khusus untuk CustomerNotification
        Task<IEnumerable<CustomerNotificationViewDTO>> GetByCustomerIdAsync(int customerId);
        Task<IEnumerable<CustomerNotificationViewDTO>> GetUnreadByCustomerIdAsync(int customerId);
        Task MarkAsReadAsync(int notificationId);
        Task MarkAllAsReadAsync(int customerId);
    }
}