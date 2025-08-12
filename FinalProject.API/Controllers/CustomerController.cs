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
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerBL _customerBL;

        public CustomerController(ICustomerBL customerBL)
        {
            _customerBL = customerBL;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerViewDTO>>> GetCustomers()
        {
            var customers = await _customerBL.GetAllCustomers();
            return Ok(customers);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerViewDTO>> GetCustomer(int id)
        {
            var customer = await _customerBL.GetCustomerById(id);
            if (customer == null)
            {
                return NotFound();
            }
            return Ok(customer);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<ActionResult<CustomerViewDTO>> CreateCustomer(CustomerInsertDTO customerInsertDTO)
        {
            if (customerInsertDTO == null)
            {
                return BadRequest("Customer data is null.");
            }

            try
            {
                var createdCustomer = await _customerBL.CreateCustomer(customerInsertDTO);
                return CreatedAtAction(nameof(GetCustomer), new { id = createdCustomer.CustomerId }, createdCustomer);
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
        public async Task<ActionResult<CustomerViewDTO>> UpdateCustomer(int id, CustomerUpdateDTO customerUpdateDTO)
        {
            if (customerUpdateDTO == null || customerUpdateDTO.CustomerId != id)
            {
                return BadRequest("Customer data is invalid.");
            }

            try
            {
                var updatedCustomer = await _customerBL.UpdateCustomer(customerUpdateDTO);
                if (updatedCustomer == null)
                {
                    return NotFound();
                }
                return Ok(updatedCustomer);
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
        public async Task<ActionResult> DeleteCustomer(int id)
        {
            try
            {
                var customer = await _customerBL.GetCustomerById(id);
                if (customer == null)
                {
                    return NotFound();
                }

                await _customerBL.DeleteCustomer(id);
                return Ok($"Customer id: {id} deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}