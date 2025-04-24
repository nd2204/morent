import * as React from 'react';
import { ScrollView, View } from 'react-native';
import { Text } from './ui/text';
import { Card } from './ui/card';
import {
  Select,
  SelectTrigger,
  SelectContent,
  SelectItem,
  type Option,
} from './ui/select';

const LOCATIONS = [
  { value: "jakarta", label: "Jakarta" },
  { value: "bandung", label: "Bandung" },
  { value: "surabaya", label: "Surabaya" },
  { value: "yogyakarta", label: "Yogyakarta" },
  { value: "semarang", label: "Semarang" }
];

const TIMES = [
  { value: "07:00", label: "07:00" },
  { value: "08:00", label: "08:00" },
  { value: "09:00", label: "09:00" },
  { value: "10:00", label: "10:00" },
  { value: "11:00", label: "11:00" },
  { value: "12:00", label: "12:00" },
  { value: "13:00", label: "13:00" },
  { value: "14:00", label: "14:00" },
  { value: "15:00", label: "15:00" },
  { value: "16:00", label: "16:00" },
  { value: "17:00", label: "17:00" },
  { value: "18:00", label: "18:00" },
];

interface RentalSectionProps {
  title: string;
  onLocationChange?: (option: Option) => void;
  onDateChange?: (option: Option) => void;
  onTimeChange?: (option: Option) => void;
}

function RentalSection({ 
  title,
  onLocationChange,
  onDateChange,
  onTimeChange
}: RentalSectionProps) {
  return (
    <Card className="flex-1 p-4 bg-card">
      <Text className="text-lg font-semibold mb-4">{title}</Text>
      
      <View className="space-y-4">
        <Select onValueChange={onLocationChange}>
          <SelectTrigger>
            <Text className="text-muted-foreground">Select City</Text>
          </SelectTrigger>
          <SelectContent>
            {LOCATIONS.map((location) => (
              <SelectItem 
                key={location.value} 
                value={location.value}
                label={location.label}
              >
                {location.label}
              </SelectItem>
            ))}
          </SelectContent>
        </Select>

        <Select onValueChange={onDateChange}>
          <SelectTrigger>
            <Text className="text-muted-foreground">Select Date</Text>
          </SelectTrigger>
          <ScrollView>
            <SelectContent>
              {[...Array(7)].map((_, i) => {
                const date = new Date();
                date.setDate(date.getDate() + i);
                const value = date.toISOString().split('T')[0];
                const label = date.toLocaleDateString('en-US', {
                  weekday: 'short',
                  month: 'short',
                  day: 'numeric',
                });
                return (
                  <SelectItem
                    key={value}
                    value={value}
                    label={label}
                  >
                    {label}
                  </SelectItem>
                );
              })}
            </SelectContent>

          </ScrollView>
        </Select>

        <Select onValueChange={onTimeChange}>
          <SelectTrigger>
            <Text className="text-muted-foreground">Select Time</Text>
          </SelectTrigger>
          <SelectContent>
            {TIMES.map((time) => (
              <SelectItem
                key={time.value}
                value={time.value}
                label={time.label}
              >
                {time.label}
              </SelectItem>
            ))}
          </SelectContent>
        </Select>
      </View>
    </Card>
  );
}

export function PickUpDropOff() {
  const [pickUpLocation, setPickUpLocation] = React.useState<Option>();
  const [pickUpDate, setPickUpDate] = React.useState<Option>();
  const [pickUpTime, setPickUpTime] = React.useState<Option>();
  const [dropOffLocation, setDropOffLocation] = React.useState<Option>();
  const [dropOffDate, setDropOffDate] = React.useState<Option>();
  const [dropOffTime, setDropOffTime] = React.useState<Option>();

  return (
    <View className="px-5 py-4 space-y-4 flex-row flex-wrap gap-4">
      <RentalSection
        title="Pick-Up"
        onLocationChange={setPickUpLocation}
        onDateChange={setPickUpDate}
        onTimeChange={setPickUpTime}
      />
      <RentalSection
        title="Drop-Off"
        onLocationChange={setDropOffLocation}
        onDateChange={setDropOffDate}
        onTimeChange={setDropOffTime}
      />
    </View>
  );
}