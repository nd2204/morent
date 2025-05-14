using Ardalis.Result;
using Ardalis.SharedKernel;
using Microsoft.EntityFrameworkCore;
using Morent.Application.Interfaces;
using Morent.Core.MediaAggregate;
using Morent.Core.MediaAggregate.Specifications;
using Morent.Core.MorentCarAggregate;
using Morent.Core.MorentRentalAggregate;
using Morent.Core.MorentReviewAggregate;
using Morent.Core.MorentUserAggregate;
using Morent.Core.ValueObjects;
using Morent.Infrastructure.Data;
using Morent.Infrastructure.Services;

public static class SeedData
{
  public static int NUM_CARS_TO_SEED = 50;

  // =============================================================================
  // Users
  // =============================================================================

  public static MorentUser admin1 = MorentUser.CreateAdmin(
    "Morent LLC",
    "admin",
    Email.Create("admin@test.com").Value,
    AuthService._HashPassword("20102001")
  );

  public static MorentUser user1 = MorentUser.CreateLocalUser(
    name: "Alex Thomson",
      username: "alexts",
      email: Email.Create("alexts@morent.com"),
      passwordHash: AuthService._HashPassword("alextsmorentcom")
    );

  public static MorentUser user2 = MorentUser.CreateLocalUser(
    name: "Sarah Chen",
    username: "sarahchen",
    email: Email.Create("sarahchen@morent.com"),
    passwordHash: AuthService._HashPassword("sarahchenmorentco")
  );

  public static MorentUser user3 = MorentUser.CreateLocalUser(
    name: "Mike Johnson",
      username: "mikejohnson",
      email: Email.Create("mikejohnson@morent.com"),
      passwordHash: AuthService._HashPassword("mikejohnsonmorentco")
    );

  public static MorentUser user4 = MorentUser.CreateLocalUser(
    name: "David Wilson",
    username: "davidwilson",
    email: Email.Create("davidwilson@morent.com"),
    passwordHash: AuthService._HashPassword("davidwilsonmorentco")
  );

  public static MorentUser user5 = MorentUser.CreateLocalUser(
    name: "Lisa Anderson",
    username: "lisaanderson",
    email: Email.Create("lisaanderson@morent.com"),
    passwordHash: AuthService._HashPassword("lisaandersonmorentco")
  );

  public static MorentUser user6 = MorentUser.CreateLocalUser(
    name: "Emily Parker",
    username: "emilyparker",
    email: Email.Create("emilyparker@morent.com"),
    passwordHash: AuthService._HashPassword("lisaandersonmorentco")
  );

  // =============================================================================
  // Car Models
  // =============================================================================

  public static MorentCarModel KoenigseggCCGT = new MorentCarModel(
      Guid.NewGuid(), "Koenigsegg", "CCGT", 2007,
      FuelType.Gasoline, GearBoxType.Manual, CarType.Sport, 100, 2);

  public static MorentCarModel NissanGTR = new MorentCarModel(
      Guid.NewGuid(), "Nissan", "GT-R", 2023,
      FuelType.Gasoline, GearBoxType.Automatic, CarType.Sport, 70, 4);

  public static MorentCarModel RollsRoyceDawn = new MorentCarModel(
      Guid.NewGuid(), "Rolls-Royce", "Dawn", 2017,
      FuelType.Gasoline, GearBoxType.Automatic, CarType.Luxury, 70, 4);

  public static MorentCarModel MercedesSClass = new MorentCarModel(
      Guid.NewGuid(), "Mercedes-Benz", "S-Class", 2022,
      FuelType.Gasoline, GearBoxType.Automatic, CarType.Luxury, 65, 5);

  public static MorentCarModel TeslaModel3 = new MorentCarModel(
      Guid.NewGuid(), "Tesla", "Model 3", 2023,
      FuelType.Electric, GearBoxType.Automatic, CarType.Sedan, 55, 5);

  public static MorentCarModel TeslaModelX = new MorentCarModel(
      Guid.NewGuid(), "Tesla", "Model X", 2023,
      FuelType.Electric, GearBoxType.Automatic, CarType.SUV, 90, 7);

  public static MorentCarModel ToyotaRush = new MorentCarModel(
      Guid.NewGuid(), "Toyota", "Rush", 2022,
      FuelType.Gasoline, GearBoxType.Automatic, CarType.SUV, 45, 7);

  public static MorentCarModel HondaCRV = new MorentCarModel(
      Guid.NewGuid(), "Honda", "CR-V", 2022,
      FuelType.Gasoline, GearBoxType.Automatic, CarType.SUV, 50, 5);

  public static MorentCarModel DaihatsuTerios = new MorentCarModel(
      Guid.NewGuid(), "Daihatsu", "Terios", 2021,
      FuelType.Gasoline, GearBoxType.Automatic, CarType.SUV, 40, 7);

