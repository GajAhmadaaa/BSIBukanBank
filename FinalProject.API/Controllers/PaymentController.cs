using FinalProject.BL.DTO;
using FinalProject.BL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace FinalProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentBL _paymentBL;

        public PaymentController(IPaymentBL paymentBL)
        {
            _paymentBL = paymentBL;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePayment([FromBody] PaymentInsertDTO paymentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _paymentBL.ProcessPaymentAsync(paymentDto);
                if (result)
                {
                    return Ok(new { Message = "Payment processed successfully." });
                }
                return BadRequest(new { Message = "Failed to process payment." });
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                // Log the exception (not shown here)
                return StatusCode(500, new { Message = "An unexpected error occurred.", Details = ex.Message });
            }
        }
    }
}
