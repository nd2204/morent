// using System.ComponentModel.DataAnnotations;

// namespace CarRental.Core.Application
// {
//   #region DTOs

//   // Auth DTOs
//   public class LoginRequest
//   {
//     [Required]
//     [EmailAddress]
//     public string Email { get; set; }

//     [Required]
//     public string Password { get; set; }
//   }

//   public class OAuthLoginRequest
//   {
//     [Required]
//     public string ProviderToken { get; set; }

//     [Required]
//     public OAuthProvider Provider { get; set; }
//   }

//   public class RegisterRequest
//   {
//     [Required]
//     public string Name { get; set; }

//     [Required]
//     [EmailAddress]
//     public string Email { get; set; }

//     [Required]
//     [MinLength(8)]
//     public string Password { get; set; }

//     [Required]
//     [Compare("Password")]
//     public string ConfirmPassword { get; set; }
//   }

//   public class AuthResponse
//   {
//     public Guid UserId { get; set; }
//     public string Name { get; set; }
//     public string Email { get; set; }
//     public UserRole Role { get; set; }
//     public string Token { get; set; }
//     public DateTime TokenExpiration { get; set; }
//     public string RefreshToken { get; set; }
//   }

//   // Car DTOs
//   public class CarDto
//   {
//     public Guid Id { get; set; }
//     public string Brand { get; set; }
//     public string Model { get; set; }
//     public int Year { get; set; }
//     public string LicensePlate { get; set; }
//     public FuelType FuelType { get; set; }
//     public decimal PricePerDay { get; set; }
//     public string Currency { get; set; }
//     public int Capacity { get; set; }
//     public List<string> Images { get; set; }
//     public bool IsAvailable { get; set; }
//     public LocationDto CurrentLocation { get; set; }
//     public double? AverageRating { get; set; }
//     public int ReviewsCount { get; set; }
//   }

//   public class LocationDto
//   {
//     public string Address { get; set; }
//     public string City { get; set; }
//     public string State { get; set; }
//     public string ZipCode { get; set; }
//     public string Country { get; set; }
//     public double? Latitude { get; set; }
//     public double? Longitude { get; set; }
//   }

//   // Rental DTOs
//   public class RentalDto
//   {
//     public Guid Id { get; set; }
//     public Guid CarId { get; set; }
//     public string CarBrand { get; set; }
//     public string CarModel { get; set; }
//     public DateTime PickupDate { get; set; }
//     public DateTime DropoffDate { get; set; }
//     public LocationDto PickupLocation { get; set; }
//     public LocationDto DropoffLocation { get; set; }
//     public decimal TotalCost { get; set; }
//     public string Currency { get; set; }
//     public RentalStatus Status { get; set; }
//     public DateTime CreatedAt { get; set; }
//   }

//   // Review DTOs
//   public class ReviewDto
//   {
//     public Guid Id { get; set; }
//     public Guid UserId { get; set; }
//     public string UserName { get; set; }
//     public Guid CarId { get; set; }
//     public string CarDetails { get; set; }
//     public int Rating { get; set; }
//     public string Comment { get; set; }
//     public DateTime CreatedAt { get; set; }
//   }

//   #endregion

//   #region Interfaces

//   // Repository Interfaces
//   public interface IUserRepository : IRepository<User, Guid>
//   {
//     Task<User> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
//     Task<User> GetByOAuthInfoAsync(OAuthProvider provider, string oauthId, CancellationToken cancellationToken = default);
//     Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default);
//   }

//   public interface ICarRepository : IRepository<Car, Guid>
//   {
//     Task<IEnumerable<Car>> GetAvailableCarsAsync(DateTime start, DateTime end, LocationDto nearLocation = null, int? minCapacity = null, CancellationToken cancellationToken = default);
//     Task<IEnumerable<Car>> GetCarsByBrandAsync(string brand, CancellationToken cancellationToken = default);
//     Task<Car> GetCarWithRentalsAsync(Guid id, CancellationToken cancellationToken = default);
//     Task<Car> GetCarWithReviewsAsync(Guid id, CancellationToken cancellationToken = default);
//   }

//   public interface IRentalRepository : IRepository<Rental, Guid>
//   {
//     Task<IEnumerable<Rental>> GetRentalsByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
//     Task<IEnumerable<Rental>> GetActiveRentalsAsync(CancellationToken cancellationToken = default);
//     Task<IEnumerable<Rental>> GetRentalsByCarIdAsync(Guid carId, CancellationToken cancellationToken = default);
//   }

//   public interface IReviewRepository : IRepository<Review, Guid>
//   {
//     Task<IEnumerable<Review>> GetReviewsByCarIdAsync(Guid carId, CancellationToken cancellationToken = default);
//     Task<IEnumerable<Review>> GetReviewsByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
//     Task<bool> ExistsByUserAndCarAsync(Guid userId, Guid carId, CancellationToken cancellationToken = default);
//   }

//   // Service Interfaces
//   public interface IAuthService
//   {
//     Task<AuthResponse> AuthenticateAsync(LoginRequest request, CancellationToken cancellationToken = default);
//     Task<AuthResponse> AuthenticateWithOAuthAsync(OAuthLoginRequest request, CancellationToken cancellationToken = default);
//     Task<AuthResponse> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default);
//     Task<AuthResponse> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default);
//     Task<bool> ValidateTokenAsync(string token, CancellationToken cancellationToken = default);
//   }

