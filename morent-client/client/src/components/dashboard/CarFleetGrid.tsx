import React from "react";
import { Link } from "wouter";
import { Car } from "@/lib/types";
import { Card, CardContent } from "@/components/ui/card";
import { Fuel, Gauge, Users } from "lucide-react";
import { Button } from "@/components/ui/button";
import { Badge } from "@/components/ui/badge";

interface CarFleetGridProps {
  cars: Car[];
}

const CarFleetGrid: React.FC<CarFleetGridProps> = ({ cars }) => {
  return (
    <div className="mt-8">
      <h2 className="font-semibold text-xl mb-6 dark:text-white">Car Fleet</h2>
      
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-6">
        {cars.map((car) => (
          <Card key={car.id} className="bg-white dark:bg-slate-800 rounded-xl shadow-sm overflow-hidden">
            <CardContent className="p-4">
              <div className="flex items-center justify-between mb-3">
                <h3 className="font-semibold dark:text-white">{car.name}</h3>
                <Badge variant="outline" className="bg-blue-100 text-primary border-none">
                  {car.category}
                </Badge>
              </div>
              <p className="text-sm text-gray-500 dark:text-gray-400 mb-4">{car.description}</p>
              <div className="relative h-40 rounded-lg overflow-hidden mb-4">
                <img 
                  src={car.image} 
                  alt={car.name} 
                  className="w-full h-full object-cover"
                />
              </div>
              <div className="flex items-center justify-between mb-4">
                <div className="flex items-center text-sm text-gray-500 dark:text-gray-400">
                  <Fuel className="h-4 w-4 mr-1.5" />
                  <span>{car.fuelCapacity}L</span>
                </div>
                <div className="flex items-center text-sm text-gray-500 dark:text-gray-400">
                  <Gauge className="h-4 w-4 mr-1.5" />
                  <span>{car.transmission}</span>
                </div>
                <div className="flex items-center text-sm text-gray-500 dark:text-gray-400">
                  <Users className="h-4 w-4 mr-1.5" />
                  <span>{car.seats} People</span>
                </div>
              </div>
              <div className="flex items-center justify-between">
                <div>
                  <p className="font-semibold text-lg dark:text-white">
                    ${car.pricePerDay.toFixed(2)}
                    <span className="text-sm font-normal text-gray-500 dark:text-gray-400">/day</span>
                  </p>
                </div>
                <Link href={`/car/${car.id}`}>
                  <Button className="bg-primary hover:bg-blue-600 text-white rounded-lg px-4 py-2 text-sm">
                    View Details
                  </Button>
                </Link>
              </div>
            </CardContent>
          </Card>
        ))}
      </div>
    </div>
  );
};

export default CarFleetGrid;
