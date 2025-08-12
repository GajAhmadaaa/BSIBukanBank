using FinalProject.MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.MVC.Controllers
{
    public class TestDriveController : Controller
    {
        public IActionResult Index()
        {
            // Dummy data untuk sementara
            var testDrives = new List<TestDriveViewModel>
            {
                new TestDriveViewModel 
                { 
                    TestDriveId = 1,
                    TestDriveDate = DateTime.Now.AddDays(1),
                    Note = "Customer ingin test drive mobil SUV",
                    DealerName = "Dealer Jakarta",
                    CustomerName = "Budi Santoso",
                    SalesPersonName = "Agus Setiawan",
                    CarModel = "Toyota Fortuner"
                },
                new TestDriveViewModel 
                { 
                    TestDriveId = 2,
                    TestDriveDate = DateTime.Now.AddDays(2),
                    Note = "Customer tertarik dengan mobil hybrid",
                    DealerName = "Dealer Bandung",
                    CustomerName = "Siti Rahmawati",
                    SalesPersonName = "Budi Prasetyo",
                    CarModel = "Toyota Prius"
                }
            };

            return View(testDrives);
        }

        public IActionResult Details(int id)
        {
            // Dummy data untuk sementara
            var testDrive = new TestDriveViewModel
            {
                TestDriveId = id,
                TestDriveDate = DateTime.Now.AddDays(1),
                Note = "Customer ingin test drive mobil SUV",
                DealerName = "Dealer Jakarta",
                CustomerName = "Budi Santoso",
                SalesPersonName = "Agus Setiawan",
                CarModel = "Toyota Fortuner"
            };

            return View(testDrive);
        }

        public IActionResult Create()
        {
            // Dummy data untuk dropdown
            ViewBag.Dealers = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>
            {
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Value = "1", Text = "Dealer Jakarta" },
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Value = "2", Text = "Dealer Bandung" }
            };

            ViewBag.Customers = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>
            {
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Value = "1", Text = "Budi Santoso" },
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Value = "2", Text = "Siti Rahmawati" }
            };

            ViewBag.SalesPersons = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>
            {
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Value = "1", Text = "Agus Setiawan" },
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Value = "2", Text = "Budi Prasetyo" }
            };

            ViewBag.Cars = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>
            {
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Value = "1", Text = "Toyota Fortuner" },
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Value = "2", Text = "Toyota Prius" },
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Value = "3", Text = "Toyota Camry" }
            };

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TestDriveViewModel testDriveViewModel)
        {
            if (ModelState.IsValid)
            {
                // Simpan data test drive
                TempData["SuccessMessage"] = "Test drive berhasil dijadwalkan!";
                return RedirectToAction(nameof(Index));
            }

            // Reload dropdown data jika ada error
            ViewBag.Dealers = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>
            {
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Value = "1", Text = "Dealer Jakarta" },
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Value = "2", Text = "Dealer Bandung" }
            };

            ViewBag.Customers = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>
            {
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Value = "1", Text = "Budi Santoso" },
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Value = "2", Text = "Siti Rahmawati" }
            };

            ViewBag.SalesPersons = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>
            {
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Value = "1", Text = "Agus Setiawan" },
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Value = "2", Text = "Budi Prasetyo" }
            };

            ViewBag.Cars = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>
            {
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Value = "1", Text = "Toyota Fortuner" },
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Value = "2", Text = "Toyota Prius" },
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Value = "3", Text = "Toyota Camry" }
            };

            return View(testDriveViewModel);
        }

        public IActionResult Edit(int id)
        {
            // Dummy data untuk sementara
            var testDrive = new TestDriveViewModel
            {
                TestDriveId = id,
                TestDriveDate = DateTime.Now.AddDays(1),
                Note = "Customer ingin test drive mobil SUV",
                DealerId = 1,
                CustomerId = 1,
                SalesPersonId = 1,
                CarId = 1
            };

            // Dummy data untuk dropdown
            ViewBag.Dealers = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>
            {
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Value = "1", Text = "Dealer Jakarta" },
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Value = "2", Text = "Dealer Bandung" }
            };

            ViewBag.Customers = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>
            {
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Value = "1", Text = "Budi Santoso" },
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Value = "2", Text = "Siti Rahmawati" }
            };

            ViewBag.SalesPersons = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>
            {
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Value = "1", Text = "Agus Setiawan" },
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Value = "2", Text = "Budi Prasetyo" }
            };

            ViewBag.Cars = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>
            {
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Value = "1", Text = "Toyota Fortuner" },
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Value = "2", Text = "Toyota Prius" },
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Value = "3", Text = "Toyota Camry" }
            };

            return View(testDrive);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, TestDriveViewModel testDriveViewModel)
        {
            if (ModelState.IsValid)
            {
                // Update data test drive
                TempData["SuccessMessage"] = "Test drive berhasil diperbarui!";
                return RedirectToAction(nameof(Index));
            }

            // Reload dropdown data jika ada error
            ViewBag.Dealers = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>
            {
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Value = "1", Text = "Dealer Jakarta" },
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Value = "2", Text = "Dealer Bandung" }
            };

            ViewBag.Customers = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>
            {
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Value = "1", Text = "Budi Santoso" },
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Value = "2", Text = "Siti Rahmawati" }
            };

            ViewBag.SalesPersons = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>
            {
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Value = "1", Text = "Agus Setiawan" },
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Value = "2", Text = "Budi Prasetyo" }
            };

            ViewBag.Cars = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>
            {
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Value = "1", Text = "Toyota Fortuner" },
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Value = "2", Text = "Toyota Prius" },
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Value = "3", Text = "Toyota Camry" }
            };

            return View(testDriveViewModel);
        }
    }
}