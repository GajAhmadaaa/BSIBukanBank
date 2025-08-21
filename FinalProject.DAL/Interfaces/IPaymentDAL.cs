using FinalProject.BO.Models;
using System.Threading.Tasks;

namespace FinalProject.DAL.Interfaces
{
    public interface IPaymentDAL : ICrud<PaymentHistory>
    {
        Task<decimal> GetTotalPaymentsForAgreementAsync(int salesAgreementId);
    }
}
