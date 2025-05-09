import React from 'react';
import { View, Text, StyleSheet, TouchableOpacity } from 'react-native';
import StarRating from './StarRating';
import { Rating } from '../data/checkoutHistory';

interface RatingDisplayProps {
  rating: Rating;
  onEdit?: () => void;
  showEdit?: boolean;
}

const RatingDisplay: React.FC<RatingDisplayProps> = ({ 
  rating, 
  onEdit, 
  showEdit = true 
}) => {
  const formattedDate = (dateString: string) => {
    const date = new Date(dateString);
    return date.toLocaleDateString('en-US', {
      year: 'numeric',
      month: 'short',
      day: 'numeric',
    });
  };

  return (
    <View style={styles.container}>
      <View style={styles.headerRow}>
        <View>
          <Text style={styles.title}>Your Rating</Text>
          <View style={styles.ratingRow}>
            <StarRating rating={rating.score} readonly={true} size={20} />
            <Text style={styles.ratingText}>{rating.score}/5</Text>
          </View>
        </View>
        
        {showEdit && onEdit && (
          <TouchableOpacity
            style={styles.editButton}
            onPress={onEdit}
          >
            <Text style={styles.editButtonText}>Edit</Text>
          </TouchableOpacity>
        )}
      </View>
      
      {rating.comment && (
        <View style={styles.commentContainer}>
          <Text style={styles.comment}>{rating.comment}</Text>
        </View>
      )}
      
      <Text style={styles.date}>
        Rated on {formattedDate(rating.created_at)}
      </Text>
    </View>
  );
};

const styles = StyleSheet.create({
  container: {
    padding: 16,
    backgroundColor: '#FFFFFF',
    borderRadius: 8,
    marginVertical: 16,
    borderWidth: 1,
    borderColor: '#F0F0F0',
  },
  headerRow: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'flex-start',
    marginBottom: 12,
  },
  title: {
    fontSize: 16,
    fontWeight: '600',
    color: '#2C2E2F',
    marginBottom: 4,
  },
  ratingRow: {
    flexDirection: 'row',
    alignItems: 'center',
  },
  ratingText: {
    marginLeft: 8,
    fontSize: 14,
    color: '#666',
  },
  commentContainer: {
    paddingVertical: 12,
    borderTopWidth: 1,
    borderTopColor: '#F0F0F0',
    marginBottom: 8,
  },
  comment: {
    fontSize: 14,
    color: '#2C2E2F',
    lineHeight: 20,
  },
  date: {
    fontSize: 12,
    color: '#999',
    textAlign: 'right',
  },
  editButton: {
    paddingHorizontal: 12,
    paddingVertical: 6,
    backgroundColor: '#F0F0F0',
    borderRadius: 4,
  },
  editButtonText: {
    fontSize: 12,
    color: '#2C2E2F',
    fontWeight: '500',
  },
});

export default RatingDisplay;