using FinalProject.BL.DTO;
using FinalProject.BL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CarController : ControllerBase
    {
        private readonly ICarBL _carBL;

        public CarController(ICarBL carBL)
        {
            _carBL = carBL;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CarViewDTO>>> GetCars()
        {
            var cars = await _carBL.GetAllCars();
            return Ok(cars);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CarViewDTO>> GetCarById(int id)
        {
            var car = await _carBL.GetCarById(id);
            if (car == null)
            {
                return NotFound();
            }
            return Ok(car);
        }

        [HttpGet("BySearch")]
        public async Task<ActionResult<IEnumerable<CarViewDTO>>> GetCarsBySearch(string search)
        {
            if (string.IsNullOrWhiteSpace(search))
            {
                return BadRequest("Search term cannot be empty.");
            }

            var cars = await _carBL.GetCarsBySearch(search);
            return Ok(cars);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<ActionResult<CarViewDTO>> AddCar(CarInsertDTO carInsertDto)
        {
            try
            {
                if (carInsertDto == null)
                {
                    return BadRequest("Car data is null.");
                }
                var addedCar = await _carBL.CreateCar(carInsertDto);
                return CreatedAtAction(nameof(GetCarById), new { id = addedCar.CarId }, addedCar);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult<CarViewDTO>> UpdateCar(int id, CarUpdateDTO carUpdateDto)
        {
            if (id != carUpdateDto.CarId)
            {
                return BadRequest("Car ID mismatch.");
            }

            try
            {
                var updatedCar = await _carBL.UpdateCar(carUpdateDto);
                if (updatedCar == null)
                {
                    return NotFound("Car not found.");
                }
                return Ok(updatedCar);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCar(int id)
        {
            try
            {
                var car = await _carBL.GetCarById(id);
                if (car == null)
                {
                    return NotFound("Car not found.");
                }

                await _carBL.DeleteCar(id);
                return Ok($"Car id: {id} deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}