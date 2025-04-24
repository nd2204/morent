using Microsoft.EntityFrameworkCore;
using Morent.Application.Interfaces;
using Morent.Core.MorentCarAggregate;
using Morent.Core.MorentUserAggregate;
using Morent.Core.ValueObjects;
using Morent.Infrastructure.Data;
public class SeedData
{
  private readonly IAuthService _authService;
  private readonly IImageService _imageService;
  private readonly MorentDbContext _dbContext;

  private MorentUser admin1 = null!;

  public SeedData(MorentDbContext context, IAuthService authService, IImageService imageService)
  {
    _dbContext = context;
    _imageService = imageService;
    _authService = authService;
  }

  private readonly Dictionary<string, MorentCarModel> _carModels = new Dictionary<string, MorentCarModel>
  {
    ["KoenigseggCCGT"] = new MorentCarModel(
        Guid.NewGuid(), "Koenigsegg", "CCGT", 2007,
        FuelType.Gasoline, GearBoxType.Manual, CarType.Sport, 100, 2),

    ["NissanGTR"] = new MorentCarModel(
        Guid.NewGuid(), "Nissan", "GT-R", 2023,
        FuelType.Gasoline, GearBoxType.Automatic, CarType.Sport, 70, 4),

    ["RollsRoyceDawn"] = new MorentCarModel(
        Guid.NewGuid(), "Rolls-Royce", "Dawn", 2017,
        FuelType.Gasoline, GearBoxType.Automatic, CarType.Luxury, 70, 4),

    ["MercedesSClass"] = new MorentCarModel(
        Guid.NewGuid(), "Mercedes-Benz", "S-Class", 2022,
        FuelType.Gasoline, GearBoxType.Automatic, CarType.Luxury, 65, 5),

    ["TeslaModel3"] = new MorentCarModel(
        Guid.NewGuid(), "Tesla", "Model 3", 2023,
        FuelType.Electric, GearBoxType.Automatic, CarType.Sedan, 55, 5),

    ["TeslaModelX"] = new MorentCarModel(
        Guid.NewGuid(), "Tesla", "Model X", 2023,
        FuelType.Electric, GearBoxType.Automatic, CarType.SUV, 90, 7),

    ["ToyotaRush"] = new MorentCarModel(
        Guid.NewGuid(), "Toyota", "Rush", 2022,
        FuelType.Gasoline, GearBoxType.Automatic, CarType.SUV, 45, 7),

    ["HondaCRV"] = new MorentCarModel(
        Guid.NewGuid(), "Honda", "CR-V", 2022,
        FuelType.Gasoline, GearBoxType.Automatic, CarType.SUV, 50, 5),

    ["DaihatsuTerios"] = new MorentCarModel(
        Guid.NewGuid(), "Daihatsu", "Terios", 2021,
        FuelType.Gasoline, GearBoxType.Automatic, CarType.SUV, 40, 7),

    ["MGZSExclusive"] = new MorentCarModel(
        Guid.NewGuid(), "MG", "ZS Exclusive", 2023,
        FuelType.Gasoline, GearBoxType.Automatic, CarType.SUV, 38, 5),

    ["ToyotaCamry"] = new MorentCarModel(
        Guid.NewGuid(), "Toyota", "Camry", 2022,
        FuelType.Gasoline, GearBoxType.Automatic, CarType.Sedan, 40, 5),

    ["HondaAccord"] = new MorentCarModel(
        Guid.NewGuid(), "Honda", "Accord", 2022,
        FuelType.Gasoline, GearBoxType.Automatic, CarType.Sedan, 42, 5),

    ["BMW3Series"] = new MorentCarModel(
        Guid.NewGuid(), "BMW", "3 Series", 2023,
        FuelType.Gasoline, GearBoxType.Automatic, CarType.Sedan, 50, 5),

    ["FordRanger"] = new MorentCarModel(
        Guid.NewGuid(), "Ford", "Ranger", 2023,
        FuelType.Diesel, GearBoxType.Automatic, CarType.Pickup, 55, 5),

    ["ToyotaHilux"] = new MorentCarModel(
        Guid.NewGuid(), "Toyota", "Hilux", 2022,
        FuelType.Diesel, GearBoxType.Automatic, CarType.Pickup, 50, 5),

    ["Hyundaii10"] = new MorentCarModel(
        Guid.NewGuid(), "Hyundai", "Grand i10", 2023,
        FuelType.Gasoline, GearBoxType.Automatic, CarType.Hatchback, 25, 5),

    ["KiaMorning"] = new MorentCarModel(
        Guid.NewGuid(), "Kia", "Morning", 2023,
        FuelType.Gasoline, GearBoxType.Automatic, CarType.Hatchback, 24, 5),
  };

  private Dictionary<string, Guid> _savedModelIds = new Dictionary<string, Guid>();

  public async Task InitializeAsync()
  {
    // First seed users
    if (!await _dbContext.Users.AnyAsync())
    {
      await SeedUser();
      Console.WriteLine("Users seeded successfully.");
    }

    // Cars depends on models to be created first
    if (!await _dbContext.Cars.AnyAsync())
    {
      // Seed car models and store their database IDs
      await SeedCarModels();
      Console.WriteLine("Car models seeded successfully.");

      // Finally seed cars using the saved model IDs
      await SeedCars();
      Console.WriteLine("Cars seeded successfully.");
    }
  }

