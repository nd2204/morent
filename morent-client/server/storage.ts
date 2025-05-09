import { 
  User, InsertUser, 
  CarCategory, InsertCarCategory,
  Car, InsertCar,
  Rental, InsertRental,
  Transaction, InsertTransaction
} from "@shared/schema";

export interface IStorage {
  // User methods
  getUser(id: number): Promise<User | undefined>;
  getUserByUsername(username: string): Promise<User | undefined>;
  createUser(user: InsertUser): Promise<User>;

  // Car Category methods
  getAllCarCategories(): Promise<CarCategory[]>;
  getCarCategory(id: number): Promise<CarCategory | undefined>;
  createCarCategory(category: InsertCarCategory): Promise<CarCategory>;
  updateCarCategory(id: number, category: Partial<InsertCarCategory>): Promise<CarCategory | undefined>;

  // Car methods
  getAllCars(): Promise<Car[]>;
  getCar(id: number): Promise<Car | undefined>;
  getCarsByCategoryId(categoryId: number): Promise<Car[]>;
  createCar(car: InsertCar): Promise<Car>;
  updateCar(id: number, car: Partial<InsertCar>): Promise<Car | undefined>;

  // Rental methods
  getAllRentals(): Promise<Rental[]>;
  getRental(id: number): Promise<Rental | undefined>;
  getRentalsByCarId(carId: number): Promise<Rental[]>;
  createRental(rental: InsertRental): Promise<Rental>;
  updateRental(id: number, rental: Partial<InsertRental>): Promise<Rental | undefined>;

  // Transaction methods
  getAllTransactions(): Promise<Transaction[]>;
  getTransaction(id: number): Promise<Transaction | undefined>;
  getTransactionsByRentalId(rentalId: number): Promise<Transaction[]>;
  createTransaction(transaction: InsertTransaction): Promise<Transaction>;
}

export class MemStorage implements IStorage {
  private users: Map<number, User>;
  private carCategories: Map<number, CarCategory>;
  private cars: Map<number, Car>;
  private rentals: Map<number, Rental>;
  private transactions: Map<number, Transaction>;
  
  private userCurrentId: number;
  private carCategoryCurrentId: number;
  private carCurrentId: number;
  private rentalCurrentId: number;
  private transactionCurrentId: number;

  constructor() {
    this.users = new Map();
    this.carCategories = new Map();
    this.cars = new Map();
    this.rentals = new Map();
    this.transactions = new Map();
    
    this.userCurrentId = 1;
    this.carCategoryCurrentId = 1;
    this.carCurrentId = 1;
    this.rentalCurrentId = 1;
    this.transactionCurrentId = 1;

    // Initialize with sample data
    this.initializeData();
    this.initializeUsers();
  }
  
  private async initializeUsers() {
    // Add a default admin user if none exists
    if (this.users.size === 0) {
      const hashedPassword = "5b722b307fce6c944905d132691d5e4a2214b7fe92b738920eb3fce3a90420a19511c3010a0e7712b054daef5b57bad59ecbd93b3280f210578f547f4aed4d25.d931ec2ad7921b101f0a74f02a37b379";
      await this.createUser({
        username: "admin",
        email: "admin@example.com",
        password: hashedPassword,
      });
      console.log("Created default admin user");
    }
  }

