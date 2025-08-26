using FinalProject.BL.DTO;
using FinalProject.BL.Interfaces;
using FinalProject.DAL;
using FinalProject.MVC.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace FinalProject.MVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly FinalProjectContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ICustomerBL _customerBL;
        private readonly IUsmanBL _usmanBL;

        public AccountController(
            FinalProjectContext context,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            ICustomerBL customerBL,
            IUsmanBL usmanBL)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _customerBL = customerBL;
            _usmanBL = usmanBL;
            
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
                            // Only refresh if user is already signed in
                            if (User.Identity.IsAuthenticated)
                            {
                                await _signInManager.RefreshSignInAsync(user);
                            }
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
                // Validasi input tambahan
                if (string.IsNullOrWhiteSpace(model.Name) || 
                    string.IsNullOrWhiteSpace(model.Email) || 
                    string.IsNullOrWhiteSpace(model.Password) ||
                    string.IsNullOrWhiteSpace(model.PhoneNumber) ||
                    string.IsNullOrWhiteSpace(model.Address))
                {
                    ModelState.AddModelError(string.Empty, "All fields are required.");
                    return View(model);
                }

                // Validasi tambahan untuk mencegah karakter tidak valid
                if (!IsValidInput(model.Name) || 
                    !IsValidInput(model.Email) || 
                    !IsValidInput(model.PhoneNumber) ||
                    !IsValidInput(model.Address))
                {
                    ModelState.AddModelError(string.Empty, "Invalid characters in input fields.");
                    return View(model);
                }

                // Validasi format email
                if (!IsValidEmail(model.Email))
                {
                    ModelState.AddModelError("Email", "Invalid email format.");
                    return View(model);
                }

                try
                {
                    // 1. Buat user di ASP.NET Core Identity
                    var userRegistrationDto = new RegistrationDTO
                    {
                        Email = model.Email.Trim(),
                        Password = model.Password,
                        ConfirmPassword = model.Password
                    };
                    
                    var userCreated = await _usmanBL.RegisterAsync(userRegistrationDto);
                    if (!userCreated)
                    {
                        ModelState.AddModelError(string.Empty, "Failed to create user account.");
                        return View(model);
                    }

                    // 2. Assign Role "customer" ke user yang baru dibuat
                    var roleAssigned = await _usmanBL.AddUserToRoleAsync(model.Email.Trim(), "customer");
                    // Note: Kita tidak return error jika assign role gagal, karena user sudah dibuat.
                    // Bisa log error ini jika ada sistem logging.

                    // 3. Simpan data customer ke tabel Customer
                    var customerInsertDto = new CustomerInsertDTO
                    {
                        Name = model.Name.Trim(),
                        Email = model.Email.Trim(),
                        PhoneNumber = model.PhoneNumber.Trim(),
                        Address = model.Address.Trim()
                    };

                    var createdCustomer = await _customerBL.CreateCustomer(customerInsertDto);

                    // Redirect to login after successful registration
                    TempData["SuccessMessage"] = "Registration successful. Please log in.";
                    return RedirectToAction("Login");
                }
                catch (ArgumentException ex)
                {
                    // Tangkap error validasi khusus dari BL/DAL
                    ModelState.AddModelError(string.Empty, ex.Message);
                    return View(model);
                }
                catch (Exception ex)
                {
                    // Tangkap error umum
                    ModelState.AddModelError(string.Empty, $"Registration failed: {ex.Message}");
                    return View(model);
                }
            }

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        // Metode bantuan untuk validasi input
        private bool IsValidInput(string input)
        {
            // Izinkan huruf, angka, spasi, dan beberapa karakter khusus yang umum
            // Karakter yang diizinkan: huruf (a-z, A-Z), angka (0-9), spasi, dan simbol-simbol umum
            var allowedPattern = @"^[a-zA-Z0-9\s\-\.\,\@\#\$\%\&\*\(\)\[\]\{\}\;\:\/\?\!\+\=\~`'""\|]*$";
            return Regex.IsMatch(input, allowedPattern);
        }

        // Metode bantuan untuk validasi email
        private bool IsValidEmail(string email)
        {
            try
            {
                var mailAddress = new MailAddress(email);
                return mailAddress.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}