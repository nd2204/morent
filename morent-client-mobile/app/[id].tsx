import * as React from 'react';
import { View, ScrollView, Image } from 'react-native';
import { useLocalSearchParams, useRouter } from 'expo-router';
import { SafeAreaView } from 'react-native-safe-area-context';
import { Text } from '~/components/ui/text';
import { Button } from '~/components/ui/button';
import { Card } from '~/components/ui/card';
import { CarList } from '~/components/CarList';
import { ReviewsList } from '~/components/Review';
import { ChevronLeft, Fuel, GaugeCircle, Users } from 'lucide-react-native';
import { iconWithClassName } from '~/lib/icons/iconWithClassName';

// Register icons with NativeWind
[ChevronLeft, Fuel, GaugeCircle, Users].forEach(iconWithClassName);

const CARS_DATA = {
  '1': {
    id: '1',
    carName: "Koenigsegg",
    carType: "Sport",
    fuelCapacity: "90L",
    transmission: "Auto",
    seats: "2 People",
    pricePerDay: 99,
    imageUrl: require('~/uploads/koenigsegg.png'),
    description: "The Koenigsegg is the epitome of luxury sports cars, offering unparalleled performance and style. With its sleek design and powerful engine, it's perfect for those who demand the very best in automotive excellence."
  },
  '2': {
    id: '2',
    carName: "Nissan GT-R",
    carType: "Sport",
    fuelCapacity: "80L",
    transmission: "Auto",
    seats: "2 People",
    pricePerDay: 80,
    imageUrl: require('~/uploads/nissangtr.png'),
    description: "The Nissan GT-R, also known as Godzilla, represents the perfect blend of Japanese engineering and supercar performance. Its advanced all-wheel-drive system and twin-turbo V6 engine deliver exceptional handling and power."
  },
  '3': {
    id: '3',
    carName: "All New Rush",
    carType: "SUV",
    fuelCapacity: "70L",
    transmission: "Manual",
    seats: "6 People",
    pricePerDay: 72,
    imageUrl: require('~/uploads/allnewrush.png'),
    description: "The All New Rush combines versatility with style, perfect for family adventures or urban exploration. Its elevated driving position and spacious interior make every journey comfortable and enjoyable."
  },
  '4': {
    id: '4',
    carName: "CR-V",
    carType: "SUV",
    fuelCapacity: "80L",
    transmission: "Manual",
    seats: "6 People",
    pricePerDay: 80,
    imageUrl: require('~/uploads/crv.png'),
    description: "The CR-V sets the standard for modern SUVs with its perfect balance of comfort, style, and practicality. Advanced safety features and ample cargo space make it ideal for both daily commutes and long trips."
  },
  '5': {
    id: '5',
    carName: "All New Terios",
    carType: "SUV",
    fuelCapacity: "90L",
    transmission: "Manual",
    seats: "6 People",
    pricePerDay: 75,
    imageUrl: require('~/uploads/allnewterios.png'),
    description: "The All New Terios brings adventure to every drive with its robust design and capable performance. Its compact size makes it perfect for both city navigation and off-road exploration."
  },
};

const RECOMMENDED_CARS = [
  {
    id: '4',
    carName: "CR-V",
    carType: "SUV",
    fuelCapacity: "80L",
    transmission: "Manual",
    seats: "6 People",
    pricePerDay: 80,
    imageUrl: require('~/uploads/crv.png'),
  },
  {
    id: '5',
    carName: "All New Terios",
    carType: "SUV",
    fuelCapacity: "90L",
    transmission: "Manual",
    seats: "6 People",
    pricePerDay: 75,
    imageUrl: require('~/uploads/allnewterios.png'),
  },
];

const RECENT_CARS = [
  {
    id: '2',
    carName: "Nissan GT-R",
    carType: "Sport",
    fuelCapacity: "80L",
    transmission: "Auto",
    seats: "2 People",
    pricePerDay: 80,
    imageUrl: require('~/uploads/nissangtr.png'),
  },
  {
    id: '3',
    carName: "All New Rush",
    carType: "SUV",
    fuelCapacity: "70L",
    transmission: "Manual",
    seats: "6 People",
    pricePerDay: 72,
    imageUrl: require('~/uploads/allnewrush.png'),
  },
];

