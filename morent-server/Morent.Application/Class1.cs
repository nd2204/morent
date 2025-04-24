// using System.ComponentModel.DataAnnotations;

// public class RegisterUserCommandValidator : IValidator<RegisterUserCommand>
// {
//   public Task<ValidationResult> ValidateAsync(RegisterUserCommand command, CancellationToken cancellationToken = default)
//   {
//     var result = new ValidationResult();

//     if (string.IsNullOrWhiteSpace(command.Name))
//       result.AddError("Name is required");

//     if (string.IsNullOrWhiteSpace(command.Email))
//       result.AddError("Email is required");
//     else if (!IsValidEmail(command.Email))
//       result.AddError("Email format is invalid");

//     if (string.IsNullOrWhiteSpace(command.Password))
//       result.AddError("Password is required");
//     else if (command.Password.Length < 8)
//       result.AddError("Password must be at least 8 characters long");

//     return Task.FromResult(result);
//   }

//   private bool IsValidEmail(string email)
//   {
//     try
//     {
//       var addr = new System.Net.Mail.MailAddress(email);
//       return addr.Address == email;
//     }
//     catch
//     {
//       return false;
//     }
//   }
// }

// public class RentCarCommandValidator : IValidator<RentCarCommand>
// {
//   public Task<ValidationResult> ValidateAsync(RentCarCommand command, CancellationToken cancellationToken = default)
//   {
//     var result = new ValidationResult();

//     if (command.CarId == Guid.Empty)
//       result.AddError("CarId is required");

//     if (command.PickupDate == default)
//       result.AddError("PickupDate is required");

//     if (command.DropoffDate == default)
//       result.AddError("DropoffDate is required");

//     if (command.PickupDate >= command.DropoffDate)
//       result.AddError("PickupDate must be before DropoffDate");

//     if (command.PickupDate.Date < DateTime.UtcNow.Date)
//       result.AddError("PickupDate cannot be in the past");

//     if (command.PickupLocation == null)
//       result.AddError("PickupLocation is required");
//     else
//     {
//       if (string.IsNullOrWhiteSpace(command.PickupLocation.Address))
//         result.AddError("PickupLocation.Address is required");
//       if (string.IsNullOrWhiteSpace(command.PickupLocation.City))
//         result.AddError("PickupLocation.City is required");
//       if (string.IsNullOrWhiteSpace(command.PickupLocation.Country))
//         result.AddError("PickupLocation.Country is required");
//     }

//     if (command.DropoffLocation == null)
//       result.AddError("DropoffLocation is required");
//     else
//     {
//       if (string.IsNullOrWhiteSpace(command.DropoffLocation.Address))
//         result.AddError("DropoffLocation.Address is required");
//       if (string.IsNullOrWhiteSpace(command.DropoffLocation.City))
//         result.AddError("DropoffLocation.City is required");
//       if (string.IsNullOrWhiteSpace(command.DropoffLocation.Country))
//         result.AddError("DropoffLocation.Country is required");
//     }

//     if (string.IsNullOrWhiteSpace(command.PaymentMethodId))
//       result.AddError("PaymentMethodId is required");

//     return Task.FromResult(result);
//   }
// }

// public class LeaveReviewCommandValidator : IValidator<LeaveReviewCommand>
// {
//   public Task<ValidationResult> ValidateAsync(LeaveReviewCommand command, CancellationToken cancellationToken = default)
//   {
//     var result = new ValidationResult();

//     if (command.CarId == Guid.Empty)
//       result.AddError("CarId is required");

//     if (command.RentalId == Guid.Empty)
//       result.AddError("RentalId is required");

//     if (command.Rating < 1 || command.Rating > 5)
//       result.AddError("Rating must be between 1 and 5");

//     if (string.IsNullOrWhiteSpace(command.Comment))
//       result.AddError("Comment is required");

//     return Task.FromResult(result);
//   }
// }

// // Additional validators would follow the same pattern

// public class ValidationResult
// {
//   private readonly List<string> _errors = new List<string>();

//   public IReadOnlyCollection<string> Errors => _errors.AsReadOnly();

//   public bool IsValid => !_errors.Any();

