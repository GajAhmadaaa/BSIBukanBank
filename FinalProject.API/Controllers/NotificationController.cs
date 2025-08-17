using FinalProject.BL.DTO;
using FinalProject.BL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly ICustomerNotificationBL _notificationBL;

        public NotificationController(ICustomerNotificationBL notificationBL)
        {
            _notificationBL = notificationBL;
        }

        /// <summary>
        /// Mendapatkan notifikasi untuk pelanggan tertentu
        /// </summary>
        /// <param name="customerId">ID pelanggan</param>
        /// <returns>Daftar notifikasi untuk pelanggan</returns>
        [HttpGet("{customerId}")]
        public async Task<IActionResult> GetNotifications(int customerId)
        {
            var notifications = await _notificationBL.GetByCustomerIdAsync(customerId);
            return Ok(notifications);
        }

        /// <summary>
        /// Mendapatkan notifikasi yang belum dibaca untuk pelanggan tertentu
        /// </summary>
        /// <param name="customerId">ID pelanggan</param>
        /// <returns>Daftar notifikasi yang belum dibaca</returns>
        [HttpGet("{customerId}/unread")]
        public async Task<IActionResult> GetUnreadNotifications(int customerId)
        {
            var notifications = await _notificationBL.GetUnreadByCustomerIdAsync(customerId);
            return Ok(notifications);
        }

        /// <summary>
        /// Menandai notifikasi sebagai sudah dibaca
        /// </summary>
        /// <param name="notificationId">ID notifikasi</param>
        /// <returns>Status berhasil</returns>
        [HttpPut("{notificationId}/read")]
        public async Task<IActionResult> MarkAsRead(int notificationId)
        {
            await _notificationBL.MarkAsReadAsync(notificationId);
            return Ok(new { Message = "Notifikasi berhasil ditandai sebagai sudah dibaca" });
        }

        /// <summary>
        /// Menandai semua notifikasi sebagai sudah dibaca untuk pelanggan tertentu
        /// </summary>
        /// <param name="customerId">ID pelanggan</param>
        /// <returns>Status berhasil</returns>
        [HttpPut("{customerId}/read-all")]
        public async Task<IActionResult> MarkAllAsRead(int customerId)
        {
            await _notificationBL.MarkAllAsReadAsync(customerId);
            return Ok(new { Message = "Semua notifikasi berhasil ditandai sebagai sudah dibaca" });
        }

        /// <summary>
        /// Membuat notifikasi baru untuk pengujian
        /// </summary>
        /// <param name="notificationDto">Data notifikasi</param>
        /// <returns>Notifikasi yang dibuat</returns>
        [HttpPost]
        public async Task<IActionResult> CreateNotification([FromBody] CustomerNotificationInsertDTO notificationDto)
        {
            var notification = await _notificationBL.CreateAsync(notificationDto);
            return Ok(notification);
        }
    }
}