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
        private readonly ISalesAgreementBL _salesAgreementBL;
        private readonly IDealerInventoryBL _dealerInventoryBL;
        private readonly IDealerBL _dealerBL;
        private readonly ICarBL _carBL;

        public OrderController(
            ILetterOfIntentBL letterOfIntentBL,
            ISalesAgreementBL salesAgreementBL,
            IDealerInventoryBL dealerInventoryBL,
            IDealerBL dealerBL,
            ICarBL carBL)
        {
            _letterOfIntentBL = letterOfIntentBL;
            _salesAgreementBL = salesAgreementBL;
            _dealerInventoryBL = dealerInventoryBL;
            _dealerBL = dealerBL;
            _carBL = carBL;
        }

        // GET: Order
        public async Task<IActionResult> Index()
        {
            var customerIdClaim = User.FindFirst("CustomerId")?.Value;
            
            if (string.IsNullOrEmpty(customerIdClaim) || !int.TryParse(customerIdClaim, out int customerId))
            {
                return NotFound("Customer not found.");
            }
            
            // Get LOI with status "Pending" and "ReadyForAgreement"
            var pendingLois = await _letterOfIntentBL.GetPendingByCustomerIdAsync(customerId);
            
            // Get Sales Agreements with status "Unpaid"
            var unpaidAgreements = await _salesAgreementBL.GetUnpaidByCustomerIdAsync(customerId);
            
            // Get Sales Agreements with status "Paid" and "Completed"
            var paidAgreements = await _salesAgreementBL.GetPaidByCustomerIdAsync(customerId);
            
            var model = new OrderIndexViewModel
            {
                PendingLois = pendingLois.ToList(),
                UnpaidAgreements = unpaidAgreements.ToList(),
                PaidAgreements = paidAgreements.ToList()
            };
            
            return View(model);
        }

        // GET: Order/Create
        public async Task<IActionResult> Create()
        {
            var customerIdClaim = User.FindFirst("CustomerId")?.Value;
            
            if (string.IsNullOrEmpty(customerIdClaim) || !int.TryParse(customerIdClaim, out _))
            {
                return NotFound("Customer not found.");
            }
            
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
            
            if (string.IsNullOrEmpty(customerIdClaim) || !int.TryParse(customerIdClaim, out int customerId))
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
                var selectedCar = await _carBL.GetCarById(model.CarId);
                if (selectedCar == null)
                {
                    ModelState.AddModelError("CarId", "Selected car is not valid.");
                    await PopulateDropdowns();
                    return View(model);
                }

                try
                {
                    // Get dealer price, fallback to base price if not available
                    var dealerInventory = await _dealerInventoryBL.GetByDealerAndCarAsync(model.DealerId, model.CarId);
                    var price = dealerInventory?.Price ?? selectedCar.BasePrice;

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
                        Status = "Pending", // Set default status
                        Details = loiDetails
                    };

                    var createdLOI = await _letterOfIntentBL.CreateWithDetailsAsync(loiWithDetailsDTO);

                    TempData["SuccessMessage"] = "Order created successfully!";
                    return RedirectToAction(nameof(Details), new { id = createdLOI.Id });
                }
                catch (Exception)
                {
                    throw;
                }
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
            
            if (string.IsNullOrEmpty(customerIdClaim) || !int.TryParse(customerIdClaim, out int customerId))
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
        
        // GET: Order/AgreementDetails/5
        public async Task<IActionResult> AgreementDetails(int id)
        {
            var customerIdClaim = User.FindFirst("CustomerId")?.Value;
            
            if (string.IsNullOrEmpty(customerIdClaim) || !int.TryParse(customerIdClaim, out int customerId))
            {
                return NotFound("Customer not found.");
            }

            var agreement = await _salesAgreementBL.GetByIdAsync(id);
            
            if (agreement == null || agreement.CustomerId != customerId)
            {
                return NotFound();
            }

            return View(agreement);
        }

        private async Task PopulateDropdowns()
        {
            ViewBag.Dealers = new SelectList(await _dealerBL.GetAllDealers(), "DealerId", "Name");
            ViewBag.Cars = new SelectList(await _carBL.GetAllCars(), "CarId", "Model");
        }
    }
}
