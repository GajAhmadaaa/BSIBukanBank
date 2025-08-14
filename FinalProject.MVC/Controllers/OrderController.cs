using FinalProject.BL.Interfaces;
using FinalProject.BL.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FinalProject.MVC.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly ILetterOfIntentBL _letterOfIntentBL;
        private readonly ICustomerBL _customerBL;
        private readonly IDealerInventoryBL _dealerInventoryBL;
        private readonly IDealerBL _dealerBL;
        private readonly ISalesPersonBL _salesPersonBL;
        private readonly ICarBL _carBL; // Add ICarBL to get car details

        public OrderController(
            ILetterOfIntentBL letterOfIntentBL,
            ICustomerBL customerBL,
            IDealerInventoryBL dealerInventoryBL,
            IDealerBL dealerBL,
            ISalesPersonBL salesPersonBL,
            ICarBL carBL) // Inject ICarBL
        {
            _letterOfIntentBL = letterOfIntentBL;
            _customerBL = customerBL;
            _dealerInventoryBL = dealerInventoryBL;
            _dealerBL = dealerBL;
            _salesPersonBL = salesPersonBL;
            _carBL = carBL;
        }

        // GET: Order
        public async Task<IActionResult> Index()
        {
            // Get the current user's customer ID from claims
            var customerIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(customerIdClaim, out int customerId))
            {
                // Get all LOIs for this customer
                // Note: We might need to filter on the BL side or get all and filter here
                // For now, let's assume BL has a method to get LOIs by customer ID
                // If not, we'll need to adjust this
                var orders = await _letterOfIntentBL.GetAllAsync();
                var customerOrders = orders.Where(o => o.CustomerId == customerId).OrderByDescending(o => o.Loidate);

                return View(customerOrders);
            }

            // If we can't get customer ID, return empty list
            return View(new List<LetterOfIntentViewDTO>());
        }

        // GET: Order/Create
        public async Task<IActionResult> Create()
        {
            // Get available cars from dealer inventory
            // We need to get car details separately since DealerInventoryViewDTO doesn't have Car navigation property
            var dealerInventories = await _dealerInventoryBL.GetAllAsync();
            
            // Get all cars to map CarId to Car details
            var cars = await _carBL.GetAllCars();
            var carDict = cars.ToDictionary(c => c.CarId, c => c);
            
            // Get all dealers to map DealerId to Dealer name
            var dealers = await _dealerBL.GetAllDealers();
            var dealerDict = dealers.ToDictionary(d => d.DealerId, d => d);

            var carsWithDealerInfo = dealerInventories
                .Where(di => carDict.ContainsKey(di.CarId)) // Ensure car exists
                .Select(di => new
                {
                    CarId = di.CarId,
                    Model = carDict[di.CarId]?.Model,
                    CarType = carDict[di.CarId]?.CarType,
                    Price = di.Price,
                    DealerName = dealerDict.ContainsKey(di.DealerId) ? dealerDict[di.DealerId]?.Name : "Unknown Dealer"
                })
                .ToList();

            ViewBag.Cars = carsWithDealerInfo;
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

            // Get customer details (optional, if needed for validation)
            var customer = await _customerBL.GetCustomerById(customerId);
            if (customer == null)
            {
                return NotFound("Customer not found.");
            }

            // Get car details to validate carId
            var car = await _carBL.GetCarById(carId);
            if (car == null)
            {
                ModelState.AddModelError("", "Selected car is not available.");
                return await Create(); // Return to create view with error
            }

            // For simplicity, we'll use the first dealer and sales person
            // In a real application, you might want to let the user select these
            // or have a default dealer/sales person associated with the inventory
            var dealers = await _dealerBL.GetAllDealers();
            var salesPeople = await _salesPersonBL.GetAllAsync(); // Use correct method name
            
            if (!dealers.Any() || !salesPeople.Any())
            {
                ModelState.AddModelError("", "Unable to process order. Please contact support.");
                return await Create(); // Return to create view with error
            }
            
            var dealer = dealers.First();
            var salesPerson = salesPeople.First();

            try
            {
                // Create Letter of Intent with details
                var loiWithDetailsDTO = new LetterOfIntentWithDetailsInsertDTO
                {
                    CustomerId = customerId,
                    DealerId = dealer.DealerId, // Use DealerId from DealerViewDTO
                    SalesPersonId = salesPerson.Id, // Use Id from SalesPersonViewDTO
                    Loidate = DateTime.Now,
                    PaymentMethod = "Cash", // Default for now
                    Note = note,
                    Details = new List<LetterOfIntentDetailInsertDTO> // Use Details property
                    {
                        new LetterOfIntentDetailInsertDTO
                        {
                            CarId = carId,
                            Price = agreedPrice, // Use Price property
                            Discount = 0, // No discount for now
                            Note = "" // Empty note for detail
                        }
                    }
                };

                var createdLOI = await _letterOfIntentBL.CreateWithDetailsAsync(loiWithDetailsDTO);

                // Note: Booking creation logic might be handled in BL as well
                // For now, we assume it's handled by the BL when creating LOI with details
                // If not, we would need to call a separate service for Booking

                TempData["SuccessMessage"] = "Order created successfully!";
                // Use Id from LetterOfIntentViewDTO
                return RedirectToAction(nameof(Details), new { id = createdLOI.Id });
            }
            catch (Exception)
            {
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

            var loi = await _letterOfIntentBL.GetByIdAsync(id);
            
            // Check if the LOI belongs to the current customer
            if (loi == null || loi.CustomerId != customerId)
            {
                return NotFound();
            }

            return View(loi);
        }
    }
}