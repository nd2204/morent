using System.Collections;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Morent.Core.Entities;
using Morent.Infrastructure.Data;

namespace Morent.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
// [Authorize (Roles = "Admin")]
public class CarsController : ControllerBase
{
    private readonly MorentDbContext _context;
    private readonly ILogger<CarsController> _logger;

    public CarsController(MorentDbContext context, ILogger<CarsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    // GET: /api/cars
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MorentCar>>> GetCars()
    {
        return await _context.Cars
            .Include(c => c.CarModel)
            .ToListAsync();
    }


    // GET: /api/cars/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<MorentCar>> GetCar(int id)
    {
        var car = await _context.Cars
            .Include(c => c.CarModel)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (car == null)
        {
            return NotFound();
        }

        return car;
    }

    // POST: /api/cars
    [HttpPost]
    public async Task<ActionResult<MorentCar>> CreateCar(MorentCar car)
    {
        _context.Cars.Add(car);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetCar), new { id = car.Id }, car);
    }

    // PUT: /api/cars/{id}
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateCar(int id, MorentCar car)
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

    private async Task<bool> CarExists(int id)
    {
        return await _context.Cars.AnyAsync(e => e.Id == id);
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

}
