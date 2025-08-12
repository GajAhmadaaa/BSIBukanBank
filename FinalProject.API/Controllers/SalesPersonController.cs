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
    public class SalesPersonController : ControllerBase
    {
        private readonly ISalesPersonBL _salesPersonBL;

        public SalesPersonController(ISalesPersonBL salesPersonBL)
        {
            _salesPersonBL = salesPersonBL;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SalesPersonViewDTO>>> Get()
        {
            var salesPeople = await _salesPersonBL.GetAllAsync();
            return Ok(salesPeople);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SalesPersonViewDTO>> Get(int id)
        {
            var salesPerson = await _salesPersonBL.GetByIdAsync(id);
            if (salesPerson == null)
            {
                return NotFound($"SalesPerson with ID {id} not found.");
            }
            return Ok(salesPerson);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<ActionResult<SalesPersonViewDTO>> Post(SalesPersonInsertDTO salesPersonInsertDTO)
        {
            try
            {
                if (salesPersonInsertDTO == null)
                {
                    return BadRequest("SalesPerson data cannot be null.");
                }
                var addedSalesPerson = await _salesPersonBL.CreateAsync(salesPersonInsertDTO);
                return CreatedAtAction(nameof(Get), new { id = addedSalesPerson.Id }, addedSalesPerson);
            }
            catch (ArgumentException aEx)
            {
                return BadRequest($"Error adding sales person: {aEx.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult<SalesPersonViewDTO>> Put(int id, SalesPersonUpdateDTO salesPersonUpdateDTO)
        {
            try
            {
                if (id != salesPersonUpdateDTO.Id)
                {
                    return BadRequest("SalesPerson ID mismatch.");
                }

                var updatedSalesPerson = await _salesPersonBL.UpdateAsync(id, salesPersonUpdateDTO);
                if (updatedSalesPerson == null)
                {
                    return NotFound($"SalesPerson with ID {id} not found.");
                }
                return Ok(updatedSalesPerson);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error updating sales person with ID {id}: {ex.Message}");
            }
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var salesPerson = await _salesPersonBL.GetByIdAsync(id);
                if (salesPerson == null)
                {
                    return NotFound($"SalesPerson with ID {id} not found.");
                }

                await _salesPersonBL.DeleteAsync(id);
                return Ok($"Sales person with ID {id} deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error deleting sales person with ID {id}: {ex.Message}");
            }
        }
    }
}