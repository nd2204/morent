// using Ardalis.Result.AspNetCore;
// using MediatR;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;
// using Morent.Application.Features.Car.Commands;

// namespace Morent.WebApi.Controllers;

// [Route("api/admin")]
// [Authorize(Roles = "Admin")]
// [ApiController]
// public class AdminController(IMediator mediator) : ControllerBase
// {
//     private readonly IMediator _mediator = mediator;

//     [HttpPost("car")]
//     public async Task<IActionResult> CreateCar([FromBody] CreateCarCommand request)
//     {
//         var result = await _mediator.Send(request);
//         return Ok(result);
//     }

//     [HttpPut("car/{id:guid}")]
//     public async Task<IActionResult> UpdateCar(Guid id, [FromBody] UpdateCarCommand request)
//     {
//         var result = await _mediator.Send(new UpdateCarCommand());
//         return Ok(result);
//     }

//     [HttpDelete("car/{id:guid}")]
//     public async Task<IActionResult> DeleteCar(Guid id)
//     {
//         var result = await _mediator.Send(new DeleteCarCommand(id));
//         return Ok(result);
//     }

//     [HttpPost("car/{carId:guid}/images")]
//     [ProducesResponseType(StatusCodes.Status201Created)]
//     [ProducesResponseType(StatusCodes.Status400BadRequest)]
//     [ProducesResponseType(StatusCodes.Status401Unauthorized)]
//     [ProducesResponseType(StatusCodes.Status403Forbidden)]
//     public async Task<ActionResult<CarImageDto>> UploadCarImage(
//         Guid carId,
//         [FromForm] UploadCarImageRequest request)
//     {
//         if (request.Image == null || request.Image.Length == 0)
//         {
//             return BadRequest("No image file provided");
//         }

//         var result = await _mediator.Send(
//           new UploadCarImageCommand(
//             carId, request.Image, request.ImageUrl, request.SetAsPrimary));

//         return this.ToActionResult(result);
//     }

//     [HttpPost("car/{carId:guid}/images/{imageId:guid}")]
//     [ProducesResponseType(StatusCodes.Status204NoContent)]
//     [ProducesResponseType(StatusCodes.Status404NotFound)]
//     [ProducesResponseType(StatusCodes.Status401Unauthorized)]
//     [ProducesResponseType(StatusCodes.Status403Forbidden)]
//     public async Task<IActionResult> DeleteCarImage(Guid carId, Guid imageId)
//     {
//         var result = await _mediator.Send(new DeleteCarImageCommand(carId, imageId));
//         return this.ToActionResult(result);
//     }

//     [HttpPut("car/{carId:guid}/images/{imageId:guid}/set-primary")]
//     [ProducesResponseType(StatusCodes.Status200OK)]
//     [ProducesResponseType(StatusCodes.Status404NotFound)]
//     [ProducesResponseType(StatusCodes.Status401Unauthorized)]
//     [ProducesResponseType(StatusCodes.Status403Forbidden)]
//     public async Task<ActionResult<CarImageDto>> SetPrimaryImage(Guid carId, Guid imageId)
//     {
//         var result = await _mediator.Send(new SetCarPrimaryImageCommand(carId, imageId));
//         return this.ToActionResult(result);
//     }

//     [HttpPut("car/{carId:guid}/images/reorder")]
//     [ProducesResponseType(StatusCodes.Status200OK)]
//     [ProducesResponseType(StatusCodes.Status400BadRequest)]
//     [ProducesResponseType(StatusCodes.Status401Unauthorized)]
//     [ProducesResponseType(StatusCodes.Status403Forbidden)]
//     public async Task<IActionResult> ReorderCarImages(
//         Guid carId,
//         [FromBody] List<CarImageOrderItem> newOrder)
//     {
//         var result = await _mediator.Send(new ReorderCarImagesCommand(carId, newOrder));
//         return this.ToActionResult(result);
//     }
// }