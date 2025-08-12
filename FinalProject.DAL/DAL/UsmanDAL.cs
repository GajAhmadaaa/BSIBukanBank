using FinalProject.DAL.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.DAL.DAL
{
    public class UsmanDAL : IUsman
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsmanDAL(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
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

                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    return false;
                }

                var roleExists = await _roleManager.RoleExistsAsync(roleName);
                if (!roleExists)
                {
                    return false;
                }

                var result = await _userManager.AddToRoleAsync(user, roleName);
                return result.Succeeded;
            }
            catch (Exception)
            {
                // Log the exception (logging mechanism not shown here)
                return false;
            }
        }

        public async Task<bool> CreateRoleAsync(string roleName)
        {
            try
            {
                if (string.IsNullOrEmpty(roleName))
                {
                    throw new ArgumentNullException(nameof(roleName), "Role name cannot be null or empty");
                }

                var roleExists = await _roleManager.RoleExistsAsync(roleName);
                if (roleExists)
                {
                    return false; // Role already exists
                }

                var role = new IdentityRole(roleName);
                var result = await _roleManager.CreateAsync(role);
                return result.Succeeded;
            }
            catch (Exception)
            {
                // Log the exception (logging mechanism not shown here)
                return false;
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

                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    return new List<string>(); // Return empty list if user not found
                }

                var roles = await _userManager.GetRolesAsync(user);
                return roles.ToList();
            }
            catch (Exception)
            {
                // Log the exception (logging mechanism not shown here)
                return new List<string>();
            }
        }

        public async Task<bool> LoginAsync(string email, string password)
        {
            try
            {
                if (string.IsNullOrEmpty(email))
                {
                    throw new ArgumentNullException(nameof(email), "Email cannot be null or empty");
                }
                if (string.IsNullOrEmpty(password))
                {
                    throw new ArgumentNullException(nameof(password), "Password cannot be null or empty");
                }

                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    return false; // User not found
                }

                var result = await _userManager.CheckPasswordAsync(user, password);
                return result;
            }
            catch (Exception)
            {
                // Log the exception (logging mechanism not shown here)
                return false;
            }
        }

        public async Task<bool> RegisterAsync(string email, string password)
        {
            try
            {
                if (string.IsNullOrEmpty(email))
                {
                    throw new ArgumentNullException(nameof(email), "Email cannot be null or empty");
                }
                if (string.IsNullOrEmpty(password))
                {
                    throw new ArgumentNullException(nameof(password), "Password cannot be null or empty");
                }

                var user = new IdentityUser { UserName = email, Email = email };
                var result = await _userManager.CreateAsync(user, password);
                return result.Succeeded;
            }
            catch (Exception)
            {
                // Log the exception (logging mechanism not shown here)
                return false;
            }
        }
    }
}