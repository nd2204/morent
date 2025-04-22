import * as React from 'react';
import { FlatList, View } from 'react-native';
import { Text } from '~/components/ui/text';
import { Link } from 'expo-router';
import { CarCard, CarCardProps } from './CarCard';
import { cn } from '~/lib/utils';

interface CarListProps {
  title?: string;
  cars: Omit<CarCardProps, 'isFavorite' | 'onToggleFavorite'>[];
  layout?: 'vertical' | 'horizontal';
  favorites: string[];
  onToggleFavorite: (id: string) => void;
  showViewAll?: boolean;
  viewAllLink?: `/${string}`;
  containerClassName?: string;
  listHeaderComponent?: any
  listFooterComponent?: any
}

export function CarList({
  title,
  cars,
  layout = 'horizontal',
  favorites,
  onToggleFavorite,
  showViewAll = true,
  viewAllLink = "/",
  containerClassName,
  listHeaderComponent = () => <View></View>,
  listFooterComponent = () => <View></View>
}: CarListProps) {
  const isHorizontal = layout === 'horizontal';

  const renderHeader = () => {
    if (!title) return null;
    
    return (
      <View className={"flex-row justify-between mb-4"}>
        <Text className="text-lg font-semibold text-primary">{title}</Text>
        {showViewAll && (
          <Link href={viewAllLink} className="text-blue-500">View All</Link>
        )}
      </View>
    );
  };

  return (
    <View className={containerClassName}>
      {renderHeader()}
      <FlatList
        horizontal={isHorizontal}
        showsHorizontalScrollIndicator={false}
        showsVerticalScrollIndicator={false}
        data={cars}
        keyExtractor={item => item.id}
        contentContainerClassName={cn(
          isHorizontal ? "gap-x-4" : "gap-y-4 mb-20 pb-20",
          isHorizontal ? "px-4" : "px-0"
        )}
        renderItem={({ item }) => (
          <View className={isHorizontal ? "" : "py-2"}>
            <CarCard
              {...item}
              layout={isHorizontal ? "vertical" : "horizontal"}
              isFavorite={favorites.includes(item.id)}
              onToggleFavorite={() => onToggleFavorite(item.id)}
            />
          </View>
        )}
        ListHeaderComponent={listHeaderComponent}
        ListFooterComponent={listFooterComponent}
      />
    </View>
  );
}