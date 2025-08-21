using FinalProject.BL.DTO;
using FinalProject.BL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SalesAgreementController : ControllerBase
    {
        private readonly ISalesAgreementBL _salesAgreementBL;

        public SalesAgreementController(ISalesAgreementBL salesAgreementBL)
        {
            _salesAgreementBL = salesAgreementBL;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SalesAgreementViewDTO>> GetSalesAgreement(int id)
        {
            var salesAgreement = await _salesAgreementBL.GetByIdAsync(id);
            if (salesAgreement == null)
            {
                return NotFound();
            }
            return Ok(salesAgreement);
        }

        // Endpoint untuk mendapatkan semua agreement customer
        [HttpGet("customer/{customerId}")]
        public async Task<ActionResult<IEnumerable<SalesAgreementViewDTO>>> GetAgreementsForCustomer(int customerId)
        {
            var agreements = await _salesAgreementBL.GetByCustomerIdAsync(customerId);
            return Ok(agreements);
        }

        // Endpoint untuk mendapatkan agreement unpaid customer
        [HttpGet("customer/{customerId}/unpaid")]
        public async Task<ActionResult<IEnumerable<SalesAgreementViewDTO>>> GetUnpaidAgreements(int customerId)
        {
            var agreements = await _salesAgreementBL.GetUnpaidByCustomerIdAsync(customerId);
            return Ok(agreements);
        }

        // Endpoint untuk mendapatkan agreement paid customer
        [HttpGet("customer/{customerId}/paid")]
        public async Task<ActionResult<IEnumerable<SalesAgreementViewDTO>>> GetPaidAgreements(int customerId)
        {
            var agreements = await _salesAgreementBL.GetPaidByCustomerIdAsync(customerId);
            return Ok(agreements);
        }

        // Endpoint untuk mengkonversi LOI ke SalesAgreement
        [HttpPost("convert-from-loi/{loiId}")]
        public async Task<ActionResult<SalesAgreementViewDTO>> ConvertFromLOI(int loiId)
        {
            try
            {
                var salesAgreement = await _salesAgreementBL.ConvertFromLOIAsync(loiId);
                return Ok(salesAgreement);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        // Endpoint untuk mengubah status SalesAgreement dari unpaid ke paid
        [HttpPut("{id}/mark-as-paid")]
        public async Task<ActionResult<SalesAgreementViewDTO>> MarkAsPaid(int id)
        {
            try
            {
                var salesAgreement = await _salesAgreementBL.MarkAsPaidAsync(id);
                return Ok(salesAgreement);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}