import * as React from 'react';
import { Image, ImageSourcePropType, Pressable, View } from 'react-native';
import { Fuel, GaugeCircle, Users } from 'lucide-react-native';
import { Text } from '~/components/ui/text';
import { Button } from '~/components/ui/button';
import { Card } from '~/components/ui/card';
import { cn } from '~/lib/utils';
import { iconWithClassName } from '~/lib/icons/iconWithClassName';
import { useRouter } from 'expo-router';
import { FavoriteButton } from './FavoriteButton';

// Register icons with NativeWind
[Fuel, GaugeCircle, Users].forEach(iconWithClassName);

export interface CarCardProps {
  id: string;
  carName: string;
  carType: string;
  fuelCapacity: string | number;
  transmission: string;
  seats: string | number;
  pricePerDay: number;
  imageUrl: string | ImageSourcePropType;
  isFavorite: boolean;
  layout?: 'vertical' | 'horizontal';
  onPressRent?: () => void;
  onToggleFavorite?: () => void;
}

export function CarCard({
  id,
  carName,
  carType,
  fuelCapacity,
  transmission,
  seats,
  pricePerDay,
  imageUrl,
  isFavorite,
  layout = 'vertical',
  onPressRent,
  onToggleFavorite,
}: CarCardProps) {
  const router = useRouter();
  const isHorizontal = layout === 'horizontal';

  const handlePressRent = () => {
    router.push(`/${id}`);
  };

  const renderTitle = () => {
    return (
      <View className="flex-row items-start justify-between">
        <View>
          <Text className="text-xl font-semibold">{carName}</Text>
          <Text className="text-md text-muted-foreground mt-1">{carType}</Text>
        </View>
        <FavoriteButton 
          isFavorite={isFavorite} 
          onToggleFavorite={onToggleFavorite || (() => {})} 
        />
      </View>
    )
  }

  const renderImage = () => {
    return (
      <View className={cn(
        isHorizontal ? 'w-[140px] h-full flex-1 px-4' : 'h-[160px] w-full'
      )}>
        <Image
          source={typeof imageUrl === 'string' ? { uri: imageUrl } : imageUrl}
          className="w-full h-full"
          resizeMode="contain"
          accessibilityLabel={`Image of ${carName}`}
        />
      </View>
    );
  }

  const renderFeature = () => {
    return (
      <View className={cn(
        isHorizontal ? 'flex-col justify-center gap-2' : 'flex-row justify-between gap-2',
        isHorizontal ? 'ml-3' : 'mb-3'
      )}>
        <View className="flex-row items-center gap-2">
          <Fuel size={20} className="text-muted-foreground" />
          <Text className="text-md text-muted-foreground">{fuelCapacity}</Text>
        </View>
        <View className="flex-row items-center gap-2">
          <GaugeCircle size={20} className="text-muted-foreground" />
          <Text className="text-md text-muted-foreground">{transmission}</Text>
        </View>
        <View className="flex-row items-center gap-2">
          <Users size={20} className="text-muted-foreground" />
          <Text className="text-md text-muted-foreground">{seats}</Text>
        </View>
      </View>
    )
  }

  return (
    <Card className={cn(
      'overflow-hidden bg-card flex-column flex-1 p-5 dark:bg-[#111111]',
      isHorizontal
        ? 'min-h-[180px]'
        : 'min-w-[280px]'
    )}>
      <Pressable onPress={() => router.push(`/${id}`)}>
        {renderTitle()}

        {/*Render image and feature on the same line*/}
        {isHorizontal &&
          <View className='flex-row justify-between flex-1 mt-8 mb-6'>
            {renderImage()}
            {renderFeature()}
          </View>
        }
        {/*Render image and feature on separated line*/}
        {!isHorizontal && renderImage()}
        {!isHorizontal && renderFeature()}

        <View className={cn('flex-row items-center justify-between mt-5')}>
          <View className="flex-row items-baseline">
            <Text className="text-2xl font-bold" numberOfLines={1}>
              ${pricePerDay}/
            </Text>
            <Text className="text-md text-muted-foreground" >day</Text>
          </View>
          <Button className="px-4" size="default" onPress={handlePressRent}>
            <Text className="text-primary-foreground font-semibold text-sm">Rent Now</Text>
          </Button>
        </View>

      </Pressable>
    </Card>
  );
}