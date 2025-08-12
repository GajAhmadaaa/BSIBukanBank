using FinalProject.BL.DTO;
using FinalProject.BL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsmanController : ControllerBase
    {
        private readonly IUsmanBL _usmanBL;

        public UsmanController(IUsmanBL usmanBL)
        {
            _usmanBL = usmanBL;
        }

        //login
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(LoginDTO loginDTO)
        {
            if (loginDTO == null)
            {
                return BadRequest("Login data cannot be null");
            }
            try
            {
                var userWithToken = await _usmanBL.LoginAsync(loginDTO);
                if (userWithToken != null)
                {
                    return Ok(userWithToken);
                }
                else
                {
                    return Unauthorized("Invalid login credentials");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        //registration
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync(RegistrationDTO registrationDTO)
        {
            if (registrationDTO == null)
            {
                return BadRequest("Registration data cannot be null");
            }
            try
            {
                var result = await _usmanBL.RegisterAsync(registrationDTO);
                if (result)
                {
                    return Ok("Registration successful");
                }
                else
                {
                    return BadRequest("Registration failed");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        //create role
        [HttpPost("create-role")]
        public async Task<IActionResult> CreateRoleAsync(RoleCreateDTO roleCreateDTO)
        {
            if (string.IsNullOrWhiteSpace(roleCreateDTO.RoleName))
            {
                return BadRequest("Role name cannot be empty");
            }
            try
            {
                var result = await _usmanBL.CreateRoleAsync(roleCreateDTO);
                if (result)
                {
                    return Ok("Role created successfully");
                }
                else
                {
                    return BadRequest("Role already exists or creation failed");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        //register user to role
        [HttpPost("add-user-to-role")]
        public async Task<IActionResult> AddUserToRoleAsync(RoleInsertDTO roleInsertDTO)
        {
            if (roleInsertDTO == null || string.IsNullOrWhiteSpace(roleInsertDTO.Email) || string.IsNullOrWhiteSpace(roleInsertDTO.RoleName))
            {
                return BadRequest("Email and role name cannot be empty");
            }
            try
            {
                var result = await _usmanBL.AddUserToRoleAsync(roleInsertDTO.Email, roleInsertDTO.RoleName);
                if (result)
                {
                    return Ok("User added to role successfully");
                }
                else
                {
                    return BadRequest("User not found or role does not exist");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }

        }
    }
}