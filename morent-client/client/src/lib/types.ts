export interface Car {
  id: number;
  name: string;
  category: string;
  categoryId: number;
  description: string;
  features: string;
  fuelCapacity: number;
  transmission: string;
  seats: number;
  pricePerDay: number;
  image: string;
  available: boolean;
}

export interface CarCategory {
  id: number;
  name: string;
  count: number;
}

export interface Transaction {
  id: number;
  rentalId: number;
  amount: number;
  date: string;
  car: {
    id: number;
    name: string;
    category: string;
    image: string;
  };
}

export interface Rental {
  id: number;
  carId: number;
  pickupLocation: string;
  dropoffLocation: string;
  pickupDate: string;
  dropoffDate: string;
  pickupTime: string;
  dropoffTime: string;
  totalPrice: number;
  status: string;
  createdAt: string;
}

export interface DashboardData {
  categories: CarCategory[];
  cars: Car[];
  rentals: Rental[];
  transactions: Transaction[];
  totalRentals: number;
  totalCars: number;
  featuredCar: Car;
}
