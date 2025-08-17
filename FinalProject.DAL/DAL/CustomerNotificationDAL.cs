using FinalProject.BO.Models;
using FinalProject.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace FinalProject.DAL.DAL
{
    public class CustomerNotificationDAL : BaseDAL<CustomerNotification>, ICustomerNotification
    {
        public CustomerNotificationDAL(FinalProjectContext context) : base(context)
        {
        }

        // Method khusus untuk mendapatkan notifikasi berdasarkan customer
        public async Task<IEnumerable<CustomerNotification>> GetByCustomerIdAsync(int customerId)
        {
            return await _dbSet.Where(n => n.CustomerId == customerId)
                              .OrderByDescending(n => n.CreatedDate)
                              .ToListAsync();
        }

        // Method khusus untuk mendapatkan notifikasi yang belum dibaca
        public async Task<IEnumerable<CustomerNotification>> GetUnreadByCustomerIdAsync(int customerId)
        {
            return await _dbSet.Where(n => n.CustomerId == customerId && !n.IsRead)
                              .OrderByDescending(n => n.CreatedDate)
                              .ToListAsync();
        }

        // Method untuk menandai notifikasi sebagai sudah dibaca
        public async Task MarkAsReadAsync(int notificationId)
        {
            var notification = await _dbSet.FindAsync(notificationId);
            if (notification != null)
            {
                notification.IsRead = true;
                notification.ReadDate = System.DateTime.Now;
                await _context.SaveChangesAsync();
            }
        }

        // Method untuk menandai semua notifikasi customer sebagai sudah dibaca
        public async Task MarkAllAsReadAsync(int customerId)
        {
            var notifications = await _dbSet.Where(n => n.CustomerId == customerId && !n.IsRead)
                                           .ToListAsync();
            foreach (var notification in notifications)
            {
                notification.IsRead = true;
                notification.ReadDate = System.DateTime.Now;
            }
            await _context.SaveChangesAsync();
        }
    }
}