  public static MorentCarModel MGZSExclusive = new MorentCarModel(
      Guid.NewGuid(), "MG", "ZS Exclusive", 2023,
      FuelType.Gasoline, GearBoxType.Automatic, CarType.SUV, 38, 5);

  public static MorentCarModel ToyotaCamry = new MorentCarModel(
      Guid.NewGuid(), "Toyota", "Camry", 2022,
      FuelType.Gasoline, GearBoxType.Automatic, CarType.Sedan, 40, 5);

  public static MorentCarModel HondaAccord = new MorentCarModel(
      Guid.NewGuid(), "Honda", "Accord", 2022,
      FuelType.Gasoline, GearBoxType.Automatic, CarType.Sedan, 42, 5);

  public static MorentCarModel BMW3Series = new MorentCarModel(
      Guid.NewGuid(), "BMW", "3 Series", 2023,
      FuelType.Gasoline, GearBoxType.Automatic, CarType.Sedan, 50, 5);

  public static MorentCarModel FordRanger = new MorentCarModel(
      Guid.NewGuid(), "Ford", "Ranger", 2023,
      FuelType.Diesel, GearBoxType.Automatic, CarType.Pickup, 55, 5);

  public static MorentCarModel ToyotaHilux = new MorentCarModel(
      Guid.NewGuid(), "Toyota", "Hilux", 2022,
      FuelType.Diesel, GearBoxType.Automatic, CarType.Pickup, 50, 5);

  public static MorentCarModel Hyundaii10 = new MorentCarModel(
      Guid.NewGuid(), "Hyundai", "Grand i10", 2023,
      FuelType.Gasoline, GearBoxType.Automatic, CarType.Hatchback, 25, 5);

  public static MorentCarModel KiaMorning = new MorentCarModel(
      Guid.NewGuid(), "Kia", "Morning", 2023,
      FuelType.Gasoline, GearBoxType.Automatic, CarType.Hatchback, 24, 5);

  // =============================================================================
  // Actual Car Instance
  // =============================================================================

  public static MorentCar KiaMorningCar = CreateRandomCarWithModel(KiaMorning);
  public static MorentCar KoenigseggCCGTCar = CreateRandomCarWithModel(KoenigseggCCGT);
  public static MorentCar NissanGTRCar = CreateRandomCarWithModel(NissanGTR);
  public static MorentCar RollsRoyceDawnCar = CreateRandomCarWithModel(RollsRoyceDawn);
  public static MorentCar MercedesSClassCar = CreateRandomCarWithModel(MercedesSClass);
  public static MorentCar TeslaModel3Car = CreateRandomCarWithModel(TeslaModel3);
  public static MorentCar TeslaModelXCar = CreateRandomCarWithModel(TeslaModelX);
  public static MorentCar ToyotaRushCar = CreateRandomCarWithModel(ToyotaRush);
  public static MorentCar HondaCRVCar = CreateRandomCarWithModel(HondaCRV);
  public static MorentCar DaihatsuTeriosCar = CreateRandomCarWithModel(DaihatsuTerios);
  public static MorentCar MGZSExclusiveCar = CreateRandomCarWithModel(MGZSExclusive);
  public static MorentCar ToyotaCamryCar = CreateRandomCarWithModel(ToyotaCamry);
  public static MorentCar HondaAccordCar = CreateRandomCarWithModel(HondaAccord);
  public static MorentCar BMW3SeriesCar = CreateRandomCarWithModel(BMW3Series);
  public static MorentCar FordRangerCar = CreateRandomCarWithModel(FordRanger);
  public static MorentCar ToyotaHiluxCar = CreateRandomCarWithModel(ToyotaHilux);
  public static MorentCar Hyundaii10Car = CreateRandomCarWithModel(Hyundaii10);

  // =============================================================================
  // Car Reviews
  // =============================================================================

  public static Dictionary<string, MorentCarModel> CarModels = new Dictionary<string, MorentCarModel>
  {
    ["KoenigseggCCGT"] = KoenigseggCCGT,
    ["NissanGTR"] = NissanGTR,
    ["RollsRoyceDawn"] = RollsRoyceDawn,
    ["MercedesSClass"] = MercedesSClass,
    ["TeslaModel3"] = TeslaModel3,
    ["TeslaModelX"] = TeslaModelX,
    ["ToyotaRush"] = ToyotaRush,
    ["HondaCRV"] = HondaCRV,
    ["DaihatsuTerios"] = DaihatsuTerios,
    ["MGZSExclusive"] = MGZSExclusive,
    ["ToyotaCamry"] = ToyotaCamry,
    ["HondaAccord"] = HondaAccord,
    ["BMW3Series"] = BMW3Series,
    ["FordRanger"] = FordRanger,
    ["ToyotaHilux"] = ToyotaHilux,
    ["Hyundaii10"] = Hyundaii10,
    ["KiaMorning"] = KiaMorning
  };

