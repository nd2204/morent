import React, { useEffect, useState } from "react";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select";
import MapComponent from "@/components/map/MapView";
import { Check, ChevronDown } from "lucide-react";
import { Car } from "@/lib/types";

interface RentalDetailsCardProps {
  car: Car;
}

const RentalDetailsCard: React.FC<RentalDetailsCardProps> = ({ car }) => {
  const [pickupLocation, setPickupLocation] = useState("Kota Semarang");
  const [dropoffLocation, setDropoffLocation] = useState("Kota Semarang");
  const [pickupDate, setPickupDate] = useState("20 July 2022");
  const [dropoffDate, setDropoffDate] = useState("21 July 2022");
  const [pickupTime, setPickupTime] = useState("07:00");
  const [dropoffTime, setDropoffTime] = useState("01:00");

  const [position, setPosition] = useState<[number, number] | [number, number]>([21.028511, 105.804817]); // Hà Nội
  useEffect(() => {
    if (!navigator.geolocation) {
      alert('Trình duyệt không hỗ trợ định vị.');
      return;
    }

    navigator.geolocation.getCurrentPosition(
      (pos) => {
        const { latitude, longitude } = pos.coords;
        setPosition([latitude, longitude]);
      },
      (err) => {
        console.error(err);
        alert('Không lấy được vị trí người dùng.');
      }
    );
  }, []);

  return (
    <Card className="bg-white dark:bg-slate-800 rounded-xl shadow-sm overflow-hidden">
      <CardHeader>
        <CardTitle className="font-semibold text-xl mb-2">Details Rental</CardTitle>
      </CardHeader>
      <CardContent>
        {/* Map Section */}
        <div className="h-48 mb-6 overflow-hidden rounded-lg">
          <MapComponent center={position} /> {/* Hà Nội */}
        </div>

        {/* Car Info Section */}
        <div className="flex items-start space-x-4 mb-6">
          <img
            src={car.image}
            className="w-24 h-16 object-cover rounded-lg"
            alt={car.name}
          />
          <div>
            <h3 className="font-semibold text-lg dark:text-white">{car.name}</h3>
            <p className="text-primary font-medium text-sm">{car.category}</p>
            <p className="text-gray-500 text-xs mt-1 dark:text-gray-400">#{car.id}</p>
          </div>
        </div>

        {/* Pick-up & Drop-off */}
        <div className="space-y-4 mb-6">
          {/* Pick-up Section */}
          <div>
            <div className="flex items-center mb-2">
              <div className="w-4 h-4 rounded-full bg-primary flex items-center justify-center mr-3">
                <div className="w-2 h-2 rounded-full bg-white"></div>
              </div>
              <p className="font-medium text-sm dark:text-white">Pick - Up</p>
            </div>

            <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
              <div>
                <p className="text-xs text-gray-500 dark:text-gray-400 mb-1">Locations</p>
                <Select value={pickupLocation} onValueChange={setPickupLocation}>
                  <SelectTrigger className="border border-gray-200 dark:border-slate-700 rounded-lg">
                    <SelectValue placeholder="Select location" />
                  </SelectTrigger>
                  <SelectContent>
                    <SelectItem value="Kota Semarang">Kota Semarang</SelectItem>
                    <SelectItem value="Jakarta">Jakarta</SelectItem>
                    <SelectItem value="Surabaya">Surabaya</SelectItem>
                    <SelectItem value="Bandung">Bandung</SelectItem>
                  </SelectContent>
                </Select>
              </div>
              <div>
                <p className="text-xs text-gray-500 dark:text-gray-400 mb-1">Date</p>
                <Select value={pickupDate} onValueChange={setPickupDate}>
                  <SelectTrigger className="border border-gray-200 dark:border-slate-700 rounded-lg">
                    <SelectValue placeholder="Select date" />
                  </SelectTrigger>
                  <SelectContent>
                    <SelectItem value="20 July 2022">20 July 2022</SelectItem>
                    <SelectItem value="21 July 2022">21 July 2022</SelectItem>
                    <SelectItem value="22 July 2022">22 July 2022</SelectItem>
                    <SelectItem value="23 July 2022">23 July 2022</SelectItem>
                  </SelectContent>
                </Select>
              </div>
              <div>
                <p className="text-xs text-gray-500 dark:text-gray-400 mb-1">Time</p>
                <Select value={pickupTime} onValueChange={setPickupTime}>
                  <SelectTrigger className="border border-gray-200 dark:border-slate-700 rounded-lg">
                    <SelectValue placeholder="Select time" />
                  </SelectTrigger>
                  <SelectContent>
                    <SelectItem value="07:00">07:00</SelectItem>
                    <SelectItem value="08:00">08:00</SelectItem>
                    <SelectItem value="09:00">09:00</SelectItem>
                    <SelectItem value="10:00">10:00</SelectItem>
                  </SelectContent>
                </Select>
              </div>
            </div>
          </div>

          {/* Drop-off Section */}
          <div>
            <div className="flex items-center mb-2">
              <div className="w-4 h-4 rounded-full bg-primary flex items-center justify-center mr-3">
                <div className="w-2 h-2 rounded-full bg-white"></div>
              </div>
              <p className="font-medium text-sm dark:text-white">Drop - Off</p>
            </div>

            <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
              <div>
                <p className="text-xs text-gray-500 dark:text-gray-400 mb-1">Locations</p>
                <Select value={dropoffLocation} onValueChange={setDropoffLocation}>
                  <SelectTrigger className="border border-gray-200 dark:border-slate-700 rounded-lg">
                    <SelectValue placeholder="Select location" />
                  </SelectTrigger>
                  <SelectContent>
                    <SelectItem value="Kota Semarang">Kota Semarang</SelectItem>
                    <SelectItem value="Jakarta">Jakarta</SelectItem>
                    <SelectItem value="Surabaya">Surabaya</SelectItem>
                    <SelectItem value="Bandung">Bandung</SelectItem>
                  </SelectContent>
                </Select>
              </div>
              <div>
                <p className="text-xs text-gray-500 dark:text-gray-400 mb-1">Date</p>
                <Select value={dropoffDate} onValueChange={setDropoffDate}>
                  <SelectTrigger className="border border-gray-200 dark:border-slate-700 rounded-lg">
                    <SelectValue placeholder="Select date" />
                  </SelectTrigger>
                  <SelectContent>
                    <SelectItem value="20 July 2022">20 July 2022</SelectItem>
                    <SelectItem value="21 July 2022">21 July 2022</SelectItem>
                    <SelectItem value="22 July 2022">22 July 2022</SelectItem>
                    <SelectItem value="23 July 2022">23 July 2022</SelectItem>
                  </SelectContent>
                </Select>
              </div>
              <div>
                <p className="text-xs text-gray-500 dark:text-gray-400 mb-1">Time</p>
                <Select value={dropoffTime} onValueChange={setDropoffTime}>
                  <SelectTrigger className="border border-gray-200 dark:border-slate-700 rounded-lg">
                    <SelectValue placeholder="Select time" />
                  </SelectTrigger>
                  <SelectContent>
                    <SelectItem value="01:00">01:00</SelectItem>
                    <SelectItem value="02:00">02:00</SelectItem>
                    <SelectItem value="03:00">03:00</SelectItem>
                    <SelectItem value="04:00">04:00</SelectItem>
                  </SelectContent>
                </Select>
              </div>
            </div>
          </div>
        </div>

        {/* Total Price Section */}
        <div>
          <h3 className="font-semibold text-lg mb-1 dark:text-white">Total Rental Price</h3>
          <p className="text-sm text-gray-500 dark:text-gray-400 mb-2">Overall price and includes rental discount</p>
          <p className="text-2xl font-bold dark:text-white">${car.pricePerDay.toFixed(2)}</p>
        </div>
      </CardContent>
    </Card>
  );
};

export default RentalDetailsCard;
