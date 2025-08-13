using FinalProject.BO.Models;
using FinalProject.DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FinalProject.MVC.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly FinalProjectContext _context;

        public OrderController(FinalProjectContext context)
        {
            _context = context;
        }

        // GET: Order
        public async Task<IActionResult> Index()
        {
            // Get the current user's customer ID from claims
            var customerIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(customerIdClaim, out int customerId))
            {
                // Get all LOIs for this customer
                var orders = await _context.LetterOfIntents
                    .Include(l => l.Customer)
                    .Include(l => l.Dealer)
                    .Include(l => l.LetterOfIntentDetails)
                        .ThenInclude(d => d.Car)
                    .Where(l => l.CustomerId == customerId)
                    .OrderByDescending(l => l.Loidate)
                    .ToListAsync();

                return View(orders);
            }

            // If we can't get customer ID, return empty list
            return View(new List<LetterOfIntent>());
        }

        // GET: Order/Create
        public async Task<IActionResult> Create()
        {
            // Get available cars from dealer inventory
            var cars = await _context.DealerInventories
                .Include(di => di.Car)
                .Include(di => di.Dealer)
                .Where(di => di.Car != null)
                .Select(di => new
                {
                    CarId = di.CarId,
                    Model = di.Car.Model,
                    CarType = di.Car.CarType,
                    Price = di.Price,
                    DealerName = di.Dealer.Name
                })
                .ToListAsync();

            ViewBag.Cars = cars;
            return View();
        }

        // POST: Order/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int carId, decimal agreedPrice, string note)
        {
            // Get the current user's customer ID from claims
            var customerIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(customerIdClaim, out int customerId))
            {
                return NotFound("Customer not found.");
            }

            // Get customer details
            var customer = await _context.Customers.FindAsync(customerId);
            if (customer == null)
            {
                return NotFound("Customer not found.");
            }

            // Get car details
            var dealerInventory = await _context.DealerInventories
                .Include(di => di.Car)
                .Include(di => di.Dealer)
                .FirstOrDefaultAsync(di => di.CarId == carId);

            if (dealerInventory == null)
            {
                ModelState.AddModelError("", "Selected car is not available.");
                return await Create(); // Return to create view with error
            }

            // For simplicity, we'll use the first dealer and sales person
            // In a real application, you might want to let the user select these
            var dealer = await _context.Dealers.FirstOrDefaultAsync();
            var salesPerson = await _context.SalesPeople.FirstOrDefaultAsync();

            if (dealer == null || salesPerson == null)
            {
                ModelState.AddModelError("", "Unable to process order. Please contact support.");
                return await Create(); // Return to create view with error
            }

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Create Letter of Intent
                var loi = new LetterOfIntent
                {
                    CustomerId = customerId,
                    DealerId = dealer.DealerId,
                    SalesPersonId = salesPerson.SalesPersonId,
                    Loidate = DateTime.Now,
                    PaymentMethod = "Cash", // Default for now
                    Note = note
                };

                _context.LetterOfIntents.Add(loi);
                await _context.SaveChangesAsync();

                // Create LOI Detail
                var loiDetail = new LetterOfIntentDetail
                {
                    Loiid = loi.Loiid,
                    CarId = carId,
                    AgreedPrice = agreedPrice,
                    Discount = 0, // No discount for now
                    DownPayment = 0 // No down payment for now
                };

                _context.LetterOfIntentDetails.Add(loiDetail);
                await _context.SaveChangesAsync();

                // Create Booking
                var booking = new Booking
                {
                    Loiid = loi.Loiid,
                    BookingFee = 1000000, // Default booking fee
                    BookingDate = DateTime.Now,
                    Status = "Pending"
                };

                _context.Bookings.Add(booking);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                TempData["SuccessMessage"] = "Order created successfully!";
                return RedirectToAction(nameof(Details), new { id = loi.Loiid });
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                ModelState.AddModelError("", "An error occurred while processing your order. Please try again.");
                return await Create(); // Return to create view with error
            }
        }

        // GET: Order/Details/5
        public async Task<IActionResult> Details(int id)
        {
            // Get the current user's customer ID from claims
            var customerIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(customerIdClaim, out int customerId))
            {
                return NotFound("Customer not found.");
            }

            var loi = await _context.LetterOfIntents
                .Include(l => l.Customer)
                .Include(l => l.Dealer)
                .Include(l => l.SalesPerson)
                .Include(l => l.LetterOfIntentDetails)
                    .ThenInclude(d => d.Car)
                .Include(l => l.Bookings)
                .FirstOrDefaultAsync(l => l.Loiid == id && l.CustomerId == customerId);

            if (loi == null)
            {
                return NotFound();
            }

            return View(loi);
        }
    }
}