  public static async Task InitializeAsync(IServiceProvider service, ILogger logger)
  {
    MorentDbContext context = service.GetRequiredService<MorentDbContext>();
    IAuthService authService = service.GetRequiredService<IAuthService>();
    IUserProfileService userProfileService = service.GetRequiredService<IUserProfileService>();
    IUserService userService = service.GetRequiredService<IUserService>();
    IImageService imageService = service.GetRequiredService<IImageService>();
    IWebHostEnvironment env = service.GetRequiredService<IWebHostEnvironment>();
    IRepository<MorentImage> imageRepository = service.GetRequiredService<IRepository<MorentImage>>();

    if (!await context.Users.AnyAsync())
    {
      await SeedUser(context, authService);
      logger.LogInformation("Users seeded successfully.");
    }

    if (!await context.Cars.AnyAsync())
    {
      await SeedCars(context);
      logger.LogInformation("Cars seeded successfully.");
    }

    await SeedCarImages(context, logger, env, imageService, imageRepository);

    if (!await context.Rentals.AnyAsync())
    {
      await SeedRentals(context, logger, userService);
    }

    if (!await context.Reviews.AnyAsync())
    {
      await SeedReviews(context, logger, userService);
    }
  }

  public static async Task PopulateTestData(IServiceProvider service, ILogger logger)
  {
    MorentDbContext context = service.GetRequiredService<MorentDbContext>();
    IAuthService authService = service.GetRequiredService<IAuthService>();
    IUserProfileService userProfileService = service.GetRequiredService<IUserProfileService>();
    IUserService userService = service.GetRequiredService<IUserService>();

    if (!await context.Users.AnyAsync())
    {
      await SeedUser(context, authService);
      logger.LogInformation("Users seeded successfully.");
    }

    if (!await context.Cars.AnyAsync())
    {
      await SeedCars(context);
      logger.LogInformation("Cars seeded successfully.");
    }

    if (!await context.Reviews.AnyAsync())
    {
      await SeedReviews(context, logger, userService);
    }

    await SeedUserProfileImage(context, userProfileService);
  }

  public static async Task<Dictionary<string, Guid>> SeedCarModels(MorentDbContext context)
  {
    Dictionary<string, Guid> SavedModelIds = new Dictionary<string, Guid>();
    // Clear existing car models if needed
    if (await context.CarModels.AnyAsync())
    {
      context.CarModels.RemoveRange(context.CarModels);
      await context.SaveChangesAsync();
    }

    // Add car models and save to get their database-generated IDs
    context.CarModels.AddRange(CarModels.Values);

    await context.SaveChangesAsync();

    // Retrieve the saved models with their actual database IDs
    var savedModels = await context.CarModels.ToListAsync();

    // Map the key names to the actual saved IDs for later use in SeedCars
    foreach (var keyValuePair in CarModels)
    {
      var modelName = keyValuePair.Key;
      var modelInDb = savedModels.FirstOrDefault(m =>
          m.Brand == keyValuePair.Value.Brand &&
          m.ModelName == keyValuePair.Value.ModelName &&
          m.Year == keyValuePair.Value.Year);

      if (modelInDb != null)
      {
        SavedModelIds[modelName] = modelInDb.Id;
      }
    }
    return SavedModelIds;
  }

  public static async Task SeedCars(MorentDbContext context)
  {
    // Clear existing cars
    if (await context.Cars.AnyAsync())
    {
      context.Cars.RemoveRange(context.Cars);
      await context.SaveChangesAsync();
    }

    Dictionary<string, Guid> SavedModelIds = await SeedCarModels(context);

    // Create a list to hold all cars
    var carsList = new List<MorentCar>();
    var random = new Random();

    for (int i = 0; i < NUM_CARS_TO_SEED; i++)
    {
      var randomModelKey = SavedModelIds.Keys.ElementAt(random.Next(SavedModelIds.Count));
      var modelInfo = CarModels[randomModelKey]; // For reference to car properties
      carsList.Add(CreateRandomCarWithModel(modelInfo));
    }

    // Add all cars to the database
    context.Cars.AddRange(carsList);
    await context.SaveChangesAsync();
  }

  public static async Task SeedTestCars(MorentDbContext context)
  {
    // Clear existing cars
    if (await context.Cars.AnyAsync())
    {
      context.Cars.RemoveRange(context.Cars);
      await context.SaveChangesAsync();
    }

    // Add all cars to the database
    context.Cars.AddRange(
      KiaMorningCar,
      KoenigseggCCGTCar,
      NissanGTRCar,
      RollsRoyceDawnCar,
      MercedesSClassCar,
      TeslaModel3Car,
      TeslaModelXCar,
      ToyotaRushCar,
      HondaCRVCar,
      DaihatsuTeriosCar,
      MGZSExclusiveCar,
      ToyotaCamryCar,
      HondaAccordCar,
      BMW3SeriesCar,
      FordRangerCar,
      ToyotaHiluxCar,
      Hyundaii10Car
    );
    await context.SaveChangesAsync();
  }