//   public interface IPasswordHasher
//   {
//     string HashPassword(string password);
//     bool VerifyPassword(string hashedPassword, string providedPassword);
//   }

//   public interface IOAuthService
//   {
//     Task<(string id, string email, string name, string pictureUrl)> ValidateGoogleTokenAsync(string token, CancellationToken cancellationToken = default);
//     Task<(string id, string email, string name, string pictureUrl)> ValidateGitHubTokenAsync(string token, CancellationToken cancellationToken = default);
//   }

//   public interface ICurrentUserService
//   {
//     Guid? UserId { get; }
//     string Email { get; }
//     UserRole Role { get; }
//     bool IsAuthenticated { get; }
//   }

//   public interface IPaymentService
//   {
//     Task<string> ProcessPaymentAsync(Guid userId, Guid rentalId, Money amount, CancellationToken cancellationToken = default);
//     Task<bool> RefundPaymentAsync(string paymentId, CancellationToken cancellationToken = default);
//   }

//   #endregion

//   #region Commands and Queries

//   // Auth Commands
//   public class RegisterUserCommand : ICommand<AuthResponse>
//   {
//     public string Name { get; set; }
//     public string Email { get; set; }
//     public string Password { get; set; }
//   }

//   public class LoginUserQuery : IQuery<AuthResponse>
//   {
//     public string Email { get; set; }
//     public string Password { get; set; }
//   }

//   public class LoginWithOAuthQuery : IQuery<AuthResponse>
//   {
//     public OAuthProvider Provider { get; set; }
//     public string ProviderToken { get; set; }
//   }

//   // Car Commands/Queries
//   public class CreateCarCommand : ICommand<Guid>
//   {
//     public string Brand { get; set; }
//     public string Model { get; set; }
//     public int Year { get; set; }
//     public string LicensePlate { get; set; }
//     public FuelType FuelType { get; set; }
//     public decimal PricePerDay { get; set; }
//     public string Currency { get; set; }
//     public int Capacity { get; set; }
//     public LocationDto Location { get; set; }
//     public List<string> Images { get; set; }
//   }

//   public class UpdateCarCommand : ICommand<bool>
//   {
//     public Guid Id { get; set; }
//     public decimal PricePerDay { get; set; }
//     public string Currency { get; set; }
//     public bool IsAvailable { get; set; }
//     public LocationDto Location { get; set; }
//     public List<string> ImagesToAdd { get; set; }
//     public List<string> ImagesToRemove { get; set; }
//   }

//   public class GetCarByIdQuery : IQuery<CarDto>
//   {
//     public Guid Id { get; set; }
//   }

//   public class GetAvailableCarsQuery : IQuery<IEnumerable<CarDto>>
//   {
//     public DateTime StartDate { get; set; }
//     public DateTime EndDate { get; set; }
//     public LocationDto NearLocation { get; set; }
//     public int? MinCapacity { get; set; }
//     public string Brand { get; set; }
//     public FuelType? FuelType { get; set; }
//   }

//   // Rental Commands/Queries
//   public class RentCarCommand : ICommand<Guid>
//   {
//     public Guid CarId { get; set; }
//     public DateTime PickupDate { get; set; }
//     public DateTime DropoffDate { get; set; }
//     public LocationDto PickupLocation { get; set; }
//     public LocationDto DropoffLocation { get; set; }
//     public string PaymentMethodId { get; set; }
//   }

//   public class ActivateRentalCommand : ICommand<bool>
//   {
//     public Guid RentalId { get; set; }
//   }

//   public class CompleteRentalCommand : ICommand<bool>
//   {
//     public Guid RentalId { get; set; }
//   }

//   public class CancelRentalCommand : ICommand<bool>
//   {
//     public Guid RentalId { get; set; }
//   }

//   public class GetUserRentalsQuery : IQuery<IEnumerable<RentalDto>>
//   {
//     public Guid UserId { get; set; }
//     public RentalStatus? Status { get; set; }
//   }

//   public class GetRentalByIdQuery : IQuery<RentalDto>
//   {
//     public Guid Id { get; set; }
//   }

//   // Review Commands/Queries
//   public class LeaveReviewCommand : ICommand<Guid>
//   {
//     public Guid CarId { get; set; }
//     public Guid RentalId { get; set; }
//     public int Rating { get; set; }
//     public string Comment { get; set; }
//   }

//   public class UpdateReviewCommand : ICommand<bool>
//   {
//     public Guid ReviewId { get; set; }
//     public int Rating { get; set; }
//     public string Comment { get; set; }
//   }

//   public class GetCarReviewsQuery : IQuery<IEnumerable<ReviewDto>>
//   {
//     public Guid CarId { get; set; }
//   }

//   #endregion

//   #region Command and Query Handlers

//   // Authentication Handlers
//   public class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand, AuthResponse>
//   {
//     private readonly IUserRepository _userRepository;
//     private readonly IAuthService _authService;
//     private readonly IPasswordHasher _passwordHasher;
//     private readonly IUnitOfWork _unitOfWork;

