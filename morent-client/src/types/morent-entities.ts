// Auto-generated - Do not modify
// Generated on: 2025-04-11 10:40:02

export interface MorentLocation {
  Id: number;
  Address: string;
  City: string;
  State: string;
  ZipCode: string;
}

export interface MorentRentalDetail {
  Id: number;
  Rental: MorentRental;
  PickupLocationId: number;
  DropoffLocationId: number;
  PickupDateTime: Date;
}

export interface MorentImage {
  Id: string;
  FileName: string;
  Url: string;
  UploadedAt: Date;
  CarModelId: number | null;
  UserId: number | null;
}

export interface MorentUser {
  Id: string;
  Name: string;
  Email: string;
  PasswordHash: string;
  Role: string;
  Rentals: Array<MorentRental>;
  Reviews: Array<MorentReview>;
  Favorites: Array<MorentFavorite>;
}

export interface MorentFavorite {
  UserId: number;
  CarId: number;
}

export interface MorentPayment {
  Id: number;
  Rental: MorentRental;
  Amount: number;
  PaymentStatus: string;
}

export interface MorentCarType {
  Id: number;
  CarModels: Array<MorentCarModel>;
}

export interface MorentCarModel {
  Id: number;
  Brand: string;
  Model: string;
  FuelCapacityLitter: number;
  PricePerDay: number;
  CarType: MorentCarType;
  Image: Array<MorentImage>;
  Cars: Array<MorentCar>;
}

export interface MorentCar {
  Id: number;
  Status: string;
  Rentals: Array<MorentRental>;
  Reviews: Array<MorentReview>;
  Favorites: Array<MorentFavorite>;
}

export interface MorentReview {
  Id: number;
  User: MorentUser;
  CarId: number;
  Rating: number;
}

export interface MorentRental {
  Id: number;
  UserId: number;
  CarId: number;
  RentalDetail: MorentRentalDetail;
}

