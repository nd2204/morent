import * as React from 'react';
import { View } from 'react-native';
import { Text } from './ui/text';
import { Card } from './ui/card';
import { Avatar, AvatarFallback, AvatarImage } from './ui/avatar';
import { Star } from 'lucide-react-native';
import { iconWithClassName } from '~/lib/icons/iconWithClassName';

// Register icons with NativeWind
iconWithClassName(Star);

interface ReviewProps {
  rating: number;
  comment: string;
  userName: string;
  userImage?: string;
  date: string;
  className?: string;
}

export function Review({ rating, comment, userName, userImage, date, className }: ReviewProps) {
  return (
    <Card className={className}>
      <View className="p-4">
        <View className="flex-row items-center mb-3">
          <Avatar className="h-10 w-10 mr-3" alt="User avatar">
            <AvatarImage source={{ uri: userImage }} />
            <AvatarFallback>
              <Text>{userName.charAt(0)}</Text>
            </AvatarFallback>
          </Avatar>
          <View className="flex-1">
            <Text className="font-semibold">{userName}</Text>
            <Text className="text-sm text-muted-foreground">{date}</Text>
          </View>
          <View className="flex-row items-center">
            <Star size={16} className="text-yellow-400 fill-yellow-400 mr-1" />
            <Text className="text-sm">{rating.toFixed(1)}</Text>
          </View>
        </View>
        <Text className="text-muted-foreground">{comment}</Text>
      </View>
    </Card>
  );
}

interface ReviewsListProps {
  reviews: Array<Omit<ReviewProps, 'className'>>;
  className?: string;
}

export function ReviewsList({ reviews, className }: ReviewsListProps) {
  const averageRating = (reviews.reduce((acc, review) => acc + review.rating, 0) / reviews.length).toFixed(1);
  
  return (
    <View className={className}>
      <View className="flex-row justify-between items-center mb-4">
        <Text className="text-lg font-semibold">Reviews ({reviews.length})</Text>
        <View className="flex-row items-center">
          <Star size={18} className="text-yellow-400 fill-yellow-400 mr-1" />
          <Text className="font-semibold">{averageRating}</Text>
        </View>
      </View>
      <View className="space-y-4 gap-4">
        {reviews.map((review, index) => (
          <Review key={index} {...review} />
        ))}
      </View>
    </View>
  );
}