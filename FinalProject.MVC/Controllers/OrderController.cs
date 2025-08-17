using FinalProject.BL.Interfaces;
using FinalProject.BL.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using FinalProject.MVC.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FinalProject.MVC.Controllers
{
    [Authorize(Roles = "customer")]
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
            IDealerBL dealerBL,
            ISalesPersonBL salesPersonBL,
            ICarBL carBL)
        {
            _letterOfIntentBL = letterOfIntentBL;
            _dealerBL = dealerBL;
            _salesPersonBL = salesPersonBL;
            _carBL = carBL;
        }

        // GET: Order
        public async Task<IActionResult> Index()
        {
            var customerIdClaim = User.FindFirst("CustomerId")?.Value;
            if (int.TryParse(customerIdClaim, out int customerId))
            {
                var orders = await _letterOfIntentBL.GetAllAsync();
                var customerOrders = orders.Where(o => o.CustomerId == customerId).OrderByDescending(o => o.Loidate);
                return View(customerOrders);
            }
            return View(new List<LetterOfIntentViewDTO>());
        }

        // GET: Order/Create
        public async Task<IActionResult> Create()
        {
            await PopulateDropdowns();
            return View(new SimplifiedOrderViewModel());
        }

        // POST: Order/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SimplifiedOrderViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropdowns();
                return View(model);
            }

            var customerIdClaim = User.FindFirst("CustomerId")?.Value;
            if (!int.TryParse(customerIdClaim, out int customerId))
            {
                return NotFound("Customer not found.");
            }
            
            var car = await _carBL.GetCarById(model.CarId);
            if (car == null)
            {
                ModelState.AddModelError("CarId", "Selected car is not valid.");
                await PopulateDropdowns();
                return View(model);
            }

            try
            {
                // Get dealer price, fallback to base price if not available
                var dealerInventory = await _dealerInventoryBL.GetByDealerAndCarAsync(model.DealerId, model.CarId);
                var price = dealerInventory?.Price ?? car.BasePrice;

                var loiDetails = new List<LetterOfIntentDetailInsertDTO>();
                for (int i = 0; i < model.Quantity; i++)
                {
                    loiDetails.Add(new LetterOfIntentDetailInsertDTO
                    {
                        CarId = model.CarId,
                        Price = price, // Use dealer price or base price
                        Discount = 0,
                        Note = ""
                    });
                }

                var loiWithDetailsDTO = new LetterOfIntentWithDetailsInsertDTO
                {
                    CustomerId = customerId,
                    DealerId = model.DealerId,
                    // Set SalesPersonId to null as requested
                    SalesPersonId = null,
                    Loidate = DateTime.Now,
                    PaymentMethod = "Cash", // Default
                    Note = model.Note,
                    Details = loiDetails
                };

                var createdLOI = await _letterOfIntentBL.CreateWithDetailsAsync(loiWithDetailsDTO);

                TempData["SuccessMessage"] = "Order created successfully!";
                return RedirectToAction(nameof(Details), new { id = createdLOI.Id });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"An error occurred while processing your order: {ex.Message}");
                await PopulateDropdowns();
                return View(model);
            }
        }

        // GET: Order/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var customerIdClaim = User.FindFirst("CustomerId")?.Value;
            if (!int.TryParse(customerIdClaim, out int customerId))
            {
                return NotFound("Customer not found.");
            }

            var loi = await _letterOfIntentBL.GetByIdAsync(id);
            
            if (loi == null || loi.CustomerId != customerId)
            {
                return NotFound();
            }

            return View(loi);
        }

        private async Task PopulateDropdowns()
        {
            ViewBag.Dealers = new SelectList(await _dealerBL.GetAllDealers(), "DealerId", "Name");
            ViewBag.Cars = new SelectList(await _carBL.GetAllCars(), "CarId", "Model");
        }
    }
}