  private initializeData() {
    // Create car categories
    const categories = [
      { name: "Sport Car", total: 17439 },
      { name: "SUV", total: 9478 },
      { name: "Coupe", total: 18197 },
      { name: "Hatchback", total: 12510 },
      { name: "MPV", total: 14406 }
    ];

    categories.forEach(category => {
      this.createCarCategory(category);
    });

    // Create sample cars
    const cars = [
      {
        name: "Nissan GT-R",
        categoryId: 1, // Sport Car
        description: "Twin-Turbo V6 Engine",
        features: "Sport, Performance, Premium",
        fuelCapacity: 80,
        transmission: "Manual",
        seats: 2,
        pricePerDay: 80.00,
        image: "https://images.unsplash.com/photo-1633509943968-5f3bd759c056",
        available: true
      },
      {
        name: "Porsche 911",
        categoryId: 1, // Sport Car
        description: "Flat-Six Turbo Engine",
        features: "Sport, Luxury, Performance",
        fuelCapacity: 64,
        transmission: "Auto",
        seats: 2,
        pricePerDay: 92.00,
        image: "https://images.unsplash.com/photo-1611859266238-4b082a2e0b70",
        available: true
      },
      {
        name: "Range Rover Sport",
        categoryId: 2, // SUV
        description: "V8 Supercharged Engine",
        features: "Luxury, Off-road, Premium",
        fuelCapacity: 90,
        transmission: "Auto",
        seats: 5,
        pricePerDay: 100.00,
        image: "https://images.unsplash.com/photo-1551522435-a13afa10f103",
        available: true
      },
      {
        name: "Ferrari F8",
        categoryId: 1, // Sport Car
        description: "V8 Twin-Turbocharged",
        features: "Sport, Luxury, Premium",
        fuelCapacity: 78,
        transmission: "Auto",
        seats: 2,
        pricePerDay: 120.00,
        image: "https://images.unsplash.com/photo-1592198084033-aade902d1aae",
        available: true
      },
      {
        name: "Koenigsegg",
        categoryId: 1, // Sport Car
        description: "V8 Twin-Turbo Engine",
        features: "Hypercar, Performance, Premium",
        fuelCapacity: 82,
        transmission: "Auto",
        seats: 2,
        pricePerDay: 99.00,
        image: "https://images.unsplash.com/photo-1626668893632-6f3a4466d109",
        available: true
      },
      {
        name: "Rolls-Royce",
        categoryId: 1, // Sport Car
        description: "V12 Engine",
        features: "Ultra Luxury, Premium, Comfort",
        fuelCapacity: 90,
        transmission: "Auto",
        seats: 4,
        pricePerDay: 96.00,
        image: "https://images.unsplash.com/photo-1632245889029-e406faaa34cd",
        available: true
      },
      {
        name: "CR-V",
        categoryId: 2, // SUV
        description: "Efficient SUV with ample space",
        features: "Family, Comfort, Utility",
        fuelCapacity: 65,
        transmission: "Auto",
        seats: 5,
        pricePerDay: 80.00,
        image: "https://images.unsplash.com/photo-1580273916550-e323be2ae537",
        available: true
      }
    ];

    cars.forEach(car => {
      this.createCar(car);
    });

    // Create sample rentals
    const now = new Date();
    const twoDaysFromNow = new Date(now);
    twoDaysFromNow.setDate(now.getDate() + 2);

    const rentals = [
      {
        carId: 1, // Nissan GT-R
        pickupLocation: "Kota Semarang",
        dropoffLocation: "Kota Semarang",
        pickupDate: now,
        dropoffDate: twoDaysFromNow,
        pickupTime: "07:00",
        dropoffTime: "01:00",
        totalPrice: 80.00,
        status: "active",
        createdAt: now
      }
    ];

    rentals.forEach(rental => {
      this.createRental(rental);
    });

    // Create sample transactions
    const threeDaysAgo = new Date(now);
    threeDaysAgo.setDate(now.getDate() - 3);
    
    const fourDaysAgo = new Date(now);
    fourDaysAgo.setDate(now.getDate() - 4);
    
    const fiveDaysAgo = new Date(now);
    fiveDaysAgo.setDate(now.getDate() - 5);
    
    const sixDaysAgo = new Date(now);
    sixDaysAgo.setDate(now.getDate() - 6);

    const transactions = [
      {
        rentalId: 1, // Nissan GT-R rental
        amount: 80.00,
        date: now
      },
      {
        rentalId: 1, // Using the same rental ID but different car in frontend display
        amount: 99.00,
        date: threeDaysAgo
      },
      {
        rentalId: 1, // Using the same rental ID but different car in frontend display
        amount: 96.00,
        date: fourDaysAgo
      },
      {
        rentalId: 1, // Using the same rental ID but different car in frontend display
        amount: 80.00,
        date: fiveDaysAgo
      }
    ];

    transactions.forEach(transaction => {
      this.createTransaction(transaction);
    });
  }

  // User methods
  async getUser(id: number): Promise<User | undefined> {
    return this.users.get(id);
  }

