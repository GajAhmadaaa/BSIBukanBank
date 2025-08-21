using FinalProject.BL.DTO;
using System.Threading.Tasks;

namespace FinalProject.BL.Interfaces
{
    public interface IPaymentBL
    {
        Task<bool> ProcessPaymentAsync(PaymentInsertDTO paymentDto);
    }
}