  private async Task SeedCarModels()
  {
    // Clear existing car models if needed
    if (await _dbContext.CarModels.AnyAsync())
    {
      _dbContext.CarModels.RemoveRange(_dbContext.CarModels);
      await _dbContext.SaveChangesAsync();
    }

    // Add car models and save to get their database-generated IDs
    _dbContext.CarModels.AddRange(_carModels.Values);
    await _dbContext.SaveChangesAsync();

    // Retrieve the saved models with their actual database IDs
    var savedModels = await _dbContext.CarModels.ToListAsync();

    // Map the key names to the actual saved IDs for later use in SeedCars
    foreach (var keyValuePair in _carModels)
    {
      var modelName = keyValuePair.Key;
      var modelInDb = savedModels.FirstOrDefault(m =>
          m.Brand == keyValuePair.Value.Brand &&
          m.ModelName == keyValuePair.Value.ModelName &&
          m.Year == keyValuePair.Value.Year);

      if (modelInDb != null)
      {
        _savedModelIds[modelName] = modelInDb.Id;
      }
    }
  }

  private async Task SeedCars()
  {
    // Clear existing cars
    if (await _dbContext.Cars.AnyAsync())
    {
      _dbContext.Cars.RemoveRange(_dbContext.Cars);
      await _dbContext.SaveChangesAsync();
    }

    // Create a list to hold all cars
    var carsList = new List<MorentCar>();
    var random = new Random();

    // Create 50 cars
    for (int i = 0; i < 50; i++)
    {
      // Select a random car model from the dictionary
      var randomModelKey = _savedModelIds.Keys.ElementAt(random.Next(_savedModelIds.Count));
      var modelId = _savedModelIds[randomModelKey];
      var modelInfo = _carModels[randomModelKey]; // For reference to car properties

      // Generate a random license plate
      string licensePlate = $"{random.Next(100, 999)}-{GetRandomLetters(3)}-{random.Next(10, 99)}";

      // Generate a random price per day based on car type
      decimal pricePerDay = modelInfo.CarType switch
      {
        CarType.Luxury => random.Next(200, 501), // $200-$500
        CarType.Sport => random.Next(150, 401),  // $150-$400
        CarType.SUV => random.Next(80, 201),     // $80-$200
        CarType.Sedan => random.Next(60, 151),   // $60-$150
        CarType.Pickup => random.Next(90, 181),  // $90-$180
        CarType.Hatchback => random.Next(40, 101), // $40-$100
        _ => random.Next(50, 151)                // $50-$150 default
      };

      // Generate a random location
      var location = new Location(
        address: $"{random.Next(1, 999)} {Faker.Address.StreetName()}",
        city: Faker.Address.City(),
        country: Faker.Address.Country()
      );

      // Generate a description based on the car model
      string description = $"Experience the {modelInfo.Year} {modelInfo.Brand} {modelInfo.ModelName}. " +
                          $"This {modelInfo.CarType.ToString().ToLower()} car features a {modelInfo.Gearbox.ToString().ToLower()} transmission " +
                          $"and runs on {modelInfo.FuelType.ToString().ToLower()} fuel. " +
                          $"It comfortably seats {modelInfo.SeatCapacity} passengers and has a capacity of {modelInfo.SeatCapacity * 15}L for luggage. " +
                          GetRandomDescription();

      // Create the MorentCar instance and add it to the list
      var car = new MorentCar(
        modelId: modelId,
        licensePlate: licensePlate,
        pricePerDay: new Money(pricePerDay),
        currentLocation: location,
        description: description
      );

      // Assign the car to an owner (optional, depends on your model requirements)
      // car.OwnerId = admin1.Id;

      carsList.Add(car);
    }

    // Add all cars to the database
    _dbContext.Cars.AddRange(carsList);
    await _dbContext.SaveChangesAsync();
  }

  private async Task SeedUser()
  {
    // Clear existing users if needed
    if (await _dbContext.Users.AnyAsync())
    {
      _dbContext.Users.RemoveRange(_dbContext.Users);
      await _dbContext.SaveChangesAsync();
    }

    admin1 = MorentUser.CreateAdmin(
      "Admin",
      "admin",
      new Email("admin@test.com"),
      _authService.HashPassword("20102001")
    );

    _dbContext.Users.Add(admin1);
    await _dbContext.SaveChangesAsync();
  }

  private string GetRandomLetters(int count)
  {
    var random = new Random();
    const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    return new string(Enumerable.Repeat(chars, count)
      .Select(s => s[random.Next(s.Length)]).ToArray());
  }

  private string GetRandomDescription()
  {
    var descriptions = new[]
    {
      "Perfect for both city driving and long road trips. Enjoy superior comfort and performance.",
      "A reliable vehicle with excellent fuel efficiency. Ideal for daily commutes and weekend getaways.",
      "Luxury meets practicality with this exceptional vehicle. Experience premium features and smooth handling.",
      "Designed for adventure seekers. This vehicle offers robust performance and ample space.",
      "Cutting-edge technology combined with sleek design. A truly modern driving experience.",
      "Exceptional performance and elegant styling make this car stand out from the crowd.",
      "Engineered for maximum efficiency without compromising on comfort or style.",
      "The perfect family vehicle with advanced safety features and spacious interior.",
      "Experience unmatched luxury and power. This vehicle redefines premium driving."
    };
    return descriptions[new Random().Next(descriptions.Length)];
  }
}