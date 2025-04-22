using System;

namespace Morent.Application.DTOs;

  public class AuthResponse
  {
    public Guid UserId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public UserRole Role { get; set; }
    public string Token { get; set; }
    public DateTime TokenExpiration { get; set; }
    public string RefreshToken { get; set; }
  }