//     public RegisterUserCommandHandler(
//         IUserRepository userRepository,
//         IAuthService authService,
//         IPasswordHasher passwordHasher,
//         IUnitOfWork unitOfWork)
//     {
//       _userRepository = userRepository;
//       _authService = authService;
//       _passwordHasher = passwordHasher;
//       _unitOfWork = unitOfWork;
//     }

//     public async Task<AuthResponse> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
//     {
//       // Validate email is unique
//       if (await _userRepository.ExistsByEmailAsync(command.Email, cancellationToken))
//         throw new ApplicationException("Email is already in use");

//       // Create the user with hashed password
//       var userId = Guid.NewGuid();
//       var passwordHash = _passwordHasher.HashPassword(command.Password);
//       var email = new Email(command.Email);

//       var user = new User(userId, command.Name, email, passwordHash);
//       await _userRepository.AddAsync(user, cancellationToken);
//       await _unitOfWork.SaveChangesAsync(cancellationToken);

//       // Return authentication response
//       var authRequest = new LoginRequest
//       {
//         Email = command.Email,
//         Password = command.Password
//       };

//       return await _authService.AuthenticateAsync(authRequest, cancellationToken);
//     }
//   }

//   public class LoginUserQueryHandler : IQueryHandler<LoginUserQuery, AuthResponse>
//   {
//     private readonly IAuthService _authService;

//     public LoginUserQueryHandler(IAuthService authService)
//     {
//       _authService = authService;
//     }

//     public async Task<AuthResponse> Handle(LoginUserQuery query, CancellationToken cancellationToken)
//     {
//       var request = new LoginRequest
//       {
//         Email = query.Email,
//         Password = query.Password
//       };

//       return await _authService.AuthenticateAsync(request, cancellationToken);
//     }
//   }

//   public class LoginWithOAuthQueryHandler : IQueryHandler<LoginWithOAuthQuery, AuthResponse>
//   {
//     private readonly IAuthService _authService;

//     public LoginWithOAuthQueryHandler(IAuthService authService)
//     {
//       _authService = authService;
//     }

//     public async Task<AuthResponse> Handle(LoginWithOAuthQuery query, CancellationToken cancellationToken)
//     {
//       var request = new OAuthLoginRequest
//       {
//         Provider = query.Provider,
//         ProviderToken = query.ProviderToken
//       };

//       return await _authService.AuthenticateWithOAuthAsync(request, cancellationToken);
//     }
//   }

//   // Car Handlers
//   public class CreateCarCommandHandler : ICommandHandler<CreateCarCommand, Guid>
//   {
//     private readonly ICarRepository _carRepository;
//     private readonly IUnitOfWork _unitOfWork;
//     private readonly ICurrentUserService _currentUserService;

//     public CreateCarCommandHandler(
//         ICarRepository carRepository,
//         IUnitOfWork unitOfWork,
//         ICurrentUserService currentUserService)
//     {
//       _carRepository = carRepository;
//       _unitOfWork = unitOfWork;
//       _currentUserService = currentUserService;
//     }

//     public async Task<Guid> Handle(CreateCarCommand command, CancellationToken cancellationToken)
//     {
//       // Ensure only admin can create cars
//       if (_currentUserService.Role != UserRole.Admin)
//         throw new UnauthorizedAccessException("Only administrators can create cars");

//       var carId = Guid.NewGuid();
//       var pricePerDay = new Money(command.PricePerDay, command.Currency);

//       var location = new Location(
//           command.Location.Address,
//           command.Location.City,
//           command.Location.State,
//           command.Location.ZipCode,
//           command.Location.Country,
//           command.Location.Latitude,
//           command.Location.Longitude);

//       var car = new Car(
//           carId,
//           command.Brand,
//           command.Model,
//           command.Year,
//           command.LicensePlate,
//           command.FuelType,
//           pricePerDay,
//           command.Capacity,
//           location);

//       // Add images if provided
//       if (command.Images != null)
//       {
//         foreach (var imageUrl in command.Images)
//         {
//           car.AddImage(imageUrl);
//         }
//       }

//       await _carRepository.AddAsync(car, cancellationToken);
//       await _unitOfWork.SaveChangesAsync(cancellationToken);

//       return carId;
//     }
//   }

//   public class UpdateCarCommandHandler : ICommandHandler<UpdateCarCommand, bool>
//   {
//     private readonly ICarRepository _carRepository;
//     private readonly IUnitOfWork _unitOfWork;
//     private readonly ICurrentUserService _currentUserService;

//     public UpdateCarCommandHandler(
//         ICarRepository carRepository,
//         IUnitOfWork unitOfWork,
//         ICurrentUserService currentUserService)
//     {
//       _carRepository = carRepository;
//       _unitOfWork = unitOfWork;
//       _currentUserService = currentUserService;
//     }

//     public async Task<bool> Handle(UpdateCarCommand command, CancellationToken cancellationToken)
//     {
//       // Ensure only admin can update cars
//       if (_currentUserService.Role != UserRole.Admin)
//         throw new UnauthorizedAccessException("Only administrators can update cars");

