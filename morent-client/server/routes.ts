import express, { type Express, Request, Response, NextFunction } from "express";
import { createServer, type Server } from "http";
import { storage } from "./storage";
import { z } from "zod";
import { insertCarSchema, insertCarCategorySchema, insertRentalSchema, insertTransactionSchema } from "@shared/schema";
import { setupAuth } from "./auth";

export async function registerRoutes(app: Express): Promise<Server> {
  // Setup authentication
  setupAuth(app);
  
  // Authentication middleware for protected routes
  const requireAuth = (req: Request, res: Response, next: NextFunction) => {
    if (!req.isAuthenticated()) {
      return res.status(401).json({ message: "Unauthorized - Please log in" });
    }
    next();
  };
  
  // API Routes
  const apiRouter = express.Router();
  app.use("/api", apiRouter);

  // Car Categories Routes
  apiRouter.get("/car-categories", async (req, res) => {
    try {
      const categories = await storage.getAllCarCategories();
      res.json(categories);
    } catch (error) {
      res.status(500).json({ message: "Error fetching car categories" });
    }
  });

  apiRouter.get("/car-categories/:id", async (req, res) => {
    try {
      const id = parseInt(req.params.id);
      if (isNaN(id)) {
        return res.status(400).json({ message: "Invalid category ID" });
      }

      const category = await storage.getCarCategory(id);
      if (!category) {
        return res.status(404).json({ message: "Category not found" });
      }

      res.json(category);
    } catch (error) {
      res.status(500).json({ message: "Error fetching category" });
    }
  });

  // Cars Routes
  apiRouter.get("/cars", async (req, res) => {
    try {
      const cars = await storage.getAllCars();
      res.json(cars);
    } catch (error) {
      res.status(500).json({ message: "Error fetching cars" });
    }
  });

  apiRouter.get("/cars/:id", async (req, res) => {
    try {
      const id = parseInt(req.params.id);
      if (isNaN(id)) {
        return res.status(400).json({ message: "Invalid car ID" });
      }

      const car = await storage.getCar(id);
      if (!car) {
        return res.status(404).json({ message: "Car not found" });
      }

      res.json(car);
    } catch (error) {
      res.status(500).json({ message: "Error fetching car" });
    }
  });

  apiRouter.get("/cars/category/:categoryId", async (req, res) => {
    try {
      const categoryId = parseInt(req.params.categoryId);
      if (isNaN(categoryId)) {
        return res.status(400).json({ message: "Invalid category ID" });
      }

      const cars = await storage.getCarsByCategoryId(categoryId);
      res.json(cars);
    } catch (error) {
      res.status(500).json({ message: "Error fetching cars by category" });
    }
  });

  // Rentals Routes
  apiRouter.get("/rentals", async (req, res) => {
    try {
      const rentals = await storage.getAllRentals();
      res.json(rentals);
    } catch (error) {
      res.status(500).json({ message: "Error fetching rentals" });
    }
  });

  apiRouter.get("/rentals/:id", async (req, res) => {
    try {
      const id = parseInt(req.params.id);
      if (isNaN(id)) {
        return res.status(400).json({ message: "Invalid rental ID" });
      }

      const rental = await storage.getRental(id);
      if (!rental) {
        return res.status(404).json({ message: "Rental not found" });
      }

      res.json(rental);
    } catch (error) {
      res.status(500).json({ message: "Error fetching rental" });
    }
  });

  apiRouter.post("/rentals", async (req, res) => {
    try {
      const validatedData = insertRentalSchema.parse(req.body);
      const rental = await storage.createRental(validatedData);
      res.status(201).json(rental);
    } catch (error) {
      if (error instanceof z.ZodError) {
        return res.status(400).json({ message: "Invalid rental data", errors: error.errors });
      }
      res.status(500).json({ message: "Error creating rental" });
    }
  });

  // Transactions Routes
  apiRouter.get("/transactions", async (req, res) => {
    try {
      const transactions = await storage.getAllTransactions();
      res.json(transactions);
    } catch (error) {
      res.status(500).json({ message: "Error fetching transactions" });
    }
  });

  // Dashboard data route
  apiRouter.get("/dashboard", async (req, res) => {
    try {
      const [categories, cars, rentals, transactionsData] = await Promise.all([
        storage.getAllCarCategories(),
        storage.getAllCars(),
        storage.getAllRentals(),
        storage.getAllTransactions()
      ]);

      // Find a featured car (first available car or just the first car)
      const featuredCar = cars.find(car => car.available) || cars[0];

      // Add count property to categories
      const categoriesWithCount = categories.map(category => {
        const count = cars.filter(car => car.categoryId === category.id).length;
        return { ...category, count };
      });

      // Add car details to transactions
      const transactions = transactionsData.map((transaction, index) => {
        // For demo purposes, use different cars for each transaction 
        // to show variety in the transactions list
        const carIndex = index % cars.length;
        const car = cars[carIndex];
        
        return {
          ...transaction,
          car: {
            id: car.id,
            name: car.name,
            category: categories.find(c => c.id === car.categoryId)?.name || 'Unknown',
            image: car.image
          }
        };
      });

      res.json({
        categories: categoriesWithCount,
        cars,
        rentals,
        transactions,
        totalRentals: rentals.length,
        totalCars: cars.length,
        featuredCar
      });
    } catch (error) {
      res.status(500).json({ message: "Error fetching dashboard data" });
    }
  });

  const httpServer = createServer(app);
  return httpServer;
}
