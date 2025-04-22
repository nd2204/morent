import * as React from 'react';
import { SectionList, View } from 'react-native';
import { Header } from '~/components/Header';
import { Hero } from '~/components/Hero';
import { PickUpDropOff } from '~/components/PickUpDropOff';
import { CarList } from '~/components/CarList';
import { SafeAreaView } from 'react-native-safe-area-context';

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
  },
};

const POPULAR_CARS = [
  CARS_DATA['1'],
  CARS_DATA['2'],
  CARS_DATA['3'],
];

const RECOMMENDED_CARS = [
  CARS_DATA['4'],
  CARS_DATA['5'],
];

const SECTIONS = [
  {
    type: 'hero',
    data: [null]
  },
  {
    type: 'pickUpDropOff',
    data: [null]
  },
  {
    type: 'popular',
    data: [POPULAR_CARS]
  },
  {
    type: 'recommended',
    data: [RECOMMENDED_CARS]
  }
];

export default function Screen() {
  const [favorites, setFavorites] = React.useState<string[]>([]);
  const [isFilterVisible, setIsFilterVisible] = React.useState(false);

  const toggleFavorite = (id: string) => {
    setFavorites(prev => 
      prev.includes(id) ? prev.filter(fid => fid !== id) : [...prev, id]
    );
  };

  const handleFilterPress = () => {
    setIsFilterVisible(!isFilterVisible);
    // TODO: Implement filter modal or dropdown
    console.log('Filter pressed');
  };

  const renderItem = ({ item, section }: any) => {
    switch (section.type) {
      case 'hero':
        return <Hero />;
      case 'pickUpDropOff':
        return <PickUpDropOff />;
      case 'popular':
        return (
          <CarList
            title="Popular Cars"
            cars={item}
            layout="horizontal"
            favorites={favorites}
            onToggleFavorite={toggleFavorite}
            containerClassName="py-4 mx-4"
          />
        );
      case 'recommended':
        return (
          <CarList
            title="Recommended Cars"
            cars={item}
            layout="vertical"
            favorites={favorites}
            onToggleFavorite={toggleFavorite}
            containerClassName="py-4 mb-4 mx-4"
          />
        );
      default:
        return null;
    }
  };

  return (
    <SafeAreaView className="flex-1 bg-background">
      <Header 
        onPressMenu={() => console.log('Menu pressed')}
        onPressAvatar={() => console.log('Avatar pressed')}
        onFilterPress={handleFilterPress}
        showSearchBar={true}
      />
      <SectionList
        sections={SECTIONS}
        renderItem={renderItem}
        renderSectionHeader={() => null}
        stickySectionHeadersEnabled={false}
        showsVerticalScrollIndicator={false}
      />
    </SafeAreaView>
  );
}

