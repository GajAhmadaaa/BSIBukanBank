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
    public class DealerInventoryController : ControllerBase
    {
        private readonly IDealerInventoryBL _dealerInventoryBL;

        public DealerInventoryController(IDealerInventoryBL dealerInventoryBL)
        {
            _dealerInventoryBL = dealerInventoryBL;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DealerInventoryViewDTO>>> Get()
        {
            var dealerInventories = await _dealerInventoryBL.GetAllAsync();
            return Ok(dealerInventories);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DealerInventoryViewDTO>> Get(int id)
        {
            var dealerInventory = await _dealerInventoryBL.GetByIdAsync(id);
            if (dealerInventory == null)
            {
                return NotFound($"DealerInventory with ID {id} not found.");
            }
            return Ok(dealerInventory);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<ActionResult<DealerInventoryViewDTO>> Post(DealerInventoryInsertDTO dealerInventoryInsertDTO)
        {
            try
            {
                if (dealerInventoryInsertDTO == null)
                {
                    return BadRequest("DealerInventory data cannot be null.");
                }
                var addedDealerInventory = await _dealerInventoryBL.CreateAsync(dealerInventoryInsertDTO);
                return CreatedAtAction(nameof(Get), new { id = addedDealerInventory.Id }, addedDealerInventory);
            }
            catch (ArgumentException aEx)
            {
                return BadRequest($"Error adding dealer inventory: {aEx.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult<DealerInventoryViewDTO>> Put(int id, DealerInventoryUpdateDTO dealerInventoryUpdateDTO)
        {
            try
            {
                if (id != dealerInventoryUpdateDTO.Id)
                {
                    return BadRequest("DealerInventory ID mismatch.");
                }

                var updatedDealerInventory = await _dealerInventoryBL.UpdateAsync(id, dealerInventoryUpdateDTO);
                if (updatedDealerInventory == null)
                {
                    return NotFound($"DealerInventory with ID {id} not found.");
                }
                return Ok(updatedDealerInventory);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error updating dealer inventory with ID {id}: {ex.Message}");
            }
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var dealerInventory = await _dealerInventoryBL.GetByIdAsync(id);
                if (dealerInventory == null)
                {
                    return NotFound($"DealerInventory with ID {id} not found.");
                }

                await _dealerInventoryBL.DeleteAsync(id);
                return Ok($"Dealer inventory with ID {id} deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error deleting dealer inventory with ID {id}: {ex.Message}");
            }
        }
    }
}