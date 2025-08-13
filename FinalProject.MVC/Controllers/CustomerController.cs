using FinalProject.BL.Interfaces;
using FinalProject.BO.Models;
using FinalProject.DAL;
using FinalProject.MVC.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.MVC.Controllers
{
    [Authorize]
    public class CustomerController : Controller
    {
        private readonly FinalProjectContext _context;

        public CustomerController(FinalProjectContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var customers = await _context.Customers.ToListAsync();
            var customerViewModels = customers.Select(c => new CustomerViewModel
            {
                CustomerId = c.CustomerId,
                Name = c.Name,
                Email = c.Email,
                PhoneNumber = c.PhoneNumber,
                Address = c.Address
            }).ToList();

            return View(customerViewModels);
        }

        public async Task<IActionResult> Details(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            var customerViewModel = new CustomerViewModel
            {
                CustomerId = customer.CustomerId,
                Name = customer.Name,
                Email = customer.Email,
                PhoneNumber = customer.PhoneNumber,
                Address = customer.Address
            };

            return View(customerViewModel);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CustomerViewModel customerViewModel)
        {
            if (ModelState.IsValid)
            {
                var customer = new Customer
                {
                    Name = customerViewModel.Name,
                    Email = customerViewModel.Email,
                    PhoneNumber = customerViewModel.PhoneNumber,
                    Address = customerViewModel.Address
                };

                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();

                TempData[("SuccessMessage")] = "Customer berhasil ditambahkan!";
                return RedirectToAction(nameof(Index));
            }

            return View(customerViewModel);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            var customerViewModel = new CustomerViewModel
            {
                CustomerId = customer.CustomerId,
                Name = customer.Name,
                Email = customer.Email,
                PhoneNumber = customer.PhoneNumber,
                Address = customer.Address
            };

            return View(customerViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CustomerViewModel customerViewModel)
        {
            if (ModelState.IsValid)
            {
                var customer = await _context.Customers.FindAsync(id);
                if (customer == null)
                {
                    return NotFound();
                }

                customer.Name = customerViewModel.Name;
                customer.Email = customerViewModel.Email;
                customer.PhoneNumber = customerViewModel.PhoneNumber;
                customer.Address = customerViewModel.Address;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                TempData[("SuccessMessage")] = "Customer berhasil diperbarui!";
                return RedirectToAction(nameof(Index));
            }

            return View(customerViewModel);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            var customerViewModel = new CustomerViewModel
            {
                CustomerId = customer.CustomerId,
                Name = customer.Name,
                Email = customer.Email,
                PhoneNumber = customer.PhoneNumber,
                Address = customer.Address
            };

            return View(customerViewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
            }

            await _context.SaveChangesAsync();
            TempData[("SuccessMessage")] = "Customer berhasil dihapus!";
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.CustomerId == id);
        }
    }
}