using FinalProject.BL.DTO;
using FinalProject.BL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly ICustomerBL _customerBL;
        private readonly ISalesPersonBL _salesPersonBL;
        private readonly IUsmanBL _usmanBL;

        public RegistrationController(ICustomerBL customerBL, ISalesPersonBL salesPersonBL, IUsmanBL usmanBL)
        {
            _customerBL = customerBL;
            _salesPersonBL = salesPersonBL;
            _usmanBL = usmanBL;
        }

        /// <summary>
        /// Mendaftarkan Customer baru.
        /// </summary>
        /// <param name="customerRegistrationDto">Data customer dan user untuk registrasi.</param>
        /// <returns>Customer yang berhasil dibuat.</returns>
        [HttpPost("customer")]
        public async Task<ActionResult<CustomerViewDTO>> RegisterCustomerAsync(CustomerRegistrationDTO customerRegistrationDto)
        {
            if (customerRegistrationDto == null)
            {
                return BadRequest("Registration data cannot be null");
            }

            // Validasi input tambahan jika diperlukan
            if (string.IsNullOrWhiteSpace(customerRegistrationDto.Email) ||
                string.IsNullOrWhiteSpace(customerRegistrationDto.Password) ||
                customerRegistrationDto.CustomerData == null ||
                string.IsNullOrWhiteSpace(customerRegistrationDto.CustomerData.Name))
            {
                return BadRequest("Email, Password, and Customer Name are required.");
            }

            try
            {
                // 1. Buat user di ASP.NET Core Identity
                var userRegistrationDto = new RegistrationDTO
                {
                    Email = customerRegistrationDto.Email,
                    Password = customerRegistrationDto.Password,
                    ConfirmPassword = customerRegistrationDto.Password
                };
                
                var userCreated = await _usmanBL.RegisterAsync(userRegistrationDto);
                if (!userCreated)
                {
                    return BadRequest("Failed to create user account.");
                }

                // 2. Assign Role "customer" ke user yang baru dibuat
                var roleAssigned = await _usmanBL.AddUserToRoleAsync(customerRegistrationDto.Email, "customer");
                // Note: Kita tidak return error jika assign role gagal, karena user sudah dibuat.
                // Bisa log error ini jika ada sistem logging.

                // 3. Simpan data customer ke tabel Customer
                // Pastikan email di CustomerData juga diisi
                customerRegistrationDto.CustomerData.Email = customerRegistrationDto.Email;
                var createdCustomer = await _customerBL.CreateCustomer(customerRegistrationDto.CustomerData);

                return CreatedAtAction(nameof(CustomerController.GetCustomer), "Customer", new { id = createdCustomer.CustomerId }, createdCustomer);
            }
            catch (ArgumentException ex)
            {
                // Tangkap error validasi khusus dari BL/DAL
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // Tangkap error umum
                // Log error disini jika ada sistem logging
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Mendaftarkan SalesPerson baru.
        /// </summary>
        /// <param name="salesPersonRegistrationDto">Data salesperson dan user untuk registrasi.</param>
        /// <returns>SalesPerson yang berhasil dibuat.</returns>
        [HttpPost("salesperson")]
        public async Task<ActionResult<SalesPersonViewDTO>> RegisterSalesPersonAsync(SalesPersonRegistrationDTO salesPersonRegistrationDto)
        {
            if (salesPersonRegistrationDto == null)
            {
                return BadRequest("Registration data cannot be null");
            }

            // Validasi input tambahan jika diperlukan
            if (string.IsNullOrWhiteSpace(salesPersonRegistrationDto.Email) ||
                string.IsNullOrWhiteSpace(salesPersonRegistrationDto.Password) ||
                salesPersonRegistrationDto.SalesPersonData == null ||
                string.IsNullOrWhiteSpace(salesPersonRegistrationDto.SalesPersonData.Name) ||
                salesPersonRegistrationDto.SalesPersonData.DealerId <= 0)
            {
                return BadRequest("Email, Password, SalesPerson Name, and DealerId are required.");
            }

            try
            {
                // 1. Buat user di ASP.NET Core Identity
                var userRegistrationDto = new RegistrationDTO
                {
                    Email = salesPersonRegistrationDto.Email,
                    Password = salesPersonRegistrationDto.Password,
                    ConfirmPassword = salesPersonRegistrationDto.Password
                };
                
                var userCreated = await _usmanBL.RegisterAsync(userRegistrationDto);
                if (!userCreated)
                {
                    return BadRequest("Failed to create user account.");
                }

                // 2. Assign Role "salesperson" ke user yang baru dibuat
                var roleAssigned = await _usmanBL.AddUserToRoleAsync(salesPersonRegistrationDto.Email, "salesperson");
                // Note: Kita tidak return error jika assign role gagal, karena user sudah dibuat.
                // Bisa log error ini jika ada sistem logging.

                // 3. Simpan data salesperson ke tabel SalesPerson
                var createdSalesPerson = await _salesPersonBL.CreateAsync(salesPersonRegistrationDto.SalesPersonData);

                return CreatedAtAction(nameof(SalesPersonController.Get), "SalesPerson", new { id = createdSalesPerson.Id }, createdSalesPerson);
            }
            catch (ArgumentException ex)
            {
                // Tangkap error validasi khusus dari BL/DAL
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // Tangkap error umum
                // Log error disini jika ada sistem logging
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }
    }
}