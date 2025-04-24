using System;
using System.ComponentModel.DataAnnotations;

namespace Morent.Application.DTOs;

// Auth DTOs
public class LoginRequest
{
  [Required]
  public string LoginId { get; set; } = null!;
  [Required]
  public string Password { get; set; } = null!;
}