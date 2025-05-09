import { pgTable, text, serial, integer, boolean, timestamp, varchar, real } from "drizzle-orm/pg-core";
import { createInsertSchema } from "drizzle-zod";
import { z } from "zod";

export const users = pgTable("users", {
  id: serial("id").primaryKey(),
  username: text("username").notNull().unique(),
  password: text("password").notNull(),
});

export const insertUserSchema = createInsertSchema(users).pick({
  username: true,
  password: true,
});

export const carCategories = pgTable("car_categories", {
  id: serial("id").primaryKey(),
  name: text("name").notNull(),
  total: integer("total").notNull().default(0),
});

export const insertCarCategorySchema = createInsertSchema(carCategories).pick({
  name: true,
  total: true,
});

export const cars = pgTable("cars", {
  id: serial("id").primaryKey(),
  name: text("name").notNull(),
  categoryId: integer("category_id").notNull(),
  description: text("description").notNull(),
  features: text("features").notNull(),
  fuelCapacity: integer("fuel_capacity").notNull(),
  transmission: text("transmission").notNull(),
  seats: integer("seats").notNull(),
  pricePerDay: real("price_per_day").notNull(),
  image: text("image").notNull(),
  available: boolean("available").notNull().default(true),
});

export const insertCarSchema = createInsertSchema(cars).pick({
  name: true,
  categoryId: true,
  description: true,
  features: true,
  fuelCapacity: true,
  transmission: true,
  seats: true,
  pricePerDay: true,
  image: true,
  available: true,
});

export const rentals = pgTable("rentals", {
  id: serial("id").primaryKey(),
  carId: integer("car_id").notNull(),
  pickupLocation: text("pickup_location").notNull(),
  dropoffLocation: text("dropoff_location").notNull(),
  pickupDate: timestamp("pickup_date").notNull(),
  dropoffDate: timestamp("dropoff_date").notNull(),
  pickupTime: text("pickup_time").notNull(),
  dropoffTime: text("dropoff_time").notNull(),
  totalPrice: real("total_price").notNull(),
  status: text("status").notNull().default("pending"),
  createdAt: timestamp("created_at").notNull().defaultNow(),
});

export const insertRentalSchema = createInsertSchema(rentals).pick({
  carId: true,
  pickupLocation: true,
  dropoffLocation: true,
  pickupDate: true,
  dropoffDate: true,
  pickupTime: true,
  dropoffTime: true,
  totalPrice: true,
  status: true,
});

export const transactions = pgTable("transactions", {
  id: serial("id").primaryKey(),
  rentalId: integer("rental_id").notNull(),
  amount: real("amount").notNull(),
  date: timestamp("date").notNull().defaultNow(),
});

export const insertTransactionSchema = createInsertSchema(transactions).pick({
  rentalId: true,
  amount: true,
  date: true,
});

export type User = typeof users.$inferSelect;
export type InsertUser = z.infer<typeof insertUserSchema>;

export type CarCategory = typeof carCategories.$inferSelect;
export type InsertCarCategory = z.infer<typeof insertCarCategorySchema>;

export type Car = typeof cars.$inferSelect;
export type InsertCar = z.infer<typeof insertCarSchema>;

export type Rental = typeof rentals.$inferSelect;
export type InsertRental = z.infer<typeof insertRentalSchema>;

export type Transaction = typeof transactions.$inferSelect;
export type InsertTransaction = z.infer<typeof insertTransactionSchema>;
