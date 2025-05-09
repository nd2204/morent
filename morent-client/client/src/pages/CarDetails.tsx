import React from "react";
import { useQuery } from "@tanstack/react-query";
import { useRoute } from "wouter";
import RentalDetailsCard from "@/components/dashboard/RentalDetailsCard";
import { Button } from "@/components/ui/button";
import { Skeleton } from "@/components/ui/skeleton";
import { 
  Card, 
  CardContent, 
  CardHeader, 
  CardTitle, 
  CardDescription 
} from "@/components/ui/card";
import { Fuel, Gauge, Users, Check } from "lucide-react";
import { transformCarData } from "@/lib/utils";

interface CarDetailsProps {
  id?: string;
}

const CarDetails: React.FC<CarDetailsProps> = ({ id }) => {
  const [, params] = useRoute("/car/:id");
  const carId = id ? parseInt(id) : params?.id ? parseInt(params.id) : null;

  const { data, isLoading, error } = useQuery({
    queryKey: [`/api/cars/${carId}`],
    enabled: !!carId,
  });

  if (isLoading) {
    return <CarDetailsSkeleton />;
  }

  if (error || !data) {
    return (
      <div className="w-full p-6 text-center">
        <p className="text-red-500">Error loading car details</p>
      </div>
    );
  }

  const car = transformCarData(data);
  const features = car.features.split(", ");

  return (
    <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
      {/* Car Details */}
      <Card className="bg-white dark:bg-slate-800 rounded-xl shadow-sm overflow-hidden">
        <CardHeader>
          <CardTitle className="font-semibold text-xl">{car.name}</CardTitle>
          <CardDescription>{car.description}</CardDescription>
        </CardHeader>
        <CardContent>
          <div className="relative h-60 rounded-lg overflow-hidden mb-6">
            <img 
              src={car.image} 
              alt={car.name} 
              className="w-full h-full object-cover"
            />
          </div>
          
          <div className="flex items-center justify-between mb-6 p-4 bg-gray-50 dark:bg-slate-700 rounded-lg">
            <div className="flex items-center text-sm">
              <Fuel className="h-5 w-5 mr-2 text-primary" />
              <div>
                <p className="font-medium dark:text-white">{car.fuelCapacity}L</p>
                <p className="text-xs text-gray-500 dark:text-gray-400">Fuel Capacity</p>
              </div>
            </div>
            <div className="flex items-center text-sm">
              <Gauge className="h-5 w-5 mr-2 text-primary" />
              <div>
                <p className="font-medium dark:text-white">{car.transmission}</p>
                <p className="text-xs text-gray-500 dark:text-gray-400">Transmission</p>
              </div>
            </div>
            <div className="flex items-center text-sm">
              <Users className="h-5 w-5 mr-2 text-primary" />
              <div>
                <p className="font-medium dark:text-white">{car.seats} People</p>
                <p className="text-xs text-gray-500 dark:text-gray-400">Capacity</p>
              </div>
            </div>
          </div>
          
          <h3 className="font-semibold text-lg mb-3 dark:text-white">Features</h3>
          <div className="grid grid-cols-2 gap-2 mb-6">
            {features.map((feature, index) => (
              <div key={index} className="flex items-center">
                <Check className="h-4 w-4 text-primary mr-2" />
                <span className="text-sm dark:text-gray-300">{feature}</span>
              </div>
            ))}
          </div>
          
          <div className="flex items-center justify-between">
            <div>
              <p className="text-gray-500 dark:text-gray-400 text-sm">Rental Price</p>
              <p className="font-semibold text-2xl dark:text-white">
                ${car.pricePerDay.toFixed(2)}
                <span className="text-sm font-normal text-gray-500 dark:text-gray-400">/day</span>
              </p>
            </div>
            <Button className="bg-primary hover:bg-blue-600 text-white px-6">
              Rent Now
            </Button>
          </div>
        </CardContent>
      </Card>
      
      {/* Rental Details */}
      <RentalDetailsCard car={car} />
    </div>
  );
};

const CarDetailsSkeleton: React.FC = () => {
  return (
    <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
      {/* Car Details Skeleton */}
      <div className="bg-white dark:bg-slate-800 rounded-xl shadow-sm p-6">
        <Skeleton className="h-8 w-48 mb-2" />
        <Skeleton className="h-4 w-full mb-6" />
        <Skeleton className="h-60 w-full rounded-lg mb-6" />
        
        <div className="flex items-center justify-between mb-6 p-4 bg-gray-50 dark:bg-slate-700 rounded-lg">
          <div className="flex items-center">
            <Skeleton className="h-5 w-5 mr-2 rounded-full" />
            <div>
              <Skeleton className="h-5 w-16 mb-1" />
              <Skeleton className="h-3 w-24" />
            </div>
          </div>
          <div className="flex items-center">
            <Skeleton className="h-5 w-5 mr-2 rounded-full" />
            <div>
              <Skeleton className="h-5 w-16 mb-1" />
              <Skeleton className="h-3 w-24" />
            </div>
          </div>
          <div className="flex items-center">
            <Skeleton className="h-5 w-5 mr-2 rounded-full" />
            <div>
              <Skeleton className="h-5 w-16 mb-1" />
              <Skeleton className="h-3 w-24" />
            </div>
          </div>
        </div>
        
        <Skeleton className="h-6 w-24 mb-3" />
        <div className="grid grid-cols-2 gap-2 mb-6">
          {[1, 2, 3, 4, 5, 6].map((i) => (
            <div key={i} className="flex items-center">
              <Skeleton className="h-4 w-4 mr-2 rounded-full" />
              <Skeleton className="h-4 w-24" />
            </div>
          ))}
        </div>
        
        <div className="flex items-center justify-between">
          <div>
            <Skeleton className="h-4 w-20 mb-1" />
            <Skeleton className="h-8 w-32" />
          </div>
          <Skeleton className="h-10 w-28 rounded-lg" />
        </div>
      </div>
      
      {/* Rental Details Card Skeleton */}
      <div className="bg-white dark:bg-slate-800 rounded-xl shadow-sm p-6">
        <Skeleton className="h-6 w-48 mb-4" />
        <Skeleton className="h-48 w-full mb-6" />
        <div className="flex items-start space-x-4 mb-6">
          <Skeleton className="w-24 h-16 rounded-lg" />
          <div className="space-y-2">
            <Skeleton className="h-5 w-32" />
            <Skeleton className="h-4 w-20" />
          </div>
        </div>
        <div className="space-y-4 mb-6">
          <div>
            <div className="flex items-center mb-2">
              <Skeleton className="w-4 h-4 rounded-full mr-3" />
              <Skeleton className="h-4 w-20" />
            </div>
            <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
              <Skeleton className="h-10 w-full rounded-lg" />
              <Skeleton className="h-10 w-full rounded-lg" />
              <Skeleton className="h-10 w-full rounded-lg" />
            </div>
          </div>
          <div>
            <div className="flex items-center mb-2">
              <Skeleton className="w-4 h-4 rounded-full mr-3" />
              <Skeleton className="h-4 w-20" />
            </div>
            <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
              <Skeleton className="h-10 w-full rounded-lg" />
              <Skeleton className="h-10 w-full rounded-lg" />
              <Skeleton className="h-10 w-full rounded-lg" />
            </div>
          </div>
        </div>
        <Skeleton className="h-6 w-40 mb-2" />
        <Skeleton className="h-4 w-60 mb-2" />
        <Skeleton className="h-8 w-24" />
      </div>
    </div>
  );
};

export default CarDetails;