//       var car = await _carRepository.GetByIdAsync(command.Id, cancellationToken);
//       if (car == null)
//         throw new ApplicationException($"Car with ID {command.Id} not found");

//       // Update price if provided
//       if (command.PricePerDay > 0)
//       {
//         var newPrice = new Money(command.PricePerDay, command.Currency);
//         car.UpdatePrice(newPrice);
//       }

//       // Update location if provided
//       if (command.Location != null)
//       {
//         var newLocation = new Location(
//             command.Location.Address,
//             command.Location.City,
//             command.Location.State,
//             command.Location.ZipCode,
//             command.Location.Country,
//             command.Location.Latitude,
//             command.Location.Longitude);

//         car.UpdateLocation(newLocation);
//       }

//       // Update availability
//       car.SetAvailability(command.IsAvailable);

//       // Add new images
//       if (command.ImagesToAdd != null)
//       {
//         foreach (var imageUrl in command.ImagesToAdd)
//         {
//           car.AddImage(imageUrl);
//         }
//       }

//       // Remove images
//       if (command.ImagesToRemove != null)
//       {
//         foreach (var imageUrl in command.ImagesToRemove)
//         {
//           car.RemoveImage(imageUrl);
//         }
//       }

//       await _carRepository.UpdateAsync(car, cancellationToken);
//       await _unitOfWork.SaveChangesAsync(cancellationToken);

//       return true;
//     }
//   }

//   public class GetCarByIdQueryHandler : IQueryHandler<GetCarByIdQuery, CarDto>
//   {
//     private readonly ICarRepository _carRepository;
//     private readonly IReviewRepository _reviewRepository;

//     public GetCarByIdQueryHandler(
//         ICarRepository carRepository,
//         IReviewRepository reviewRepository)
//     {
//       _carRepository = carRepository;
//       _reviewRepository = reviewRepository;
//     }

//     public async Task<CarDto> Handle(GetCarByIdQuery query, CancellationToken cancellationToken)
//     {
//       var car = await _carRepository.GetCarWithReviewsAsync(query.Id, cancellationToken);
//       if (car == null)
//         throw new ApplicationException($"Car with ID {query.Id} not found");

//       var reviews = car.Reviews.ToList();
//       double? averageRating = reviews.Any()
//           ? reviews.Average(r => r.Rating)
//           : null;

//       return new CarDto
//       {
//         Id = car.Id,
//         Brand = car.Brand,
//         Model = car.Model,
//         Year = car.Year,
//         LicensePlate = car.LicensePlate,
//         FuelType = car.FuelType,
//         PricePerDay = car.PricePerDay.Amount,
//         Currency = car.PricePerDay.Currency,
//         Capacity = car.Capacity,
//         Images = car.Images.ToList(),
//         IsAvailable = car.IsAvailable,
//         CurrentLocation = new LocationDto
//         {
//           Address = car.CurrentLocation.Address,
//           City = car.CurrentLocation.City,
//           State = car.CurrentLocation.State,
//           ZipCode = car.CurrentLocation.ZipCode,
//           Country = car.CurrentLocation.Country,
//           Latitude = car.CurrentLocation.Latitude,
//           Longitude = car.CurrentLocation.Longitude
//         },
//         AverageRating = averageRating,
//         ReviewsCount = reviews.Count
//       };
//     }
//   }

//   public class GetAvailableCarsQueryHandler : IQueryHandler<GetAvailableCarsQuery, IEnumerable<CarDto>>
//   {
//     private readonly ICarRepository _carRepository;
//     private readonly IReviewRepository _reviewRepository;

//     public GetAvailableCarsQueryHandler(
//         ICarRepository carRepository,
//         IReviewRepository reviewRepository)
//     {
//       _carRepository = carRepository;
//       _reviewRepository = reviewRepository;
//     }

//     public async Task<IEnumerable<CarDto>> Handle(GetAvailableCarsQuery query, CancellationToken cancellationToken)
//     {
//       var cars = await _carRepository.GetAvailableCarsAsync(
//           query.StartDate,
//           query.EndDate,
//           query.NearLocation,
//           query.MinCapacity,
//           cancellationToken);

//       // Filter by additional criteria if provided
//       if (!string.IsNullOrWhiteSpace(query.Brand))
//       {
//         cars = cars.Where(c => c.Brand.Equals(query.Brand, StringComparison.OrdinalIgnoreCase));
//       }

//       if (query.FuelType.HasValue)
//       {
//         cars = cars.Where(c => c.FuelType == query.FuelType.Value);
//       }

//       var carDtos = new List<CarDto>();
//       foreach (var car in cars)
//       {
//         var reviews = await _reviewRepository.GetReviewsByCarIdAsync(car.Id, cancellationToken);
//         var reviewsList = reviews.ToList();
//         double? averageRating = reviewsList.Any()
//             ? reviewsList.Average(r => r.Rating)
//             : null;