//   public void AddError(string error)
//   {
//     _errors.Add(error);
//   }
// }


// // Infrastructure/Persistence/CarRentalDbContext.cs
// using CarRentalService.Core.Domain.Entities;
// using Microsoft.EntityFrameworkCore;

// namespace CarRentalService.Infrastructure.Persistence
// {
//   public class CarRentalDbContext : DbContext
//   {
//     public CarRentalDbContext(DbContextOptions<CarRentalDbContext> options) : base(options)
//     {
//     }

//     public DbSet<User> Users { get; set; }
//     public DbSet<Car> Cars { get; set; }
//     public DbSet<Rental> Rentals { get; set; }
//     public DbSet<Review> Reviews { get; set; }

//     protected override void OnModelCreating(ModelBuilder modelBuilder)
//     {
//       // Configure User entity
//       modelBuilder.Entity<User>(entity =>
//       {
//         entity.HasKey(e => e.Id);
//         entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
//         entity.Property(e => e.PasswordHash).HasMaxLength(256);
//         entity.Property(e => e.OAuthProvider).HasMaxLength(50);
//         entity.Property(e => e.OAuthId).HasMaxLength(100);
//         entity.Property(e => e.Role).IsRequired().HasMaxLength(20);
//         entity.Property(e => e.ProfileImageUrl).HasMaxLength(500);

//         // Configure Email Value Object
//         entity.OwnsOne(u => u.Email, email =>
//               {
//             email.Property(e => e.Value)
//                        .HasColumnName("Email")
//                        .IsRequired()
//                        .HasMaxLength(256);
//           });
//       });

//       // Configure Car entity
//       modelBuilder.Entity<Car>(entity =>
//       {
//         entity.HasKey(e => e.Id);
//         entity.Property(e => e.Brand).IsRequired().HasMaxLength(50);
//         entity.Property(e => e.Model).IsRequired().HasMaxLength(100);
//         entity.Property(e => e.FuelType).IsRequired().HasMaxLength(30);
//         entity.Property(e => e.Capacity).IsRequired();

//         // Configure Money Value Object
//         entity.OwnsOne(c => c.PricePerDay, price =>
//               {
//             price.Property(p => p.Amount)
//                        .HasColumnName("PriceAmount")
//                        .HasColumnType("decimal(18,2)")
//                        .IsRequired();

//             price.Property(p => p.Currency)
//                        .HasColumnName("PriceCurrency")
//                        .HasMaxLength(3)
//                        .IsRequired();
//           });

//         // Configure Images collection
//         entity.Property(e => e.Images)
//                    .HasConversion(
//                        v => string.Join(',', v),
//                        v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList());
//       });

//       // Configure Review entity
//       modelBuilder.Entity<Review>(entity =>
//       {
//         entity.HasKey(e => new { e.UserId, e.CarId });

//         entity.HasOne<User>()
//                    .WithMany()
//                    .HasForeignKey(e => e.UserId)
//                    .OnDelete(DeleteBehavior.Restrict);

//         entity.HasOne<Car>()
//                    .WithMany()
//                    .HasForeignKey(e => e.CarId)
//                    .OnDelete(DeleteBehavior.Restrict);

//         entity.Property(e => e.Rating).IsRequired();
//         entity.Property(e => e.Comment).HasMaxLength(1000);
//         entity.Property(e => e.CreatedAt).IsRequired();
//       });
//     }
//   }
// }

// // Infrastructure/Repositories/UserRepository.cs
// using CarRentalService.Core.Application.Interfaces;
// using CarRentalService.Core.Domain.Entities;
// using CarRentalService.Core.Domain.ValueObjects;
// using CarRentalService.Infrastructure.Persistence;
// using Microsoft.EntityFrameworkCore;
// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;

// namespace CarRentalService.Infrastructure.Repositories
// {
//   public class UserRepository : IUserRepository
//   {
//     private readonly CarRentalDbContext _dbContext;

//     public UserRepository(CarRentalDbContext dbContext)
//     {
//       _dbContext = dbContext;
//     }

//     public async Task<User> GetByIdAsync(Guid id)
//     {
//       return await _dbContext.Users.FindAsync(id);
//     }