const MOCK_REVIEWS = {
  '1': [
    {
      rating: 4.8,
      comment: "Amazing car! The performance is unmatched and the service was excellent. Would definitely rent again.",
      userName: "Alex Thompson",
      userImage: "https://i.pravatar.cc/150?u=alex",
      date: "March 15, 2024"
    },
    {
      rating: 5.0,
      comment: "Best supercar experience ever! The Koenigsegg exceeded all my expectations.",
      userName: "Sarah Chen",
      userImage: "https://i.pravatar.cc/150?u=sarah",
      date: "March 10, 2024"
    }
  ],
  '2': [
    {
      rating: 4.5,
      comment: "The GT-R is a beast! Great handling and power. Rental process was smooth.",
      userName: "Mike Johnson",
      userImage: "https://i.pravatar.cc/150?u=mike",
      date: "March 20, 2024"
    }
  ],
  '3': [
    {
      rating: 4.7,
      comment: "Perfect family SUV. Spacious and comfortable for long trips.",
      userName: "Emily Parker",
      userImage: "https://i.pravatar.cc/150?u=emily",
      date: "March 18, 2024"
    }
  ],
  '4': [
    {
      rating: 4.6,
      comment: "Very reliable SUV with great features. Fuel efficient too!",
      userName: "David Wilson",
      userImage: "https://i.pravatar.cc/150?u=david",
      date: "March 12, 2024"
    }
  ],
  '5': [
    {
      rating: 4.4,
      comment: "Good car for city driving. Compact yet spacious inside.",
      userName: "Lisa Anderson",
      userImage: "https://i.pravatar.cc/150?u=lisa",
      date: "March 8, 2024"
    }
  ]
};

export default function DetailScreen() {
  const { id } = useLocalSearchParams();
  const router = useRouter();
  const car = CARS_DATA[id as keyof typeof CARS_DATA];
  const [favorites, setFavorites] = React.useState<string[]>([]);

  const toggleFavorite = (id: string) => {
    setFavorites(prev => 
      prev.includes(id) ? prev.filter(fid => fid !== id) : [...prev, id]
    );
  };

  if (!car) {
    return (
      <SafeAreaView className="flex-1 bg-background">
        <View className="flex-1 items-center justify-center">
          <Text>Car not found</Text>
        </View>
      </SafeAreaView>
    );
  }

  return (
    <SafeAreaView className="flex-1 bg-background">
      <ScrollView className='mx-2' showsVerticalScrollIndicator={false}>
        {/* Header */}
        <View className="flex-row items-center px-4 py-3 border-b border-border">
          <Button
            variant="ghost"
            size="icon"
            className="rounded-full mr-4"
            onPress={() => router.navigate("/")}
          >
            <ChevronLeft size={24} className="text-foreground" />
          </Button>
          <Text className="text-xl font-bold text-primary">Car Details</Text>
        </View>

        {/* Car Image */}
        <View className="p-4">
          <Card className="overflow-hidden bg-card p-6">
            <Image
              source={car.imageUrl}
              className="w-full h-48"
              resizeMode="contain"
              accessibilityLabel={`Image of ${car.carName}`}
            />
          </Card>
        </View>

        {/* Car Info */}
        <View className="px-4">
          <View className="flex-row justify-between items-start mb-4">
            <View>
              <Text className="text-2xl font-bold">{car.carName}</Text>
              <Text className="text-lg text-muted-foreground">{car.carType}</Text>
            </View>
            <View className="items-end">
              <Text className="text-2xl font-bold">${car.pricePerDay}</Text>
              <Text className="text-muted-foreground">/day</Text>
            </View>
          </View>

          <Text className="text-base text-muted-foreground mb-6">
            {car.description}
          </Text>

          <Card className="bg-card p-4 mb-6">
            <Text className="text-lg font-semibold mb-4">Specifications</Text>
            <View className="flex-row justify-between">
              <View className="items-center">
                <Fuel size={24} className="text-muted-foreground mb-2" />
                <Text className="text-muted-foreground">{car.fuelCapacity}</Text>
              </View>
              <View className="items-center">
                <GaugeCircle size={24} className="text-muted-foreground mb-2" />
                <Text className="text-muted-foreground">{car.transmission}</Text>
              </View>
              <View className="items-center">
                <Users size={24} className="text-muted-foreground mb-2" />
                <Text className="text-muted-foreground">{car.seats}</Text>
              </View>
            </View>
          </Card>

          {/* Reviews Section */}
          <ReviewsList
            reviews={MOCK_REVIEWS[id as keyof typeof MOCK_REVIEWS] || []}
            className="mt-6"
          />

          <CarList
            title="Recent Cars"
            cars={RECENT_CARS}
            layout="horizontal"
            favorites={favorites}
            onToggleFavorite={toggleFavorite}
            containerClassName="mt-6"
          />

          <CarList
            title="Recommended Cars"
            cars={RECOMMENDED_CARS}
            layout="horizontal"
            favorites={favorites}
            onToggleFavorite={toggleFavorite}
            containerClassName="mt-6 mb-4"
          />
        </View>
      </ScrollView>

      {/* Bottom Action */}
      <View className="p-4 border-t border-border bg-background">
        <Button 
          className="w-full" 
          onPress={() => console.log('Proceed to payment')}
        >
          <Text className="text-primary-foreground font-semibold">
            Proceed to Payment
          </Text>
        </Button>
      </View>
    </SafeAreaView>
  );
}