//         carDtos.Add(new CarDto
//         {
//           Id = car.Id,
//           Brand = car.Brand,
//           Model = car.Model,
//           Year = car.Year,
//           LicensePlate = car.LicensePlate,
//           FuelType = car.FuelType,
//           PricePerDay = car.PricePerDay.Amount,
//           Currency = car.PricePerDay.Currency,
//           Capacity = car.Capacity,
//           Images = car.Images.ToList(),
//           IsAvailable = car.IsAvailable,
//           CurrentLocation = new LocationDto
//           {
//             Address = car.CurrentLocation.Address,
//             City = car.CurrentLocation.City,
//             State = car.CurrentLocation.State,
//             ZipCode = car.CurrentLocation.ZipCode,
//             Country = car.CurrentLocation.Country,
//             Latitude = car.CurrentLocation.Latitude,
//             Longitude = car.CurrentLocation.Longitude
//           },
//           AverageRating = averageRating,
//           ReviewsCount = reviewsList.Count
//         });
//       }

//       return carDtos;
//     }
//   }

//   // Rental Handlers
//   public class RentCarCommandHandler : ICommandHandler<RentCarCommand, Guid>
//   {
//     private readonly ICarRepository _carRepository;
//     private readonly IRentalRepository _rentalRepository;
//     private readonly ICurrentUserService _currentUserService;
//     private readonly IPaymentService _paymentService;
//     private readonly IUnitOfWork _unitOfWork;

//     public RentCarCommandHandler(
//         ICarRepository carRepository,
//         IRentalRepository rentalRepository,
//         ICurrentUserService currentUserService,
//         IPaymentService paymentService,
//         IUnitOfWork unitOfWork)
//     {
//       _carRepository = carRepository;
//       _rentalRepository = rentalRepository;
//       _currentUserService = currentUserService;
//       _paymentService = paymentService;
//       _unitOfWork = unitOfWork;
//     }

//     public async Task<Guid> Handle(RentCarCommand command, CancellationToken cancellationToken)
//     {
//       if (!_currentUserService.IsAuthenticated || !_currentUserService.UserId.HasValue)
//         throw new UnauthorizedAccessException("User must be authenticated to rent a car");

//       var userId = _currentUserService.UserId.Value;

//       // Validate dates
//       if (command.PickupDate >= command.DropoffDate)
//         throw new ApplicationException("Pickup date must be before dropoff date");

//       if (command.PickupDate.Date < DateTime.UtcNow.Date)
//         throw new ApplicationException("Pickup date cannot be in the past");

//       // Get car and verify availability
//       var car = await _carRepository.GetCarWithRentalsAsync(command.CarId, cancellationToken);
//       if (car == null)
//         throw new ApplicationException($"Car with ID {command.CarId} not found");

//       var rentalPeriod = new DateRange(command.PickupDate, command.DropoffDate);

//       if (!car.IsAvailableDuring(rentalPeriod))
//         throw new ApplicationException("Car is not available for the selected dates");

//       // Create pickup and dropoff locations
//       var pickupLocation = new Location(
//           command.PickupLocation.Address,
//           command.PickupLocation.City,
//           command.PickupLocation.State,
//           command.PickupLocation.ZipCode,
//           command.PickupLocation.Country,
//           command.PickupLocation.Latitude,
//           command.PickupLocation.Longitude);

//       var dropoffLocation = new Location(
//           command.DropoffLocation.Address,
//           command.DropoffLocation.City,
//           command.DropoffLocation.State,
//           command.DropoffLocation.ZipCode,
//           command.DropoffLocation.Country,
//           command.DropoffLocation.Latitude,
//           command.DropoffLocation.Longitude);

//       // Calculate total cost
//       var durationDays = rentalPeriod.DurationInDays;
//       var totalCost = car.PricePerDay.Multiply(durationDays);

//       // Process payment
//       var paymentId = await _paymentService.ProcessPaymentAsync(
//           userId,
//           Guid.NewGuid(), // Temporary rental ID for payment processing
//           totalCost,
//           cancellationToken);

//       if (string.IsNullOrEmpty(paymentId))
//         throw new ApplicationException("Payment processing failed");

//       // Create rental
//       var rentalId = Guid.NewGuid();
//       var rental = new Rental(
//           rentalId,
//           car.Id,
//           userId,
//           rentalPeriod,
//           pickupLocation,
//           dropoffLocation,
//           totalCost);

//       // Add rental to car and repository
//       car.AddRental(rental);
//       await _rentalRepository.AddAsync(rental, cancellationToken);
//       await _carRepository.UpdateAsync(car, cancellationToken);

//       // Add domain event
//       await _unitOfWork.SaveChangesAsync(cancellationToken);

//       return rentalId;
//     }
//   }

//   public class ActivateRentalCommandHandler : ICommandHandler<ActivateRentalCommand, bool>
//   {
//     private readonly IRentalRepository _rentalRepository;
//     private readonly ICurrentUserService _currentUserService;
//     private readonly IUnitOfWork _unitOfWork;

//     public ActivateRentalCommandHandler(
//         IRentalRepository rentalRepository,
//         ICurrentUserService currentUserService,
//         IUnitOfWork unitOfWork)
//     {
//       _rentalRepository = rentalRepository;
//       _currentUserService = currentUserService;
//       _unitOfWork = unitOfWork;
//     }