//     public async Task<User> GetByEmailAsync(Email email)
//     {
//       return await _dbContext.Users
//           .SingleOrDefaultAsync(u => u.Email.Value == email.Value);
//     }

//     public async Task<User> GetByOAuthIdAsync(string provider, string oauthId)
//     {
//       return await _dbContext.Users
//           .SingleOrDefaultAsync(u => u.OAuthProvider == provider && u.OAuthId == oauthId);
//     }

//     public async Task AddAsync(User user)
//     {
//       await _dbContext.Users.AddAsync(user);
//       await _dbContext.SaveChangesAsync();
//     }

//     public async Task UpdateAsync(User user)
//     {
//       _dbContext.Users.Update(user);
//       await _dbContext.SaveChangesAsync();
//     }

//     public async Task<bool> EmailExistsAsync(Email email)
//     {
//       return await _dbContext.Users
//           .AnyAsync(u => u.Email.Value == email.Value);
//     }
//   }
// }

// // Infrastructure/Repositories/CarRepository.cs
// using CarRentalService.Core.Application.Interfaces;
// using CarRentalService.Core.Domain.Entities;
// using CarRentalService.Core.Domain.ValueObjects;
// using CarRentalService.Infrastructure.Persistence;
// using Microsoft.EntityFrameworkCore;
// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;

// namespace CarRentalService.Infrastructure.Repositories
// {
//   public class CarRepository : ICarRepository
//   {
//     private readonly CarRentalDbContext _dbContext;

//     public CarRepository(CarRentalDbContext dbContext)
//     {
//       _dbContext = dbContext;
//     }

//     public async Task<Car> GetByIdAsync(Guid id)
//     {
//       return await _dbContext.Cars.FindAsync(id);
//     }

//     public async Task<List<Car>> GetAllAsync()
//     {
//       return await _dbContext.Cars.ToListAsync();
//     }

//     public async Task<List<Car>> GetAvailableCarsAsync(DateRange period, Location location)
//     {
//       // Get all cars
//       var allCars = await _dbContext.Cars.ToListAsync();

//       // Get cars that are already rented during the specified period
//       var rentedCarIds = await _dbContext.Rentals
//           .Where(r =>
//               (r.RentalPeriod.Start <= period.End && r.RentalPeriod.End >= period.Start) &&
//               r.PickupLocation.Address == location.Address)
//           .Select(r => r.CarId)
//           .ToListAsync();

//       // Return cars that are not in the rented list
//       return allCars.Where(c => !rentedCarIds.Contains(c.Id)).ToList();
//     }

//     public async Task AddAsync(Car car)
//     {
//       await _dbContext.Cars.AddAsync(car);
//       await _dbContext.SaveChangesAsync();
//     }

//     public async Task UpdateAsync(Car car)
//     {
//       _dbContext.Cars.Update(car);
//       await _dbContext.SaveChangesAsync();
//     }

//     public async Task DeleteAsync(Guid id)
//     {
//       var car = await _dbContext.Cars.FindAsync(id);
//       if (car != null)
//       {
//         _dbContext.Cars.Remove(car);
//         await _dbContext.SaveChangesAsync();
//       }
//     }
//   }
// }

// // Infrastructure/Repositories/RentalRepository.cs
// using CarRentalService.Core.Application.Interfaces;
// using CarRentalService.Core.Domain.Entities;
// using CarRentalService.Infrastructure.Persistence;
// using Microsoft.EntityFrameworkCore;
// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;

// namespace CarRentalService.Infrastructure.Repositories
// {
//   public class RentalRepository : IRentalRepository
//   {
//     private readonly CarRentalDbContext _dbContext;

//     public RentalRepository(CarRentalDbContext dbContext)
//     {
//       _dbContext = dbContext;
//     }

//     public async Task<Rental> GetByIdAsync(Guid id)
//     {
//       return await _dbContext.Rentals.FindAsync(id);
//     }

//     public async Task<List<Rental>> GetByUserIdAsync(Guid userId)
//     {
//       return await _dbContext.Rentals
//           .Where(r => r.UserId == userId)
//           .ToListAsync();
//     }

