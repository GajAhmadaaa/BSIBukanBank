using FinalProject.BL.Interfaces;
using FinalProject.BO.Models;
using FinalProject.MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.MVC.Controllers
{
    public class CustomerController : Controller
    {
        //private readonly ICustomerService _customerService;

        //public CustomerController(ICustomerService customerService)
        //{
        //    _customerService = customerService;
        //}

        public IActionResult Index()
        {
            // Dummy data untuk sementara
            var customers = new List<CustomerViewModel>
            {
                new CustomerViewModel 
                { 
                    CustomerId = 1, 
                    Name = "Budi Santoso", 
                    Email = "budi.santoso@email.com", 
                    PhoneNumber = "08123456789", 
                    Address = "Jl. Merdeka No. 123, Jakarta" 
                },
                new CustomerViewModel 
                { 
                    CustomerId = 2, 
                    Name = "Siti Rahmawati", 
                    Email = "siti.rahma@email.com", 
                    PhoneNumber = "08567891234", 
                    Address = "Jl. Sudirman No. 456, Bandung" 
                }
            };

            return View(customers);
        }

        public IActionResult Details(int id)
        {
            // Dummy data untuk sementara
            var customer = new CustomerViewModel
            {
                CustomerId = id,
                Name = "Budi Santoso",
                Email = "budi.santoso@email.com",
                PhoneNumber = "08123456789",
                Address = "Jl. Merdeka No. 123, Jakarta"
            };

            return View(customer);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CustomerViewModel customerViewModel)
        {
            if (ModelState.IsValid)
            {
                // Simpan data customer
                // _customerService.Create(customer);
                
                TempData["SuccessMessage"] = "Customer berhasil ditambahkan!";
                return RedirectToAction(nameof(Index));
            }

            return View(customerViewModel);
        }

        public IActionResult Edit(int id)
        {
            // Dummy data untuk sementara
            var customer = new CustomerViewModel
            {
                CustomerId = id,
                Name = "Budi Santoso",
                Email = "budi.santoso@email.com",
                PhoneNumber = "08123456789",
                Address = "Jl. Merdeka No. 123, Jakarta"
            };

            return View(customer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, CustomerViewModel customerViewModel)
        {
            if (ModelState.IsValid)
            {
                // Update data customer
                // _customerService.Update(customer);
                
                TempData["SuccessMessage"] = "Customer berhasil diperbarui!";
                return RedirectToAction(nameof(Index));
            }

            return View(customerViewModel);
        }

        public IActionResult Delete(int id)
        {
            // Dummy data untuk sementara
            var customer = new CustomerViewModel
            {
                CustomerId = id,
                Name = "Budi Santoso",
                Email = "budi.santoso@email.com",
                PhoneNumber = "08123456789",
                Address = "Jl. Merdeka No. 123, Jakarta"
            };

            return View(customer);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            // Hapus data customer
            // _customerService.Delete(id);
            
            TempData["SuccessMessage"] = "Customer berhasil dihapus!";
            return RedirectToAction(nameof(Index));
        }
    }
}