import React from 'react';
import { View, StyleSheet, TouchableOpacity, Text } from 'react-native';

interface StarRatingProps {
  maxStars?: number;
  rating: number;
  onRatingChange?: (rating: number) => void;
  size?: number;
  readonly?: boolean;
}

const StarRating: React.FC<StarRatingProps> = ({
  maxStars = 5,
  rating,
  onRatingChange,
  size = 24,
  readonly = false,
}) => {
  const renderStar = (position: number) => {
    const filled = position <= rating;
    const starStyle = {
      fontSize: size,
      color: filled ? '#FFD700' : '#D3D3D3',
      marginRight: 4,
    };

    if (readonly) {
      return (
        <Text key={position} style={starStyle}>
          {filled ? '★' : '☆'}
        </Text>
      );
    }

    return (
      <TouchableOpacity
        key={position}
        onPress={() => onRatingChange && onRatingChange(position)}
        activeOpacity={0.7}
      >
        <Text style={starStyle}>
          {filled ? '★' : '☆'}
        </Text>
      </TouchableOpacity>
    );
  };

  return (
    <View style={styles.container}>
      {[...Array(maxStars)].map((_, index) => renderStar(index + 1))}
    </View>
  );
};

const styles = StyleSheet.create({
  container: {
    flexDirection: 'row',
    alignItems: 'center',
  },
});

export default StarRating;