//     public async Task<List<Rental>> GetByCarIdAsync(Guid carId)
//     {
//       return await _dbContext.Rentals
//           .Where(r => r.CarId == carId)
//           .ToListAsync();
//     }

//     public async Task AddAsync(Rental rental)
//     {
//       await _dbContext.Rentals.AddAsync(rental);
//       await _dbContext.SaveChangesAsync();
//     }

//     public async Task UpdateAsync(Rental rental)
//     {
//       _dbContext.Rentals.Update(rental);
//       await _dbContext.SaveChangesAsync();
//     }

//     public async Task<bool> IsCarAvailableAsync(Guid carId, DateTime start, DateTime end)
//     {
//       return !await _dbContext.Rentals
//           .AnyAsync(r =>
//               r.CarId == carId &&
//               r.RentalPeriod.Start <= end &&
//               r.RentalPeriod.End >= start);
//     }
//   }
// }

// // Infrastructure/Repositories/ReviewRepository.cs
// using CarRentalService.Core.Application.Interfaces;
// using CarRentalService.Core.Domain.Entities;
// using CarRentalService.Infrastructure.Persistence;
// using Microsoft.EntityFrameworkCore;
// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;

// namespace CarRentalService.Infrastructure.Repositories
// {
//   public class ReviewRepository : IReviewRepository
//   {
//     private readonly CarRentalDbContext _dbContext;

//     public ReviewRepository(CarRentalDbContext dbContext)
//     {
//       _dbContext = dbContext;
//     }

//     public async Task<Review> GetByUserAndCarIdAsync(Guid userId, Guid carId)
//     {
//       return await _dbContext.Reviews
//           .FirstOrDefaultAsync(r => r.UserId == userId && r.CarId == carId);
//     }

//     public async Task<List<Review>> GetByCarIdAsync(Guid carId)
//     {
//       return await _dbContext.Reviews
//           .Where(r => r.CarId == carId)
//           .ToListAsync();
//     }

//     public async Task AddAsync(Review review)
//     {
//       await _dbContext.Reviews.AddAsync(review);
//       await _dbContext.SaveChangesAsync();
//     }

//     public async Task UpdateAsync(Review review)
//     {
//       _dbContext.Reviews.Update(review);
//       await _dbContext.SaveChangesAsync();
//     }

//     public async Task<bool> HasUserRentedCarAsync(Guid userId, Guid carId)
//     {
//       return await _dbContext.Rentals
//           .AnyAsync(r =>
//               r.UserId == userId &&
//               r.CarId == carId &&
//               r.RentalPeriod.End < DateTime.UtcNow); // Rental must be completed
//     }
//   }
// }

// // Infrastructure/Services/AuthService.cs
// using CarRentalService.Core.Application.DTOs;
// using CarRentalService.Core.Application.Interfaces;
// using CarRentalService.Core.Domain.Entities;
// using Microsoft.Extensions.Configuration;
// using Microsoft.IdentityModel.Tokens;
// using System;
// using System.Collections.Generic;
// using System.IdentityModel.Tokens.Jwt;
// using System.Net.Http;
// using System.Security.Claims;
// using System.Text;
// using System.Text.Json;
// using System.Threading.Tasks;

// namespace CarRentalService.Infrastructure.Services
// {
//   public class AuthService : IAuthService
//   {
//     private readonly IConfiguration _configuration;
//     private readonly IHttpClientFactory _httpClientFactory;

//     public AuthService(IConfiguration configuration, IHttpClientFactory httpClientFactory)
//     {
//       _configuration = configuration;
//       _httpClientFactory = httpClientFactory;
//     }

//     public string HashPassword(string password)
//     {
//       return BCrypt.Net.BCrypt.HashPassword(password, 12);
//     }

//     public bool VerifyPassword(string password, string passwordHash)
//     {
//       return BCrypt.Net.BCrypt.Verify(password, passwordHash);
//     }

//     public string GenerateJwtToken(User user)
//     {
//       var tokenHandler = new JwtSecurityTokenHandler();
//       var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Secret"]);

//       var claims = new List<Claim>
//             {
//                 new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
//                 new Claim(ClaimTypes.Email, user.Email.Value),
//                 new Claim(ClaimTypes.Name, user.Name),
//                 new Claim(ClaimTypes.Role, user.Role)
//             };

