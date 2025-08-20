using FinalProject.DAL;
using FinalProject.MVC.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FinalProject.MVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly FinalProjectContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(
            FinalProjectContext context,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            
            // Ensure "customer" role exists
            InitializeRoles().Wait();
        }

        private async Task InitializeRoles()
        {
            if (!await _roleManager.RoleExistsAsync("customer"))
            {
                var roleResult = await _roleManager.CreateAsync(new IdentityRole("customer"));
            }
        }

        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                
                if (result.Succeeded)
                {
                    // Get user details to check role
                    var user = await _userManager.FindByEmailAsync(model.Email);
                    if (user != null)
                    {
                        // Check if user has customer role
                        var isCustomer = await _userManager.IsInRoleAsync(user, "customer");
                        if (!isCustomer)
                        {
                            await _signInManager.SignOutAsync();
                            ModelState.AddModelError(string.Empty, "Access denied. This application is for customers only.");
                            return View(model);
                        }

                        // Get customer details to add custom claim
                        var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Email == model.Email);
                        if (customer != null)
                        {
                            // Add custom claim for CustomerId
                            var existingClaim = (await _userManager.GetClaimsAsync(user)).FirstOrDefault(c => c.Type == "CustomerId");
                            if (existingClaim != null)
                            {
                                await _userManager.RemoveClaimAsync(user, existingClaim);
                            }
                            await _userManager.AddClaimAsync(user, new Claim("CustomerId", customer.CustomerId.ToString(), ClaimValueTypes.Integer32));
                            
                            // Refresh the sign-in cookie to include the new claim
                            await _signInManager.RefreshSignInAsync(user);
                        }
                    }

                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }

                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View(model);
            }

            return View(model);
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Validasi tambahan
                if (string.IsNullOrWhiteSpace(model.Name) || 
                    string.IsNullOrWhiteSpace(model.Email) || 
                    string.IsNullOrWhiteSpace(model.Password) ||
                    string.IsNullOrWhiteSpace(model.PhoneNumber) ||
                    string.IsNullOrWhiteSpace(model.Address))
                {
                    ModelState.AddModelError(string.Empty, "All fields are required.");
                    return View(model);
                }

                // Check if email is already used
                var existingUser = await _userManager.FindByEmailAsync(model.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Email", "Email is already taken.");
                    return View(model);
                }

                // Check if customer with same email already exists
                var existingCustomer = await _context.Customers.FirstOrDefaultAsync(c => c.Email == model.Email);
                if (existingCustomer != null)
                {
                    ModelState.AddModelError("Email", "Customer with this email already exists.");
                    return View(model);
                }

                // Create Identity user
                var user = new IdentityUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    try
                    {
                        // Assign customer role
                        var roleResult = await _userManager.AddToRoleAsync(user, "customer");
                        if (!roleResult.Succeeded)
                        {
                            // Log error jika perlu, tapi tetap lanjutkan proses
                            foreach (var error in roleResult.Errors)
                            {
                                ModelState.AddModelError(string.Empty, $"Error assigning role: {error.Description}");
                            }
                        }

                        // Create customer record
                        var customer = new FinalProject.BO.Models.Customer
                        {
                            Name = model.Name,
                            Email = model.Email,
                            PhoneNumber = model.PhoneNumber,
                            Address = model.Address
                        };

                        _context.Customers.Add(customer);
                        await _context.SaveChangesAsync();

                        // Redirect to login after successful registration
                        TempData["SuccessMessage"] = "Registration successful. Please log in.";
                        return RedirectToAction("Login");
                    }
                    catch (Exception ex)
                    {
                        // Jika ada error saat membuat record customer, hapus user yang sudah dibuat
                        await _userManager.DeleteAsync(user);
                        ModelState.AddModelError(string.Empty, $"Registration failed: {ex.Message}");
                        return View(model);
                    }
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}