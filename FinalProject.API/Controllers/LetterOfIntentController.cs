using FinalProject.BL.DTO;
using FinalProject.BL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class LetterOfIntentController : ControllerBase
    {
        private readonly ILetterOfIntentBL _letterOfIntentBL;

        public LetterOfIntentController(ILetterOfIntentBL letterOfIntentBL)
        {
            _letterOfIntentBL = letterOfIntentBL;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LetterOfIntentViewDTO>> GetLetterOfIntent(int id)
        {
            var letterOfIntent = await _letterOfIntentBL.GetByIdAsync(id);
            if (letterOfIntent == null)
            {
                return NotFound();
            }
            return Ok(letterOfIntent);
        }

        // Endpoint untuk mendapatkan semua order customer
        [HttpGet("customer/{customerId}")]
        public async Task<ActionResult<IEnumerable<LetterOfIntentViewDTO>>> GetOrdersForCustomer(int customerId)
        {
            var orders = await _letterOfIntentBL.GetByCustomerIdAsync(customerId);
            return Ok(orders);
        }

        // Endpoint untuk mendapatkan order pending customer
        [HttpGet("customer/{customerId}/pending")]
        public async Task<ActionResult<IEnumerable<LetterOfIntentViewDTO>>> GetPendingOrders(int customerId)
        {
            var orders = await _letterOfIntentBL.GetPendingByCustomerIdAsync(customerId);
            return Ok(orders);
        }

        // Endpoint untuk mendapatkan order unpaid customer
        [HttpGet("customer/{customerId}/unpaid")]
        public async Task<ActionResult<IEnumerable<LetterOfIntentViewDTO>>> GetUnpaidOrders(int customerId)
        {
            var orders = await _letterOfIntentBL.GetUnpaidByCustomerIdAsync(customerId);
            return Ok(orders);
        }

        // Endpoint untuk mendapatkan order paid customer
        [HttpGet("customer/{customerId}/paid")]
        public async Task<ActionResult<IEnumerable<LetterOfIntentViewDTO>>> GetPaidOrders(int customerId)
        {
            var orders = await _letterOfIntentBL.GetPaidByCustomerIdAsync(customerId);
            return Ok(orders);
        }
    }
}