//     public async Task<bool> Handle(ActivateRentalCommand command, CancellationToken cancellationToken)
//     {
//       // Verify user is an admin or support
//       if (_currentUserService.Role != UserRole.Admin && _currentUserService.Role != UserRole.Support)
//         throw new UnauthorizedAccessException("Only admin or support can activate rentals");

//       var rental = await _rentalRepository.GetByIdAsync(command.RentalId, cancellationToken);
//       if (rental == null)
//         throw new ApplicationException($"Rental with ID {command.RentalId} not found");

//       rental.Activate();
//       await _rentalRepository.UpdateAsync(rental, cancellationToken);
//       await _unitOfWork.SaveChangesAsync(cancellationToken);

//       return true;
//     }
//   }

//   public class CompleteRentalCommandHandler : ICommandHandler<CompleteRentalCommand, bool>
//   {
//     private readonly IRentalRepository _rentalRepository;
//     private readonly ICarRepository _carRepository;
//     private readonly ICurrentUserService _currentUserService;
//     private readonly IUnitOfWork _unitOfWork;

//     public CompleteRentalCommandHandler(
//         IRentalRepository rentalRepository,
//         ICarRepository carRepository,
//         ICurrentUserService currentUserService,
//         IUnitOfWork unitOfWork)
//     {
//       _rentalRepository = rentalRepository;
//       _carRepository = carRepository;
//       _currentUserService = currentUserService;
//       _unitOfWork = unitOfWork;
//     }

//     public async Task<bool> Handle(CompleteRentalCommand command, CancellationToken cancellationToken)
//     {
//       // Verify user is an admin or support
//       if (_currentUserService.Role != UserRole.Admin && _currentUserService.Role != UserRole.Support)
//         throw new UnauthorizedAccessException("Only admin or support can complete rentals");

//       var rental = await _rentalRepository.GetByIdAsync(command.RentalId, cancellationToken);
//       if (rental == null)
//         throw new ApplicationException($"Rental with ID {command.RentalId} not found");

//       rental.Complete();
//       await _rentalRepository.UpdateAsync(rental, cancellationToken);

//       // Add domain event
//       var domainEvent = new RentalCompletedEvent(rental.Id, rental.CarId, rental.UserId);

//       await _unitOfWork.SaveChangesAsync(cancellationToken);

//       return true;
//     }
//   }

//   public class CancelRentalCommandHandler : ICommandHandler<CancelRentalCommand, bool>
//   {
//     private readonly IRentalRepository _rentalRepository;
//     private readonly IPaymentService _paymentService;
//     private readonly ICurrentUserService _currentUserService;
//     private readonly IUnitOfWork _unitOfWork;

//     public CancelRentalCommandHandler(
//         IRentalRepository rentalRepository,
//         IPaymentService paymentService,
//         ICurrentUserService currentUserService,
//         IUnitOfWork unitOfWork)
//     {
//       _rentalRepository = rentalRepository;
//       _paymentService = paymentService;
//       _currentUserService = currentUserService;
//       _unitOfWork = unitOfWork;
//     }

//     public async Task<bool> Handle(CancelRentalCommand command, CancellationToken cancellationToken)
//     {
//       if (!_currentUserService.IsAuthenticated || !_currentUserService.UserId.HasValue)
//         throw new UnauthorizedAccessException("User must be authenticated to cancel a rental");

//       var userId = _currentUserService.UserId.Value;

//       var rental = await _rentalRepository.GetByIdAsync(command.RentalId, cancellationToken);
//       if (rental == null)
//         throw new ApplicationException($"Rental with ID {command.RentalId} not found");

//       // Verify it's the user's rental or user is admin
//       if (rental.UserId != userId && _currentUserService.Role != UserRole.Admin)
//         throw new UnauthorizedAccessException("You can only cancel your own rentals");

//       rental.Cancel();
//       await _rentalRepository.UpdateAsync(rental, cancellationToken);

//       // Process refund if needed - for example, full refund if cancelled more than 24h in advance
//       var now = DateTime.UtcNow;
//       if (rental.RentalPeriod.Start.Subtract(now).TotalHours > 24)
//       {
//         // Implement refund logic via payment service
//         // await _paymentService.RefundPaymentAsync(paymentId);
//       }

//       await _unitOfWork.SaveChangesAsync(cancellationToken);

//       return true;
//     }
//   }

//   public class GetUserRentalsQueryHandler : IQueryHandler<GetUserRentalsQuery, IEnumerable<RentalDto>>
//   {
//     private readonly IRentalRepository _rentalRepository;
//     private readonly ICarRepository _carRepository;
//     private readonly ICurrentUserService _currentUserService;

//     public GetUserRentalsQueryHandler(
//         IRentalRepository rentalRepository,
//         ICarRepository carRepository,
//         ICurrentUserService currentUserService)
//     {
//       _rentalRepository = rentalRepository;
//       _carRepository = carRepository;
//       _currentUserService = currentUserService;
//     }

//     public async Task<IEnumerable<RentalDto>> Handle(GetUserRentalsQuery query, CancellationToken cancellationToken)
//     {
//       if (!_currentUserService.IsAuthenticated || !_currentUserService.UserId.HasValue)
//         throw new UnauthorizedAccessException("User must be authenticated to view rentals");

