import * as React from 'react';
import { ScrollView, View } from 'react-native';
import { SafeAreaView } from 'react-native-safe-area-context';
import { Header } from '~/components/Header';
import { CarList } from '~/components/CarList';
import { Select, SelectTrigger, SelectContent, SelectItem, type Option } from '~/components/ui/select';
import { Text } from '~/components/ui/text';
import { useRouter } from 'expo-router';

interface OptionType {
  value: string;
  label: string;
}

// Car categories
const CATEGORIES: OptionType[] = [
  { value: 'all', label: 'All' },
  { value: 'sport', label: 'Sport' },
  { value: 'suv', label: 'SUV' },
  { value: 'mpv', label: 'MPV' },
  { value: 'sedan', label: 'Sedan' },
  { value: 'coupe', label: 'Coupe' },
  { value: 'hatchback', label: 'Hatchback' },
];

const SORT_OPTIONS: OptionType[] = [
  { value: 'price-asc', label: 'Price: Low to High' },
  { value: 'price-desc', label: 'Price: High to Low' },
];

const ALL_CARS = [
  {
    id: '1',
    carName: "Koenigsegg",
    carType: "Sport",
    fuelCapacity: "90L",
    transmission: "Auto",
    seats: "2 People",
    pricePerDay: 99,
    imageUrl: require('~/uploads/koenigsegg.png'),
  },
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

export default function CategoryScreen() {
  const router = useRouter();
  const [selectedCategory, setSelectedCategory] = React.useState<OptionType>(CATEGORIES[0]);
  const [favorites, setFavorites] = React.useState<string[]>([]);
  const [sortBy, setSortBy] = React.useState<OptionType>(SORT_OPTIONS[0]);
  const [searchQuery, setSearchQuery] = React.useState('');

  const filteredCars = React.useMemo(() => {
    let cars = [...ALL_CARS];
    
    // Filter by search query
    if (searchQuery) {
      const query = searchQuery.toLowerCase();
      cars = cars.filter(car => 
        car.carName.toLowerCase().includes(query) ||
        car.carType.toLowerCase().includes(query)
      );
    }
    
    // Filter by category
    if (selectedCategory.value !== 'all') {
      cars = cars.filter(car => 
        car.carType.toLowerCase() === selectedCategory.value.toLowerCase()
      );
    }

    // Sort cars
    cars.sort((a, b) => {
      if (sortBy.value === 'price-desc') {
        return b.pricePerDay - a.pricePerDay;
      }
      return a.pricePerDay - b.pricePerDay;
    });

    return cars;
  }, [selectedCategory.value, sortBy.value, searchQuery]);

  const handleCategoryChange = React.useCallback((value: any) => {
    const category = CATEGORIES.find(c => c.value === value);
    if (category) {
      setSelectedCategory(category);
    }
  }, []);

  const handleSortChange = React.useCallback((value: any) => {
    const sortOption = SORT_OPTIONS.find(o => o.value === value);
    if (sortOption) {
      setSortBy(sortOption);
    }
  }, []);

  const toggleFavorite = (id: string) => {
    setFavorites(prev => 
      prev.includes(id) ? prev.filter(fid => fid !== id) : [...prev, id]
    );
  };

  return (
    <SafeAreaView className="flex-1 bg-background">
      <Header 
        showSearchBar
        onPressMenu={() => router.back()}
        onSearch={setSearchQuery}
        onFilterPress={() => {}}
      />

      <View className="flex-row items-center justify-between px-4 py-3 bg-muted/50">
        <View className="flex-1 mr-2">
          <Select>
            <SelectTrigger>
              <Text className="text-foreground">{selectedCategory.label}</Text>
            </SelectTrigger>
            <SelectContent>
              {CATEGORIES.map((category) => (
                <SelectItem
                  key={category.value}
                  value={category.value}
                  label={category.label}
                  onPress={() => handleCategoryChange(category.value)}
                >
                  {category.label}
                </SelectItem>
              ))}
            </SelectContent>
          </Select>
        </View>
        <View className="flex-1 ml-2">
          <Select>
            <SelectTrigger>
              <Text className="text-foreground">{sortBy.label}</Text>
            </SelectTrigger>
            <SelectContent>
              {SORT_OPTIONS.map((option) => (
                <SelectItem
                  key={option.value}
                  value={option.value}
                  label={option.label}
                  onPress={() => handleSortChange(option.value)}
                >
                  {option.label}
                </SelectItem>
              ))}
            </SelectContent>
          </Select>
        </View>
      </View>

      <CarList
        cars={filteredCars}
        layout="vertical"
        favorites={favorites}
        onToggleFavorite={toggleFavorite}
        showViewAll={false}
        containerClassName="px-4 py-4 mb-20 pb-20"
      />
    </SafeAreaView>
  );
}