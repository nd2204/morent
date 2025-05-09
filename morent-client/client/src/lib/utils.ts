import { clsx, type ClassValue } from "clsx"
import { twMerge } from "tailwind-merge"
import { format, parse } from 'date-fns'
import { Car, DashboardData } from "./types"
import { DATE_FORMAT } from "./constants"

export function cn(...inputs: ClassValue[]) {
  return twMerge(clsx(inputs))
}

// Format date string to readable format
export function formatDate(dateString: string): string {
  try {
    // Check if it's already a formatted date (e.g., "21 July 2022") or ISO date
    if (dateString.includes(" ")) {
      return dateString;
    }
    
    const date = new Date(dateString);
    return format(date, DATE_FORMAT);
  } catch (error) {
    console.error("Error formatting date:", error);
    return dateString;
  }
}

// Transform car data from API to frontend format
export function transformCarData(data: any): Car {
  // If data is already in the right format, return as is
  if (data && typeof data === 'object' && 'id' in data) {
    return data as Car;
  }
  
  // Handle error case or empty data
  if (!data) {
    // Return default car data
    return {
      id: 0,
      name: "Unknown Car",
      category: "Unknown",
      categoryId: 0,
      description: "No description available",
      features: "No features available",
      fuelCapacity: 0,
      transmission: "N/A",
      seats: 0,
      pricePerDay: 0,
      image: "https://images.unsplash.com/photo-1552519507-da3b142c6e3d",
      available: false
    };
  }
  
  return data as Car;
}

// Transform dashboard data from API to frontend format
export function transformDashboardData(data: any): DashboardData {
  // If data is already in the right format, return as is
  if (data && typeof data === 'object' && 'categories' in data) {
    return data as DashboardData;
  }
  
  // Handle error case or empty data
  if (!data) {
    // Return mock data for development purposes
    return {
      categories: [
        { id: 1, name: "Sport Car", count: 64 },
        { id: 2, name: "SUV", count: 47 },
        { id: 3, name: "Coupe", count: 24 },
        { id: 4, name: "Hatchback", count: 15 },
        { id: 5, name: "MPV", count: 8 }
      ],
      cars: [
        {
          id: 1,
          name: "Koenigsegg",
          category: "Sport",
          categoryId: 1,
          description: "Sport car with best performance and high speed capability.",
          features: "Speed, Comfort, Luxury, Performance",
          fuelCapacity: 80,
          transmission: "Manual",
          seats: 2,
          pricePerDay: 99.0,
          image: "https://images.unsplash.com/photo-1633509943968-5f3bd759c056",
          available: true
        },
        {
          id: 2,
          name: "Nissan GT-R",
          category: "Sport",
          categoryId: 1,
          description: "Sport car with excellent acceleration and handling.",
          features: "Performance, Speed, Handling, Premium",
          fuelCapacity: 75,
          transmission: "Automatic",
          seats: 2,
          pricePerDay: 80.0,
          image: "https://images.unsplash.com/photo-1611859266238-4b082a2e0b70",
          available: true
        },
        {
          id: 3,
          name: "Roll-Royce",
          category: "SUV",
          categoryId: 2,
          description: "Luxury SUV with spacious interior and premium features.",
          features: "Luxury, Comfort, Premium, Spacious",
          fuelCapacity: 90,
          transmission: "Automatic",
          seats: 5,
          pricePerDay: 96.0,
          image: "https://images.unsplash.com/photo-1551522435-a13afa10f103",
          available: true
        },
        {
          id: 4,
          name: "CR-V",
          category: "SUV",
          categoryId: 2,
          description: "Family-friendly SUV with great utility and comfort.",
          features: "Family, Utility, Comfort, Off-road",
          fuelCapacity: 65,
          transmission: "Automatic",
          seats: 5,
          pricePerDay: 50.0,
          image: "https://images.unsplash.com/photo-1580273916550-e323be2ae537",
          available: false
        }
      ],
      rentals: [
        {
          id: 1,
          carId: 1,
          pickupLocation: "Kota Semarang",
          dropoffLocation: "Jakarta",
          pickupDate: "20 July 2022",
          dropoffDate: "21 July 2022",
          pickupTime: "07:00",
          dropoffTime: "01:00",
          totalPrice: 99.0,
          status: "Active",
          createdAt: "2022-07-19T08:00:00Z"
        }
      ],
      transactions: [
        {
          id: 1,
          rentalId: 1,
          amount: 99.0,
          date: "19 July 2022",
          car: {
            id: 1,
            name: "Koenigsegg",
            category: "Sport",
            image: "https://images.unsplash.com/photo-1633509943968-5f3bd759c056"
          }
        },
        {
          id: 2,
          rentalId: 2,
          amount: 80.0,
          date: "18 July 2022",
          car: {
            id: 2,
            name: "Nissan GT-R",
            category: "Sport",
            image: "https://images.unsplash.com/photo-1611859266238-4b082a2e0b70"
          }
        },
        {
          id: 3,
          rentalId: 3,
          amount: 96.0,
          date: "17 July 2022",
          car: {
            id: 3,
            name: "Roll-Royce",
            category: "SUV",
            image: "https://images.unsplash.com/photo-1551522435-a13afa10f103"
          }
        },
        {
          id: 4,
          rentalId: 4,
          amount: 50.0,
          date: "16 July 2022",
          car: {
            id: 4,
            name: "CR-V",
            category: "SUV",
            image: "https://images.unsplash.com/photo-1580273916550-e323be2ae537"
          }
        }
      ],
      totalRentals: 158,
      totalCars: 64,
      featuredCar: {
        id: 1,
        name: "Koenigsegg",
        category: "Sport",
        categoryId: 1,
        description: "Sport car with best performance and high speed capability.",
        features: "Speed, Comfort, Luxury, Performance",
        fuelCapacity: 80,
        transmission: "Manual",
        seats: 2,
        pricePerDay: 99.0,
        image: "https://images.unsplash.com/photo-1633509943968-5f3bd759c056",
        available: true
      }
    };
  }
  
  return data as DashboardData;
}
