using FinalProject.BL.Interfaces;
using FinalProject.BL.DTO;
using FinalProject.MVC.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.MVC.Controllers
{
    [Authorize]
    public class CustomerController : Controller
    {
        private readonly ICustomerBL _customerBL;

        public CustomerController(ICustomerBL customerBL)
        {
            _customerBL = customerBL;
        }

        public async Task<IActionResult> Index()
        {
            var customers = await _customerBL.GetAllCustomers();
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
            var customer = await _customerBL.GetCustomerById(id);
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
                var customerInsertDTO = new CustomerInsertDTO
                {
                    Name = customerViewModel.Name,
                    Email = customerViewModel.Email,
                    PhoneNumber = customerViewModel.PhoneNumber,
                    Address = customerViewModel.Address
                };

                await _customerBL.CreateCustomer(customerInsertDTO);

                TempData["SuccessMessage"] = "Customer berhasil ditambahkan!";
                return RedirectToAction(nameof(Index));
            }

            return View(customerViewModel);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var customer = await _customerBL.GetCustomerById(id);
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
                var customerUpdateDTO = new CustomerUpdateDTO
                {
                    CustomerId = id,
                    Name = customerViewModel.Name,
                    Email = customerViewModel.Email,
                    PhoneNumber = customerViewModel.PhoneNumber,
                    Address = customerViewModel.Address
                };

                var updatedCustomer = await _customerBL.UpdateCustomer(customerUpdateDTO);
                if (updatedCustomer == null)
                {
                    return NotFound();
                }

                TempData["SuccessMessage"] = "Customer berhasil diperbarui!";
                return RedirectToAction(nameof(Index));
            }

            return View(customerViewModel);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var customer = await _customerBL.GetCustomerById(id);
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
            await _customerBL.DeleteCustomer(id);
            TempData["SuccessMessage"] = "Customer berhasil dihapus!";
            return RedirectToAction(nameof(Index));
        }
    }
}