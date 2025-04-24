using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Morent.Application.Features.Car.DTOs;

/// <summary>
/// Allow uploading by form/multipart or from external ImageUrl
/// </summary>
public class UploadCarImageRequest
{
  public IFormFile Image { get; set; } = null!;
  public string ImageUrl { get; set; } = null!;
  public bool SetAsPrimary { get; set; }
}