  async getUserByUsername(username: string): Promise<User | undefined> {
    return Array.from(this.users.values()).find(
      (user) => user.username === username,
    );
  }

  async createUser(insertUser: InsertUser): Promise<User> {
    const id = this.userCurrentId++;
    const user: User = { ...insertUser, id };
    this.users.set(id, user);
    return user;
  }

  // Car Category methods
  async getAllCarCategories(): Promise<CarCategory[]> {
    return Array.from(this.carCategories.values());
  }

  async getCarCategory(id: number): Promise<CarCategory | undefined> {
    return this.carCategories.get(id);
  }

  async createCarCategory(insertCategory: InsertCarCategory): Promise<CarCategory> {
    const id = this.carCategoryCurrentId++;
    // Ensure total property exists with a default value if not provided
    const category: CarCategory = { 
      ...insertCategory, 
      id,
      total: insertCategory.total ?? 0 
    };
    this.carCategories.set(id, category);
    return category;
  }

  async updateCarCategory(id: number, updateCategory: Partial<InsertCarCategory>): Promise<CarCategory | undefined> {
    const category = this.carCategories.get(id);
    if (!category) return undefined;

    const updatedCategory = { ...category, ...updateCategory };
    this.carCategories.set(id, updatedCategory);
    return updatedCategory;
  }

  // Car methods
  async getAllCars(): Promise<Car[]> {
    return Array.from(this.cars.values());
  }

  async getCar(id: number): Promise<Car | undefined> {
    return this.cars.get(id);
  }

  async getCarsByCategoryId(categoryId: number): Promise<Car[]> {
    return Array.from(this.cars.values()).filter(
      (car) => car.categoryId === categoryId,
    );
  }

  async createCar(insertCar: InsertCar): Promise<Car> {
    const id = this.carCurrentId++;
    const car: Car = { 
      ...insertCar, 
      id,
      available: insertCar.available ?? true 
    };
    this.cars.set(id, car);
    return car;
  }

  async updateCar(id: number, updateCar: Partial<InsertCar>): Promise<Car | undefined> {
    const car = this.cars.get(id);
    if (!car) return undefined;

    const updatedCar = { ...car, ...updateCar };
    this.cars.set(id, updatedCar);
    return updatedCar;
  }

  // Rental methods
  async getAllRentals(): Promise<Rental[]> {
    return Array.from(this.rentals.values());
  }

  async getRental(id: number): Promise<Rental | undefined> {
    return this.rentals.get(id);
  }

  async getRentalsByCarId(carId: number): Promise<Rental[]> {
    return Array.from(this.rentals.values()).filter(
      (rental) => rental.carId === carId,
    );
  }

  async createRental(insertRental: InsertRental): Promise<Rental> {
    const id = this.rentalCurrentId++;
    const now = new Date();
    const rental: Rental = { 
      ...insertRental, 
      id,
      status: insertRental.status ?? 'pending',
      createdAt: now 
    };
    this.rentals.set(id, rental);
    return rental;
  }

  async updateRental(id: number, updateRental: Partial<InsertRental>): Promise<Rental | undefined> {
    const rental = this.rentals.get(id);
    if (!rental) return undefined;

    const updatedRental = { ...rental, ...updateRental };
    this.rentals.set(id, updatedRental);
    return updatedRental;
  }

  // Transaction methods
  async getAllTransactions(): Promise<Transaction[]> {
    return Array.from(this.transactions.values());
  }

  async getTransaction(id: number): Promise<Transaction | undefined> {
    return this.transactions.get(id);
  }

  async getTransactionsByRentalId(rentalId: number): Promise<Transaction[]> {
    return Array.from(this.transactions.values()).filter(
      (transaction) => transaction.rentalId === rentalId,
    );
  }

  async createTransaction(insertTransaction: InsertTransaction): Promise<Transaction> {
    const id = this.transactionCurrentId++;
    const now = new Date();
    const transaction: Transaction = { 
      ...insertTransaction, 
      id,
      date: insertTransaction.date ?? now
    };
    this.transactions.set(id, transaction);
    return transaction;
  }
}

export const storage = new MemStorage();
