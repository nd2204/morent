using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Morent.Core.Entities;
using Morent.Core.Interfaces;

namespace Morent.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarsController : ControllerBase
    {
        private readonly IMorentDbContext _context;
        private readonly ILogger<CarsController> _logger;

        public CarsController(IMorentDbContext context, ILogger<CarsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: /api/cars/model
        [HttpGet("model")]
        public async Task<ActionResult<IEnumerable<MorentCarModel>>> GetCarModels()
        {
            return await _context.CarModels.ToListAsync();
        }

        // GET: /api/cars
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MorentCar>>> GetCars()
        {
            return await _context.Cars
                .Include(c => c.CarModel)
                .Include(c => c.Location)
                .ToListAsync();
        }

        // GET: /api/cars/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<MorentCar>> GetCar(int id)
        {
            var car = await _context.Cars
                .Include(c => c.CarModel)
                .Include(c => c.Location)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (car == null)
            {
                return NotFound();
            }

            return car;
        }

        // POST: /api/cars/{id}
        [HttpPost]
        public async Task<ActionResult<MorentCar>> CreateCar(MorentCar car)
        {
            _context.Cars.Add(car);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCar), new { id = car.Id }, car);
        }

        // PUT: /api/cars/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCar(int id, MorentCar car)
        {
            if (id != car.Id)
            {
                return BadRequest();
            }

            try
            {
                _context.Cars.Update(car);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await CarExists(id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        // Delete: /api/cars/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCar(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car == null)
            {
                return NotFound();
            }

            _context.Cars.Remove(car);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: /api/cars/{carId}/images
        // [Authorize(Roles = "Admin")]
        [HttpPost("{id}/images")]
        public async Task<IActionResult> UploadImage(int id, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            var car = await _context.CarModels.FindAsync(id);
            if (car == null) return NotFound("Car not found.");

            var fileName = $"{Guid.NewGuid()}_{file.FileName}";
            var uploadPath = Path.Combine("wwwroot/images", fileName);

            using (var stream = new FileStream(uploadPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var image = new MorentImage
            {
                FileName = fileName,
                Url = $"/images/{fileName}",
                CarModelId = id
            };

            _context.Images.Add(image);
            await _context.SaveChangesAsync();

            return Ok(new { image.Id, image.Url });
        }

        private async Task<bool> CarExists(int id)
        {
            return await _context.Cars.AnyAsync(c => c.Id == id);
        }
    }
}