//       var currentUserId = _currentUserService.UserId.Value;

//       // Only allow users to see their own rentals, unless they're an admin
//       if (query.UserId != currentUserId && _currentUserService.Role != UserRole.Admin)
//         throw new UnauthorizedAccessException("You can only view your own rentals");

//       var rentals = await _rentalRepository.GetRentalsByUserIdAsync(query.UserId, cancellationToken);

//       if (query.Status.HasValue)
//       {
//         rentals = rentals.Where(r => r.Status == query.Status.Value);
//       }

//       var rentalDtos = new List<RentalDto>();
//       foreach (var rental in rentals)
//       {
//         var car = await _carRepository.GetByIdAsync(rental.CarId, cancellationToken);

//         rentalDtos.Add(new RentalDto
//         {
//           Id = rental.Id,
//           CarId = rental.CarId,
//           CarBrand = car?.Brand ?? "Unknown",
//           CarModel = car?.Model ?? "Unknown",
//           PickupDate = rental.RentalPeriod.Start,
//           DropoffDate = rental.RentalPeriod.End,
//           PickupLocation = new LocationDto
//           {
//             Address = rental.PickupLocation.Address,
//             City = rental.PickupLocation.City,
//             State = rental.PickupLocation.State,
//             ZipCode = rental.PickupLocation.ZipCode,
//             Country = rental.PickupLocation.Country,
//             Latitude = rental.PickupLocation.Latitude,
//             Longitude = rental.PickupLocation.Longitude
//           },
//           DropoffLocation = new LocationDto
//           {
//             Address = rental.DropoffLocation.Address,
//             City = rental.DropoffLocation.City,
//             State = rental.DropoffLocation.State,
//             ZipCode = rental.DropoffLocation.ZipCode,
//             Country = rental.DropoffLocation.Country,
//             Latitude = rental.DropoffLocation.Latitude,
//             Longitude = rental.DropoffLocation.Longitude
//           },
//           TotalCost = rental.TotalCost.Amount,
//           Currency = rental.TotalCost.Currency,
//           Status = rental.Status,
//           CreatedAt = rental.CreatedAt
//         });
//       }

//       return rentalDtos;
//     }
//   }

//   // Review Handlers
//   public class LeaveReviewCommandHandler : ICommandHandler<LeaveReviewCommand, Guid>
//   {
//     private readonly IReviewRepository _reviewRepository;
//     private readonly IRentalRepository _rentalRepository;
//     private readonly ICarRepository _carRepository;
//     private readonly ICurrentUserService _currentUserService;
//     private readonly IUnitOfWork _unitOfWork;

//     public LeaveReviewCommandHandler(
//         IReviewRepository reviewRepository,
//         IRentalRepository rentalRepository,
//         ICarRepository carRepository,
//         ICurrentUserService currentUserService,
//         IUnitOfWork unitOfWork)
//     {
//       _reviewRepository = reviewRepository;
//       _rentalRepository = rentalRepository;
//       _carRepository = carRepository;
//       _currentUserService = currentUserService;
//       _unitOfWork = unitOfWork;
//     }

//     public async Task<Guid> Handle(LeaveReviewCommand command, CancellationToken cancellationToken)
//     {
//       if (!_currentUserService.IsAuthenticated || !_currentUserService.UserId.HasValue)
//         throw new UnauthorizedAccessException("User must be authenticated to leave a review");

//       var userId = _currentUserService.UserId.Value;

//       // Verify the user has completed a rental for this car
//       var rental = await _rentalRepository.GetByIdAsync(command.RentalId, cancellationToken);
//       if (rental == null)
//         throw new ApplicationException($"Rental with ID {command.RentalId} not found");

//       if (!rental.CanBeReviewedBy(userId))
//         throw new ApplicationException("You can only review cars from completed rentals");

//       // Check if user already reviewed this car
//       if (await _reviewRepository.ExistsByUserAndCarAsync(userId, command.CarId, cancellationToken))
//         throw new ApplicationException("You have already reviewed this car");

//       // Create and save review
//       var reviewId = Guid.NewGuid();
//       var review = new Review(reviewId, userId, command.CarId, command.Rating, command.Comment);

//       await _reviewRepository.AddAsync(review, cancellationToken);

//       // Add review to car
//       var car = await _carRepository.GetCarWithReviewsAsync(command.CarId, cancellationToken);
//       car.AddReview(review);
//       await _carRepository.UpdateAsync(car, cancellationToken);

//       // Add domain event
//       var domainEvent = new ReviewAddedEvent(reviewId, command.CarId, userId, command.Rating);

//       await _unitOfWork.SaveChangesAsync(cancellationToken);

//       return reviewId;
//     }
//   }

//   public class GetCarReviewsQueryHandler : IQueryHandler<GetCarReviewsQuery, IEnumerable<ReviewDto>>
//   {
//     private readonly IReviewRepository _reviewRepository;
//     private readonly IUserRepository _userRepository;

//     public GetCarReviewsQueryHandler(
//         IReviewRepository reviewRepository,
//         IUserRepository userRepository)
//     {
//       _reviewRepository = reviewRepository;
//       _userRepository = userRepository;
//     }