  // Add this method to your existing SeedData class
  public static async Task SeedCarImages(MorentDbContext context, ILogger logger, IWebHostEnvironment env, IImageService imageService, IRepository<MorentImage> imageRepository)
  {
    try
    {
      // Get all cars IDs only to minimize tracking
      var cars = await context.Cars
          .Where(c => !c.Images.Any()) // Only cars without images
          .ToListAsync();


      if (!cars.Any())
      {
        logger.LogInformation("All cars already have images - skipping image seeding.");
        return;
      }

      logger.LogInformation($"Found {cars.Count} cars without images.");

      // Get image IDs only
      var imageIds = await context.Images
          .Select(i => i.Id)
          .ToListAsync();

      if (!imageIds.Any())
      {
        // Upload new images if none exist
        var assetPath = Path.Combine(env.WebRootPath, "..", "SeedData", "uploads");
        string[] filePaths = Directory.GetFiles(assetPath);

        foreach (var filePath in filePaths)
        {
          using Stream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
          var response = await imageService.UploadImageAsync(new ImageUploadRequest
          {
            ImageData = stream,
            FileName = Path.GetFileName(filePath),
            ContentType = GetContentTypeFromExtension(Path.GetExtension(filePath))
          });
          if (response != null)
          {
            imageIds.Add(response.Value.ImageId);
          }
        }
      }

      if (!imageIds.Any())
      {
        logger.LogInformation("No images available for seeding.");
        return;
      }

      if (!(await imageService.GetPlaceHolderImageAsync()).IsSuccess)
        await SeedPlaceholderImages(env, imageService);

      // Process cars one by one with clear context between operations
      var random = new Random();
      int successCount = 0;
      int failureCount = 0;

      foreach (var car in cars)
      {
        try
        {
          // Clear the change tracker before each operation
          context.ChangeTracker.Clear();

          var fileName = $"{car.CarModel.Brand.ToLower()}-{car.CarModel.ModelName.ToLower()}-1.png";
          var image = await imageRepository.FirstOrDefaultAsync(new ImageByFileNameSpec(fileName));
          if (image != null)
          {
            logger.LogInformation($"Images with name {fileName} found: {image.Path}");
            var filePath = Path.Combine(env.WebRootPath, "uploads", image.Path);
            if (File.Exists(filePath))
            {
              await AssignCarImageDirectly(context, car.Id, image.Id);
              successCount++;
            }
          }
          else
          {
            logger.LogInformation($"No images with name {fileName} found. Assigning random");
            int index = random.Next(imageIds.Count);
            var imageId = imageIds[index];
            await AssignCarImageDirectly(context, car.Id, imageId);
            successCount++;
          }
        }
        catch (Exception ex)
        {
          failureCount++;
          logger.LogInformation($"Failed to assign image to car ID: {car.Id}. Error: {ex.Message}");
        }
      }

      logger.LogInformation($"Car image seeding completed. Success: {successCount}, Failures: {failureCount}");
    }
    catch (Exception ex)
    {
      logger.LogInformation($"Error seeding car images: {ex.Message}");
      throw; // Rethrow to allow the calling code to handle it
    }
    finally
    {
      await CleanupOrphanedImages(env, logger, imageRepository);
    }
  }

  public static async Task SeedPlaceholderImages(IWebHostEnvironment env, IImageService imageService)
  {
    var assetPath = Path.Combine(env.WebRootPath, "..", "SeedData", "placeholder");
    string[] filePaths = Directory.GetFiles(assetPath);

    foreach (var filePath in filePaths)
    {
      using Stream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
      var response = await imageService.UploadImageAsync(new ImageUploadRequest
      {
        ImageData = stream,
        FileName = Path.GetFileName(filePath),
        ContentType = GetContentTypeFromExtension(Path.GetExtension(filePath))
      });
    }
  }

  // New method to directly insert car image records
  public static async Task AssignCarImageDirectly(MorentDbContext context, Guid carId, Guid imageId)
  {
    // Check if the car exists
    bool carExists = await context.Cars.AnyAsync(c => c.Id == carId);
    if (!carExists)
    {
      throw new ApplicationException($"Car with ID {carId} not found");
    }

    // Check if the image exists
    bool imageExists = await context.Images.AnyAsync(i => i.Id == imageId);
    if (!imageExists)
    {
      throw new ApplicationException($"Image with ID {imageId} not found");
    }

    // Check if this car already has a primary image
    bool hasPrimaryImage = await context.CarImages
        .AnyAsync(ci => ci.CarId == carId && ci.IsPrimary);

    // Get the next display order
    int nextDisplayOrder = 1;
    var maxOrder = await context.CarImages
        .Where(ci => ci.CarId == carId)
        .Select(ci => (int?)ci.DisplayOrder)
        .MaxAsync() ?? 0;

    nextDisplayOrder = maxOrder + 1;

    // Create new car image entity directly
    var carImage = new MorentCarImage(carId, imageId, !hasPrimaryImage, nextDisplayOrder);

    // If this will be primary and there are existing primary images, update them first
    if (!hasPrimaryImage)
    {
      // This will be the first image, so it will be primary
      // No need to update other images
    }

    // Add the new car image
    context.CarImages.Add(carImage);
    await context.SaveChangesAsync();
  }

