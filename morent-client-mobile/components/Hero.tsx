import * as React from 'react';
import { Image, View } from 'react-native';
import { Text } from './ui/text';
import { Button } from './ui/button';
import { Card } from './ui/card';
import { useRouter } from 'expo-router';

export function Hero() {
  const router = useRouter();

  return (
    <Card className='mx-5 mt-8'>
      <View className="bg-background px-4 py-6">
        <View className="gap-y-4">
          <Text className="text-3xl font-bold text-foreground leading-tight">
            The Best Platform for Car Rental
          </Text>
          <Text className="text-base text-muted-foreground leading-relaxed">
            Ease of doing a car rental safely and reliably. Of course at a low price.
          </Text>
          <Button 
            className="mt-4 w-40" 
            onPress={() => router.push('/category')}
          >
            <Text className="text-primary-foreground font-semibold">Rental Car</Text>
          </Button>
        </View>
        <View className="mt-6">
          <Image
            source={require('~/uploads/koenigsegg.png')}
            className="w-full h-48"
            resizeMode="contain"
            accessibilityLabel="Featured car"
          />
        </View>
      </View>
    </Card>
  );
}