using FinalProject.BL.DTO;
using FinalProject.BL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DealerController : ControllerBase
    {
        private readonly IDealerBL _dealerBL;

        public DealerController(IDealerBL dealerBL)
        {
            _dealerBL = dealerBL;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DealerViewDTO>>> GetDealers()
        {
            var dealers = await _dealerBL.GetAllDealers();
            return Ok(dealers);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DealerViewDTO>> GetDealer(int id)
        {
            var dealer = await _dealerBL.GetDealerById(id);
            if (dealer == null)
            {
                return NotFound();
            }
            return Ok(dealer);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<ActionResult<DealerViewDTO>> CreateDealer(DealerInsertDTO dealerInsertDTO)
        {
            if (dealerInsertDTO == null)
            {
                return BadRequest("Dealer data is null.");
            }

            try
            {
                var createdDealer = await _dealerBL.CreateDealer(dealerInsertDTO);
                return CreatedAtAction(nameof(GetDealer), new { id = createdDealer.DealerId }, createdDealer);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult<DealerViewDTO>> UpdateDealer(int id, DealerUpdateDTO dealerUpdateDTO)
        {
            if (dealerUpdateDTO == null || dealerUpdateDTO.DealerId != id)
            {
                return BadRequest("Dealer data is invalid.");
            }

            try
            {
                var updatedDealer = await _dealerBL.UpdateDealer(dealerUpdateDTO);
                if (updatedDealer == null)
                {
                    return NotFound();
                }
                return Ok(updatedDealer);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteDealer(int id)
        {
            try
            {
                var dealer = await _dealerBL.GetDealerById(id);
                if (dealer == null)
                {
                    return NotFound();
                }

                await _dealerBL.DeleteDealer(id);
                return Ok($"Dealer id: {id} deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}