  public static string GetContentTypeFromExtension(string extension)
  {
    return extension.ToLower() switch
    {
      ".jpg" or ".jpeg" => "image/jpeg",
      ".png" => "image/png",
      _ => "application/octet-stream"
    };
  }

  public static async Task SeedUser(MorentDbContext context, IAuthService authService)
  {
    // Clear existing users if needed
    if (await context.Users.AnyAsync())
    {
      context.Users.RemoveRange(context.Users);
      await context.SaveChangesAsync();
    }

    context.Users.AddRange(admin1, user1, user2, user3, user4, user5, user6);
    await context.SaveChangesAsync();
  }

  public static async Task SeedUserProfileImage(MorentDbContext context, IUserProfileService userProfileService)
  {
    var userWithoutImages = await context.Users.Where(u => !u.ProfileImageId.HasValue).ToListAsync();
    foreach (var user in userWithoutImages)
    {
      // only seed user with morent email
      if (user.Email.ToString().EndsWith("@morent.com"))
        await userProfileService.UpdateUserProfileImageAsync(user.Id, $"https://i.pravatar.cc/150?u={user.Name.Split(" ")[0].ToLower()}");
    }
  }

  public static string GetRandomLetters(int count)
  {
    var random = new Random();
    const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    return new string(Enumerable.Repeat(chars, count)
      .Select(s => s[random.Next(s.Length)]).ToArray());
  }

  public static async Task SeedReviews(MorentDbContext context, ILogger logger, IUserService userService)
  {
    // Skip if reviews already exist
    if (await context.Reviews.AnyAsync())
    {
      logger.LogInformation("Reviews already exist - skipping review seeding.");
      return;
    }

    logger.LogInformation("Starting review seeding...");

    var random = new Random();

    // Get all completed rentals that don't have reviews yet
    var completedRentals = await context.Rentals
        .Where(r => r.Status == MorentRentalStatus.Completed)
        .ToListAsync();

    if (!completedRentals.Any())
    {
      logger.LogInformation("No completed rentals available for review seeding.");
      return;
    }

    logger.LogInformation($"Found {completedRentals.Count} completed rentals for review seeding.");

    // About 70% of completed rentals will have reviews
    int reviewCount = (int)(completedRentals.Count * 0.7);
    int successCount = 0;
    int failureCount = 0;

    // Shuffle completed rentals to randomize which ones get reviews
    var shuffledRentals = completedRentals.OrderBy(x => random.Next()).Take(reviewCount).ToList();

    foreach (var rental in shuffledRentals)
    {
      try
      {
        var review = CreateRandomReview(rental.UserId, rental.CarId);

        // Create review
        var reviewResult = await userService.LeaveReviewAsync(
            rental.UserId,
            rental.Id,
            review.Rating,
            review.Comment,
            CancellationToken.None
        );

        if (reviewResult.IsSuccess)
        {
          successCount++;
        }
        else
        {
          logger.LogError($"Failed to create review: {string.Join(", ", reviewResult.Errors)}");
          failureCount++;
        }
      }
      catch (Exception ex)
      {
        logger.LogError($"Error creating review: {ex.Message}");
        failureCount++;
      }
    }

    logger.LogInformation("Review seeding completed. Created {0} reviews. Failed: {1}", successCount, failureCount);
  }

