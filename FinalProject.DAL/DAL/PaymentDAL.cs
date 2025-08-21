using FinalProject.BO.Models;
using FinalProject.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.DAL.DAL
{
    public class PaymentDAL : BaseDAL<PaymentHistory>, IPaymentDAL
    {
        public PaymentDAL(FinalProjectContext context) : base(context)
        {
        }

        public async Task<decimal> GetTotalPaymentsForAgreementAsync(int salesAgreementId)
        {
            return await _dbSet
                .Where(p => p.SalesAgreementId == salesAgreementId)
                .SumAsync(p => p.PaymentAmount);
        }
    }
}