//       var tokenDescriptor = new SecurityTokenDescriptor
//       {
//         Subject = new ClaimsIdentity(claims),
//         Expires = DateTime.UtcNow.AddDays(7),
//         SigningCredentials = new SigningCredentials(
//               new SymmetricSecurityKey(key),
//               SecurityAlgorithms.HmacSha256Signature),
//         Issuer = _configuration["Jwt:Issuer"],
//         Audience = _configuration["Jwt:Audience"]
//       };

//       var token = tokenHandler.CreateToken(tokenDescriptor);
//       return tokenHandler.WriteToken(token);
//     }

//     // public async Task<OAuthUserInfoDto> VerifyGoogleTokenAsync(string token)
//     // {
//     //   var httpClient = _httpClientFactory.CreateClient();
//     //   var response = await httpClient.GetAsync(
//     //       $"https://www.googleapis.com/oauth2/v3/tokeninfo?id_token={token}");

//     //   if (!response.IsSuccessStatusCode)
//     //   {
//     //     return null;
//     //   }

//     //   var jsonString = await response.Content.ReadAsStringAsync();
//     //   var googleUserInfo = JsonSerializer.Deserialize<JsonElement>(jsonString);

//     //   return new OAuthUserInfoDto
//     //   {
//     //     Provider = "Google",
//     //     ProviderId = googleUserInfo.GetProperty("sub").GetString(),
//     //     Email = googleUserInfo.GetProperty("email").GetString(),
//     //     Name = googleUserInfo.GetProperty("name").GetString(),
//     //     ProfileImageUrl = googleUserInfo.TryGetProperty("picture", out var picture) ?
//     //           picture.GetString() : null
//     //   };
//     // }

//     // public async Task<OAuthUserInfoDto> VerifyGithubTokenAsync(string token)
//     // {
//     //   var httpClient = _httpClientFactory.CreateClient();
//     //   httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
//     //   httpClient.DefaultRequestHeaders.Add("User-Agent", "CarRentalService");

//     //   // Get user info
//     //   var userResponse = await httpClient.GetAsync("https://api.github.com/user");
//     //   if (!userResponse.IsSuccessStatusCode)
//     //   {
//     //     return null;
//     //   }

//     //   var userJsonString = await userResponse.Content.ReadAsStringAsync();
//     //   var githubUserInfo = JsonSerializer.Deserialize<JsonElement>(userJsonString);

//     //   // Get email (might be private)
//     //   var emailResponse = await httpClient.GetAsync("https://api.github.com/user/emails");
//     //   string email = null;

//     //   if (emailResponse.IsSuccessStatusCode)
//     //   {
//     //     var emailJsonString = await emailResponse.Content.ReadAsStringAsync();
//     //     var emails = JsonSerializer.Deserialize<JsonElement>(emailJsonString);

//     //     // Find primary email
//     //     foreach (var emailObj in emails.EnumerateArray())
//     //     {
//     //       if (emailObj.TryGetProperty("primary", out var primary) &&
//     //           primary.GetBoolean() &&
//     //           emailObj.TryGetProperty("email", out var emailProp))
//     //       {
//     //         email = emailProp.GetString();
//     //         break;
//     //       }
//     //     }
//     //   }

//     //   // Use login as email if no email is available
//     //   if (string.IsNullOrEmpty(email))
//     //   {
//     //     email = $"{githubUserInfo.GetProperty("login").GetString()}@github.com";
//     //   }

//     //   return new OAuthUserInfoDto
//     //   {
//     //     Provider = "GitHub",
//     //     ProviderId = githubUserInfo.GetProperty("id").GetString(),
//     //     Email = email,
//     //     Name = githubUserInfo.TryGetProperty("name", out var name) &&
//     //              !name.ValueKind.Equals(JsonValueKind.Null) ?
//     //              name.GetString() : githubUserInfo.GetProperty("login").GetString(),
//     //     ProfileImageUrl = githubUserInfo.TryGetProperty("avatar_url", out var avatar) ?
//     //           avatar.GetString() : null
//     //   };
//     }
//   }