  public static async Task SeedRentals(MorentDbContext context, ILogger logger, IUserService userService)
  {
    // Skip if rentals already exist
    if (await context.Rentals.AnyAsync())
    {
      logger.LogInformation("Rentals already exist - skipping rental seeding.");
      return;
    }

    logger.LogInformation("Starting rental seeding...");

    var random = new Random();

    // Get all users and cars to create rentals
    var users = await context.Users
        .Where(u => u.Role != MorentUserRole.Admin) // Only regular users, not admins
        .ToListAsync();

    var cars = await context.Cars
        .Include(c => c.CarModel)
        .ToListAsync();

    if (!users.Any() || !cars.Any())
    {
      logger.LogInformation("No users or cars available for rental seeding.");
      return;
    }

    logger.LogInformation($"Found {users.Count} users and {cars.Count} cars for rental seeding.");

    // Create a list to track which cars are rented in which periods
    var carRentalPeriods = new Dictionary<Guid, List<DateRange>>();
    foreach (var car in cars)
    {
      carRentalPeriods[car.Id] = new List<DateRange>();
    }

    // Generate between 50-80 rentals with various statuses
    int totalRentals = random.Next(50, 81);
    int successCount = 0;
    int failureCount = 0;

    // Status distribution: 60% completed, 20% active, 10% confirmed, 10% reserved
    int completedTarget = (int)(totalRentals * 0.6);
    int activeTarget = (int)(totalRentals * 0.2);
    int confirmedTarget = (int)(totalRentals * 0.1);
    int reservedTarget = totalRentals - completedTarget - activeTarget - confirmedTarget;

    int completedCount = 0;
    int activeCount = 0;
    int confirmedCount = 0;
    int reservedCount = 0;

    logger.LogInformation($"Target rental counts - Total: {totalRentals}, Completed: {completedTarget}, " +
                     $"Active: {activeTarget}, Confirmed: {confirmedTarget}, Reserved: {reservedTarget}");

    for (int i = 0; i < totalRentals; i++)
    {
      try
      {
        // Select random user and car
        var user = users[random.Next(users.Count)];
        var car = cars[random.Next(cars.Count)];

        // Define rental period params based on status goal
        DateTime pickupDate;
        DateTime dropoffDate;
        MorentRentalStatus targetStatus;

        // Determine which status to create based on current counts
        if (completedCount < completedTarget)
        {
          // Completed rentals are in the past
          pickupDate = DateTime.UtcNow.AddDays(-random.Next(30, 120));
          dropoffDate = pickupDate.AddDays(random.Next(1, 14));
          targetStatus = MorentRentalStatus.Completed;
          completedCount++;
        }
        else if (activeCount < activeTarget)
        {
          // Active rentals started in the past but end in the future
          pickupDate = DateTime.UtcNow.AddDays(-random.Next(1, 7));
          dropoffDate = DateTime.UtcNow.AddDays(random.Next(1, 14));
          targetStatus = MorentRentalStatus.Active;
          activeCount++;
        }
        else if (confirmedCount < confirmedTarget)
        {
          // Confirmed rentals are in the near future
          pickupDate = DateTime.UtcNow.AddDays(random.Next(1, 14));
          dropoffDate = pickupDate.AddDays(random.Next(1, 14));
          targetStatus = MorentRentalStatus.Confirmed;
          confirmedCount++;
        }
        else
        {
          // Reserved rentals are further in the future
          pickupDate = DateTime.UtcNow.AddDays(random.Next(14, 60));
          dropoffDate = pickupDate.AddDays(random.Next(1, 14));
          targetStatus = MorentRentalStatus.Reserved;
          reservedCount++;
        }

        // Create rental period
        var rentalPeriod = DateRange.Create(pickupDate, dropoffDate).Value;

        // Check if car is already booked for this period
        bool conflictExists = carRentalPeriods[car.Id].Any(period => period.Overlaps(rentalPeriod));
        if (conflictExists)
        {
          // Try again with a different period up to 3 times
          int attempts = 0;
          while (conflictExists && attempts < 3)
          {
            // Adjust the dates slightly
            pickupDate = pickupDate.AddDays(random.Next(3, 10));
            dropoffDate = pickupDate.AddDays(random.Next(1, 14));
            rentalPeriod = DateRange.Create(pickupDate, dropoffDate).Value;

            conflictExists = carRentalPeriods[car.Id].Any(period => period.Overlaps(rentalPeriod));
            attempts++;
          }

          // If still conflicts, skip this rental
          if (conflictExists)
          {
            i--; // Try again with different car/user
            continue;
          }
        }

        // Create random locations
        var cities = new[] { "New York", "Los Angeles", "Chicago", "Miami", "Seattle", "Boston", "Austin", "San Francisco", "Denver", "Philadelphia" };
        var pickupCity = cities[random.Next(cities.Length)];
        var dropoffCity = random.NextDouble() > 0.7 ? cities[random.Next(cities.Length)] : pickupCity; // 30% chance of different city

        var pickupAddress = $"{random.Next(100, 9999)} {Faker.Address.StreetName()}";
        var dropoffAddress = dropoffCity == pickupCity && random.NextDouble() > 0.5 ?
                            pickupAddress : $"{random.Next(100, 9999)} {Faker.Address.StreetName()}";

        // Create request DTO
        var rentalRequest = new CreateRentalRequest
        {
          CarId = car.Id,
          PickupDate = pickupDate,
          DropoffDate = dropoffDate,
          PickupLocation = new CarLocationDto
          {
            City = pickupCity,
            Address = pickupAddress,
            Country = "North America"
          },
          DropoffLocation = new CarLocationDto
          {
            City = dropoffCity,
            Address = dropoffAddress,
            Country = "North America"
          },
        };

        // Create rental
        var rentalResult = await userService.CreateRentalAsync(user.Id, car.Id, rentalRequest);

        if (!rentalResult.IsSuccess)
        {
          logger.LogInformation($"Failed to create rental: {string.Join(", ", rentalResult.Errors)}");
          failureCount++;
          i--; // Try again
          continue;
        }

        // Add the period to car's rental periods to track conflicts
        carRentalPeriods[car.Id].Add(rentalPeriod);

        // Modify status based on target status
        if (targetStatus != MorentRentalStatus.Reserved)
        {
          // Get the created rental
          var rental = await context.Rentals
              .FirstOrDefaultAsync(r => r.Id == rentalResult.Value.Id);

          if (rental != null)
          {
            // Progress the rental through each state as needed
            if (targetStatus == MorentRentalStatus.Confirmed ||
                targetStatus == MorentRentalStatus.Active ||
                targetStatus == MorentRentalStatus.Completed)
            {
              rental.ConfirmRental();
            }

            if (targetStatus == MorentRentalStatus.Active ||
                targetStatus == MorentRentalStatus.Completed)
            {
              rental.StartRental();
            }

            if (targetStatus == MorentRentalStatus.Completed)
            {
              rental.CompleteRental();
            }

            await context.SaveChangesAsync();
          }
        }

        successCount++;

        // Add some cancellations (about 10% of total attempts)
        if (random.NextDouble() < 0.1)
        {
          // Create an extra rental that will be cancelled
          var cancelRentalResult = await userService.CreateRentalAsync(user.Id, car.Id, new CreateRentalRequest
          {
            CarId = car.Id,
            PickupDate = DateTime.UtcNow.AddDays(random.Next(7, 30)),
            DropoffDate = DateTime.UtcNow.AddDays(random.Next(31, 45)),
            PickupLocation = new CarLocationDto
            {
              City = cities[random.Next(cities.Length)],
              Address = $"{random.Next(100, 9999)} {Faker.Address.StreetName()}",
              Country = "North America"
            },
            DropoffLocation = new CarLocationDto
            {
              City = cities[random.Next(cities.Length)],
              Address = $"{random.Next(100, 9999)} {Faker.Address.StreetName()}",
              Country = "North America"
            }
          });

          if (cancelRentalResult.IsSuccess)
          {
            // Cancel the rental
            await userService.CancelRentalAsync(user.Id, cancelRentalResult.Value.Id);
            successCount++;
          }
        }
      }
      catch (Exception ex)
      {
        logger.LogInformation($"Error creating rental: {ex.Message}");
        failureCount++;
      }
    }

    logger.LogInformation($"Rental seeding completed. Created {successCount} rentals. Failed: {failureCount}");
    logger.LogInformation($"Status breakdown - Completed: {completedCount}, Active: {activeCount}, " +
                     $"Confirmed: {confirmedCount}, Reserved: {reservedCount}");
  }

