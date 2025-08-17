using AutoMapper;
using FinalProject.BL.DTO;
using FinalProject.BL.Interfaces;
using FinalProject.BO.Models;
using FinalProject.DAL.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinalProject.BL.BL
{
    public class CustomerNotificationBL : ICustomerNotificationBL
    {
        private readonly ICustomerNotification _customerNotificationDAL;
        private readonly IMapper _mapper;

        public CustomerNotificationBL(ICustomerNotification customerNotificationDAL, IMapper mapper)
        {
            _customerNotificationDAL = customerNotificationDAL;
            _mapper = mapper;
        }

        public async Task<CustomerNotificationViewDTO> CreateAsync(CustomerNotificationInsertDTO customerNotification)
        {
            var newCustomerNotification = _mapper.Map<CustomerNotification>(customerNotification);
            newCustomerNotification.CreatedDate = System.DateTime.Now;
            await _customerNotificationDAL.CreateAsync(newCustomerNotification);
            return _mapper.Map<CustomerNotificationViewDTO>(newCustomerNotification);
        }

        public async Task DeleteAsync(int id)
        {
            await _customerNotificationDAL.DeleteAsync(id);
        }

        public async Task<IEnumerable<CustomerNotificationViewDTO>> GetAllAsync()
        {
            var customerNotifications = await _customerNotificationDAL.GetAllAsync();
            return _mapper.Map<IEnumerable<CustomerNotificationViewDTO>>(customerNotifications);
        }

        public async Task<CustomerNotificationViewDTO> GetByIdAsync(int id)
        {
            var customerNotification = await _customerNotificationDAL.GetByIdAsync(id);
            if (customerNotification == null)
            {
                return null;
            }
            return _mapper.Map<CustomerNotificationViewDTO>(customerNotification);
        }

        public async Task<CustomerNotificationViewDTO> UpdateAsync(int id, CustomerNotificationUpdateDTO customerNotification)
        {
            var existingCustomerNotification = await _customerNotificationDAL.GetByIdAsync(id);
            if (existingCustomerNotification != null)
            {
                _mapper.Map(customerNotification, existingCustomerNotification);
                await _customerNotificationDAL.UpdateAsync(existingCustomerNotification);
                return _mapper.Map<CustomerNotificationViewDTO>(existingCustomerNotification);
            }
            return null;
        }

        // Method khusus untuk CustomerNotification
        public async Task<IEnumerable<CustomerNotificationViewDTO>> GetByCustomerIdAsync(int customerId)
        {
            var customerNotifications = await _customerNotificationDAL.GetByCustomerIdAsync(customerId);
            return _mapper.Map<IEnumerable<CustomerNotificationViewDTO>>(customerNotifications);
        }

        public async Task<IEnumerable<CustomerNotificationViewDTO>> GetUnreadByCustomerIdAsync(int customerId)
        {
            var customerNotifications = await _customerNotificationDAL.GetUnreadByCustomerIdAsync(customerId);
            return _mapper.Map<IEnumerable<CustomerNotificationViewDTO>>(customerNotifications);
        }

        public async Task MarkAsReadAsync(int notificationId)
        {
            await _customerNotificationDAL.MarkAsReadAsync(notificationId);
        }

        public async Task MarkAllAsReadAsync(int customerId)
        {
            await _customerNotificationDAL.MarkAllAsReadAsync(customerId);
        }
    }
}