//     public async Task<IEnumerable<ReviewDto>> Handle(GetCarReviewsQuery query, CancellationToken cancellationToken)
//     {
//       var reviews = await _reviewRepository.GetReviewsByCarIdAsync(query.CarId, cancellationToken);

//       var reviewDtos = new List<ReviewDto>();
//       foreach (var review in reviews)
//       {
//         var user = await _userRepository.GetByIdAsync(review.UserId, cancellationToken);

//         reviewDtos.Add(new ReviewDto
//         {
//           Id = review.Id,
//           UserId = review.UserId,
//           UserName = user?.Name ?? "Anonymous",
//           CarId = review.CarId,
//           CarDetails = string.Empty, // Could be populated if needed
//           Rating = review.Rating,
//           Comment = review.Comment,
//           CreatedAt = review.CreatedAt
//         });
//       }

//       return reviewDtos;
//     }
//   }

//   #endregion

//   #region Validation

//   public class RegisterUserCommandValidator : IValidator<RegisterUserCommand>
//   {
//     public Task<ValidationResult> ValidateAsync(RegisterUserCommand command, CancellationToken cancellationToken = default)
//     {
//       var result = new ValidationResult();

//       if (string.IsNullOrWhiteSpace(command.Name))
//         result.AddError("Name is required");

//       if (string.IsNullOrWhiteSpace(command.Email))
//         result.AddError("Email is required");
//       else if (!IsValidEmail(command.Email))
//         result.AddError("Email format is invalid");

//       if (string.IsNullOrWhiteSpace(command.Password))
//         result.AddError("Password is required");
//       else if (command.Password.Length < 8)
//         result.AddError("Password must be at least 8 characters long");

//       return Task.FromResult(result);
//     }

//     private bool IsValidEmail(string email)
//     {
//       try
//       {
//         var addr = new System.Net.Mail.MailAddress(email);
//         return addr.Address == email;
//       }
//       catch
//       {
//         return false;
//       }
//     }
//   }

//   public class RentCarCommandValidator : IValidator<RentCarCommand>
//   {
//     public Task<ValidationResult> ValidateAsync(RentCarCommand command, CancellationToken cancellationToken = default)
//     {
//       var result = new ValidationResult();

//       if (command.CarId == Guid.Empty)
//         result.AddError("CarId is required");

//       if (command.PickupDate == default)
//         result.AddError("PickupDate is required");

//       if (command.DropoffDate == default)
//         result.AddError("DropoffDate is required");

//       if (command.PickupDate >= command.DropoffDate)
//         result.AddError("PickupDate must be before DropoffDate");

//       if (command.PickupDate.Date < DateTime.UtcNow.Date)
//         result.AddError("PickupDate cannot be in the past");

//       if (command.PickupLocation == null)
//         result.AddError("PickupLocation is required");
//       else
//       {
//         if (string.IsNullOrWhiteSpace(command.PickupLocation.Address))
//           result.AddError("PickupLocation.Address is required");
//         if (string.IsNullOrWhiteSpace(command.PickupLocation.City))
//           result.AddError("PickupLocation.City is required");
//         if (string.IsNullOrWhiteSpace(command.PickupLocation.Country))
//           result.AddError("PickupLocation.Country is required");
//       }

//       if (command.DropoffLocation == null)
//         result.AddError("DropoffLocation is required");
//       else
//       {
//         if (string.IsNullOrWhiteSpace(command.DropoffLocation.Address))
//           result.AddError("DropoffLocation.Address is required");
//         if (string.IsNullOrWhiteSpace(command.DropoffLocation.City))
//           result.AddError("DropoffLocation.City is required");
//         if (string.IsNullOrWhiteSpace(command.DropoffLocation.Country))
//           result.AddError("DropoffLocation.Country is required");
//       }

//       if (string.IsNullOrWhiteSpace(command.PaymentMethodId))
//         result.AddError("PaymentMethodId is required");

//       return Task.FromResult(result);
//     }
//   }

//   public class LeaveReviewCommandValidator : IValidator<LeaveReviewCommand>
//   {
//     public Task<ValidationResult> ValidateAsync(LeaveReviewCommand command, CancellationToken cancellationToken = default)
//     {
//       var result = new ValidationResult();

//       if (command.CarId == Guid.Empty)
//         result.AddError("CarId is required");

//       if (command.RentalId == Guid.Empty)
//         result.AddError("RentalId is required");

//       if (command.Rating < 1 || command.Rating > 5)
//         result.AddError("Rating must be between 1 and 5");

//       if (string.IsNullOrWhiteSpace(command.Comment))
//         result.AddError("Comment is required");

//       return Task.FromResult(result);
//     }
//   }

//   // Additional validators would follow the same pattern

//   public class ValidationResult
//   {
//     private readonly List<string> _errors = new List<string>();

//     public IReadOnlyCollection<string> Errors => _errors.AsReadOnly();

//     public bool IsValid => !_errors.Any();

//     public void AddError(string error)
//     {
//       _errors.Add(error);
//     }
//   }

//   public interface IValidator<T>
//   {
//     Task<ValidationResult> ValidateAsync(T command, CancellationToken cancellationToken = default);
//   }

//   #endregion
// }