  public static string GetRandomDescription()
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

  public static async Task CleanupOrphanedImages(IWebHostEnvironment env, ILogger logger, IRepository<MorentImage> imageRepository)
  {
    try
    {
      logger.LogInformation("Starting orphaned image cleanup...");

      // Get the uploads directory path
      string uploadsDirectory = Path.Combine(env.WebRootPath, "uploads");
      if (!Directory.Exists(uploadsDirectory))
      {
        logger.LogInformation("Uploads directory does not exist. Nothing to clean up.");
        return;
      }

      // Get all files in the uploads directory
      var filesOnDisk = Directory.GetFiles(uploadsDirectory, "*.*", SearchOption.AllDirectories)
          .Select(file => Path.GetRelativePath(uploadsDirectory, file).Replace("\\", "/"))
          .ToList();

      logger.LogInformation($"Found {filesOnDisk.Count} files on disk in uploads directory.");

      // Get all image filenames from the database
      var imagesInDb = (await imageRepository.ListAsync()).AsQueryable()
          .Select(img => img.Path)
          .ToList();

      logger.LogInformation($"Found {imagesInDb.Count} images registered in database.");

      // Find files that exist on disk but not in the database
      var orphanedPersistedFiles = filesOnDisk.Except(imagesInDb).ToList();
      // Find files that exist on database but not in the disk
      var orphanedDatabaseFiles = imagesInDb.Except(filesOnDisk).ToList();

      logger.LogInformation($"Found {orphanedDatabaseFiles.Count} database image files to delete.");

      foreach (var fileName in orphanedDatabaseFiles)
      {
        try
        {
          string filePath = Path.Combine(uploadsDirectory, fileName);
          if (!File.Exists(filePath))
          {
            await imageRepository.DeleteRangeAsync(new ImageByPathSpec(fileName));
            logger.LogInformation($"Deleted orphaned database images: {fileName}");
          }
        }
        catch (Exception ex)
        {
          logger.LogInformation($"Error deleting file {fileName}: {ex.Message}");
        }
      }

      logger.LogInformation($"Found {orphanedPersistedFiles.Count} orphaned image files to delete.");

      foreach (var fileName in orphanedPersistedFiles)
      {
        try
        {
          string filePath = Path.Combine(uploadsDirectory, fileName);
          if (File.Exists(filePath))
          {
            File.Delete(filePath);
            logger.LogInformation($"Deleted orphaned file: {fileName}");
          }
        }
        catch (Exception ex)
        {
          logger.LogInformation($"Error deleting file {fileName}: {ex.Message}");
        }
      }
    }
    catch (Exception ex)
    {
      logger.LogInformation($"Error during image cleanup: {ex.Message}");
    }
  }

