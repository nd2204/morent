namespace Morent.WebApi;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Morent.Application.Features.Car.DTOs;
using Morent.Application.Features.Images.DTOs;
using Morent.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[ApiController]
[Route("api/cars/{carId}/images")]
public class CarImagesController : ControllerBase
{
  private readonly ICarImageService _carImageService;

  public CarImagesController(ICarImageService carImageService)
  {
    _carImageService = carImageService;
  }

  [HttpGet]
  [AllowAnonymous]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public async Task<ActionResult<IEnumerable<CarImageDto>>> GetCarImages(Guid carId)
  {
    var images = await _carImageService.GetCarImagesAsync(carId);

    if (!images.Any())
    {
      return NotFound();
    }

    return Ok(images);
  }

  [HttpPost]
  [Authorize(Roles = "Admin")]
  [ProducesResponseType(StatusCodes.Status201Created)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  [ProducesResponseType(StatusCodes.Status403Forbidden)]
  public async Task<ActionResult<CarImageDto>> UploadCarImage(
      Guid carId,
      [FromForm] UploadCarImageRequest request)
  {
    if (request.Image == null || request.Image.Length == 0)
    {
      return BadRequest("No image file provided");
    }

    try
    {
      using var stream = request.Image.OpenReadStream();
      var uploadRequest = new ImageUploadRequest
      {
        ImageData = stream,
        FileName = request.Image.FileName,
        ContentType = request.Image.ContentType
      };

      var result = await _carImageService.AddCarImageAsync(carId, request);

      return CreatedAtAction(
          nameof(GetCarImages),
          new { carId = carId },
          result);
    }
    catch (Exception ex)
    {
      return BadRequest(new { error = ex.Message });
    }
  }

  [HttpDelete("{imageId}")]
  [Authorize(Roles = "Admin")]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  [ProducesResponseType(StatusCodes.Status403Forbidden)]
  public async Task<IActionResult> DeleteCarImage(Guid carId, Guid imageId)
  {
    var result = await _carImageService.DeleteCarImageAsync(carId, imageId);

    if (!result)
    {
      return NotFound();
    }

    return NoContent();
  }

  [HttpPut("{imageId}/set-primary")]
  [Authorize(Roles = "Admin")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  [ProducesResponseType(StatusCodes.Status403Forbidden)]
  public async Task<ActionResult<CarImageDto>> SetPrimaryImage(Guid carId, Guid imageId)
  {
    try
    {
      var result = await _carImageService.SetPrimaryImageAsync(carId, imageId);
      return Ok(result);
    }
    catch (Exception ex)
    {
      return NotFound(new { error = ex.Message });
    }
  }

  [HttpPut("reorder")]
  [Authorize(Roles = "Admin,Manager")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  [ProducesResponseType(StatusCodes.Status403Forbidden)]
  public async Task<IActionResult> ReorderCarImages(
      Guid carId,
      [FromBody] List<CarImageOrderItem> newOrder)
  {
    if (newOrder == null || !newOrder.Any())
    {
      return BadRequest("No reordering information provided");
    }

    var result = await _carImageService.ReorderCarImagesAsync(carId, newOrder);

    if (!result)
    {
      return BadRequest("Failed to reorder images");
    }

    return Ok();
  }
}
