using FinalProject.BL.DTO;
using FinalProject.BL.Helpers;
using FinalProject.BL.Interfaces;
using FinalProject.DAL.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FinalProject.BL.BL
{
    public class UsmanBL : IUsmanBL
    {
        private readonly IUsman _usmanDAL;
        private readonly AppSettings _appSettings;

        public UsmanBL(IUsman usmanDAL, IOptions<AppSettings> appSettings)
        {
            _usmanDAL = usmanDAL;
            _appSettings = appSettings.Value;
        }

        public async Task<bool> AddUserToRoleAsync(string email, string roleName)
        {
            try
            {
                if (string.IsNullOrEmpty(email))
                {
                    throw new ArgumentNullException(nameof(email), "Email cannot be null or empty");
                }
                if (string.IsNullOrEmpty(roleName))
                {
                    throw new ArgumentNullException(nameof(roleName), "Role name cannot be null or empty");
                }
                var result = await _usmanDAL.AddUserToRoleAsync(email, roleName);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> CreateRoleAsync(RoleCreateDTO roleCreateDTO)
        {
            try
            {
                if (string.IsNullOrEmpty(roleCreateDTO.RoleName))
                {
                    throw new ArgumentNullException(nameof(roleCreateDTO), "Role name cannot be null or empty");
                }
                var result = await _usmanDAL.CreateRoleAsync(roleCreateDTO.RoleName);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<string>> GetRolesByUserAsync(string email)
        {
            try
            {
                if (string.IsNullOrEmpty(email))
                {
                    throw new ArgumentNullException(nameof(email), "Email cannot be null or empty");
                }
                var roles = await _usmanDAL.GetRolesByUserAsync(email);
                return roles;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<UserWithTokenDTO> LoginAsync(LoginDTO loginDTO)
        {
            try
            {
                if (loginDTO == null)
                {
                    throw new ArgumentNullException(nameof(loginDTO), "Login data cannot be null");
                }
                
                // Validasi input
                if (string.IsNullOrEmpty(loginDTO.Email))
                {
                    throw new ArgumentException("Email is required", nameof(loginDTO.Email));
                }
                
                if (string.IsNullOrEmpty(loginDTO.Password))
                {
                    throw new ArgumentException("Password is required", nameof(loginDTO.Password));
                }

                var loginResult = await _usmanDAL.LoginAsync(loginDTO.Email, loginDTO.Password);
                if (!loginResult)
                {
                    return null; // User not found or invalid password
                }

                // Get user roles
                var roles = await _usmanDAL.GetRolesByUserAsync(loginDTO.Email);

                // Create claims
                List<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.Email, loginDTO.Email));
                if (roles != null && roles.Count > 0)
                {
                    foreach (var role in roles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role));
                    }
                }

                // Generate JWT token
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.UtcNow.AddHours(1),
                    SigningCredentials = new SigningCredentials(
                        new SymmetricSecurityKey(key),
                        SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);

                var result = new UserWithTokenDTO
                {
                    Email = loginDTO.Email,
                    Token = tokenHandler.WriteToken(token)
                };

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error during login: {ex.Message}", ex);
            }
        }

        public async Task<bool> RegisterAsync(RegistrationDTO registrationDTO)
        {
            try
            {
                if (registrationDTO == null)
                {
                    throw new ArgumentNullException(nameof(registrationDTO), "Registration data cannot be null");
                }
                
                // Validasi input
                if (string.IsNullOrEmpty(registrationDTO.Email))
                {
                    throw new ArgumentException("Email is required", nameof(registrationDTO.Email));
                }
                
                if (string.IsNullOrEmpty(registrationDTO.Password))
                {
                    throw new ArgumentException("Password is required", nameof(registrationDTO.Password));
                }
                
                if (string.IsNullOrEmpty(registrationDTO.ConfirmPassword))
                {
                    throw new ArgumentException("Confirm password is required", nameof(registrationDTO.ConfirmPassword));
                }
                
                if (registrationDTO.Password != registrationDTO.ConfirmPassword)
                {
                    throw new ArgumentException("Password and confirm password do not match");
                }

                var result = await _usmanDAL.RegisterAsync(registrationDTO.Email, registrationDTO.Password);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error during registration: {ex.Message}", ex);
            }
        }
    }
}