  private static MorentCar CreateRandomCarWithModel(MorentCarModel model)
  {
    var random = new Random();
    // Select a random car model from the dictionary

    // Generate a random license plate
    string licensePlate = $"{random.Next(100, 999)}-{GetRandomLetters(3)}-{random.Next(10, 99)}";

    // Generate a random price per day based on car type
    decimal pricePerDay = model.CarType switch
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
    var location = Location.Create(
      address: $"{random.Next(1, 999)} {Faker.Address.StreetName()}",
      city: Faker.Address.City(),
      country: Faker.Address.Country()
    ).Value;

    // Generate a description based on the car model
    string description = $"Experience the {model.Year} {model.Brand} {model.ModelName}. " +
                        $"This {model.CarType.ToString().ToLower()} car features a {model.Gearbox.ToString().ToLower()} transmission " +
                        $"and runs on {model.FuelType.ToString().ToLower()} fuel. " +
                        $"It comfortably seats {model.SeatCapacity} passengers and has a capacity of {model.SeatCapacity * 15}L for luggage. " +
                        GetRandomDescription();

    var pricePerDayResult = Money.Create(pricePerDay);

    // Create the MorentCar instance and add it to the list
    return new MorentCar(
      modelId: model.Id,
      licensePlate: licensePlate,
      pricePerDay: pricePerDayResult.Value,
      currentLocation: location,
      description: description
    );
  }

  private static MorentReview CreateRandomReview(Guid userId, Guid carId)
  {
    // Common review phrases
    var positiveComments = new[]
    {
        "Great car! Very clean and ran perfectly.",
        "Excellent condition and very comfortable for our trip.",
        "The car was amazing! Would definitely rent again.",
        "Perfect vehicle for our vacation. Very spacious and fuel-efficient.",
        "The car was delivered on time and in pristine condition.",
        "Smooth ride and excellent handling. Very satisfied!",
        "The car exceeded my expectations. Great experience overall.",
        "Very reliable vehicle with good gas mileage.",
        "Clean, well-maintained car. No issues whatsoever.",
        "Luxury experience at a reasonable price. Highly recommended!"
    };

    var mixedComments = new[]
    {
        "Good car overall, though it had a few minor issues.",
        "Decent vehicle for the price. Nothing extraordinary but got the job done.",
        "Car was clean but not as fuel-efficient as expected.",
        "Good experience overall, but pickup process was a bit slow.",
        "The car served its purpose well, but the AC wasn't working properly.",
        "Good value, but there were some minor mechanical issues.",
        "Car was okay, but had some scratches that weren't listed.",
        "Decent experience. The car was a bit older than expected.",
        "Generally satisfied, though the interior showed signs of wear.",
        "Car worked fine but wasn't as clean as I expected."
    };

    var negativeComments = new[]
    {
        "Car had several issues that weren't disclosed beforehand.",
        "Not satisfied with the cleanliness of the vehicle.",
        "Several mechanical problems during our trip. Not recommended.",
        "The car was much older than advertised.",
        "Poor condition and unreliable. Had to call roadside assistance.",
        "Disappointing experience. The car broke down during our trip.",
        "Vehicle was not properly maintained. Had safety concerns.",
        "Car was delivered late and dirty. Very disappointing.",
        "Terrible fuel economy and uncomfortable seats.",
        "Many undisclosed issues with the car. Would not rent again."
    };

    var random = new Random();
    // Generate random rating (1-5)
    // Weighted distribution: 5★: 40%, 4★: 30%, 3★: 15%, 2★: 10%, 1★: 5%
    int rating;
    double ratingRoll = random.NextDouble();
    if (ratingRoll < 0.05) rating = 1;
    else if (ratingRoll < 0.15) rating = 2;
    else if (ratingRoll < 0.30) rating = 3;
    else if (ratingRoll < 0.60) rating = 4;
    else rating = 5;

    // Select appropriate comment based on rating
    string comment;
    if (rating >= 4)
    {
      comment = positiveComments[random.Next(positiveComments.Length)];
    }
    else if (rating >= 3)
    {
      comment = mixedComments[random.Next(mixedComments.Length)];
    }
    else
    {
      comment = negativeComments[random.Next(negativeComments.Length)];
    }

    // Add some custom details to make reviews more unique
    if (random.NextDouble() > 0.7)
    {
      var additionalDetails = new[]
      {
                    " The pickup and dropoff process was seamless.",
                    " Would definitely recommend this car.",
                    " Perfect for a family trip.",
                    " Great for city driving.",
                    " Ideal for long road trips.",
                    " The owner was very helpful and responsive.",
                    " The car was very fuel efficient.",
                    " Loved the entertainment system.",
                    " The GPS was very helpful during our trip.",
                    " All the features worked flawlessly."
                };

      comment += additionalDetails[random.Next(additionalDetails.Length)];
    }

    return MorentReview.Create(userId, carId, rating, comment);
  }
}