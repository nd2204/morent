using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Morent.Api.Helpers;
using Morent.Core.Entities;
using Morent.Core.Interfaces;

namespace Morent.Api.Controllers;
using CarModelIdType = int;

[Route("api/car-models")]
[ApiController]
public class CarModelsController : ControllerBase
{
    private readonly IMorentDbContext _context;
    private readonly ILogger<CarModelsController> _logger;

    public CarModelsController(IMorentDbContext context, ILogger<CarModelsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    // GET: /api/car-models
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MorentCarModel>>> GetCarModels()
    {
        return await _context.CarModels
            .ToListAsync();
    }

    // GET: /api/car-models/{carModelId}/images
    [HttpGet("{carModelId}/images")]
    public async Task<ActionResult<IEnumerable<MorentImage>>> GetCarModelImages(CarModelIdType carModelId)
    {
        return await _context.Images
            .ToListAsync();
    }

    // GET: /api/car-models/{carModelId}/images/{imageId}
    [HttpGet("{carModelId}/images/{imageId}")]
    public async Task<ActionResult<MorentImage>> GetCarModelImage(CarModelIdType carModelId, Guid imageId) 
    {
        var car = await _context.Images
            .FirstOrDefaultAsync(i => i.Id == imageId);

        if (car == null)
        {
            return NotFound();
        }

        return car;
    }

    // POST: /api/car-models/{carId}/images
    // [Authorize(Roles = "Admin")]
    [HttpPost("{carModelId}/images")]
    public async Task<IActionResult> UploadImage(CarModelIdType carModelId, IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded.");

        var car = await _context.CarModels.FindAsync(carModelId);
        if (car == null) return NotFound("Car not found.");

        try
        {
            var (fileName, _) = await FileHelper.SaveImageFileAsync(file);

            var image = new MorentImage
            {
                FileName = fileName,
                Url = $"/images/{fileName}",
                CarModelId = carModelId
            };

            _context.Images.Add(image);
            await _context.SaveChangesAsync();

            return Ok(new { image.Id, image.Url });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error saving image file.");
            return StatusCode(500, "Internal server error while saving the image.");
        }
    }

    private async Task<bool> CarModelImageExists(Guid imageId)
    {
        return await _context.Images.AnyAsync(i => i.Id == imageId);
    }

    private async Task<bool> CarModelExists(int carModelId)
    {
        return await _context.CarModels.AnyAsync(i => i.Id == carModelId);
    }
}