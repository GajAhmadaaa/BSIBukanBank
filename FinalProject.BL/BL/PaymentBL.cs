using AutoMapper;
using FinalProject.BL.DTO;
using FinalProject.BL.Interfaces;
using FinalProject.BO.Models;
using FinalProject.DAL.Interfaces;
using System;
using System.Threading.Tasks;

namespace FinalProject.BL.BL
{
    public class PaymentBL : IPaymentBL
    {
        private readonly IPaymentDAL _paymentDAL;
        private readonly ISalesAgreement _salesAgreementDAL;
        private readonly IMapper _mapper;

        public PaymentBL(IPaymentDAL paymentDAL, ISalesAgreement salesAgreementDAL, IMapper mapper)
        {
            _paymentDAL = paymentDAL;
            _salesAgreementDAL = salesAgreementDAL;
            _mapper = mapper;
        }

        public async Task<bool> ProcessPaymentAsync(PaymentInsertDTO paymentDto)
        {
            var agreement = await _salesAgreementDAL.GetByIdAsync(paymentDto.SalesAgreementID);
            if (agreement == null)
            {
                throw new ArgumentException("Sales Agreement not found.");
            }

            if (agreement.Status == "Paid")
            {
                throw new InvalidOperationException("This agreement has already been paid in full.");
            }

            var payment = _mapper.Map<PaymentHistory>(paymentDto);
            await _paymentDAL.CreateAsync(payment);

            decimal totalPaid = await _paymentDAL.GetTotalPaymentsForAgreementAsync(paymentDto.SalesAgreementID);

            if (agreement.TotalAmount.HasValue && totalPaid >= agreement.TotalAmount.Value)
            {
                agreement.Status = "Paid";
                await _salesAgreementDAL.UpdateAsync(agreement);
            }

            return true;
        }
    }
}
