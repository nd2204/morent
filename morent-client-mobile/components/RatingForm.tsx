import React, { useState } from 'react';
import { 
  View, 
  Text, 
  StyleSheet, 
  TextInput, 
  TouchableOpacity,
  ActivityIndicator,
  Alert,
} from 'react-native';
import StarRating from './StarRating';
import { addRating } from '../data/checkoutHistory';

interface RatingFormProps {
  checkoutId: number;
  onRatingSubmitted: () => void;
  existingRating?: {
    score: number;
    comment?: string;
  };
}

const RatingForm: React.FC<RatingFormProps> = ({ 
  checkoutId, 
  onRatingSubmitted,
  existingRating 
}) => {
  const [rating, setRating] = useState(existingRating?.score || 0);
  const [comment, setComment] = useState(existingRating?.comment || '');
  const [submitting, setSubmitting] = useState(false);
  const [error, setError] = useState<string | null>(null);
  
  const isEditing = !!existingRating;
  const buttonText = isEditing ? 'Update Rating' : 'Submit Rating';
  
  const handleSubmit = () => {
    if (rating === 0) {
      setError('Please select a rating before submitting');
      return;
    }
    
    setSubmitting(true);
    setError(null);
    
    // Simulate network latency
    setTimeout(() => {
      try {
        const result = addRating(checkoutId, rating, comment);
        
        if (!result) {
          throw new Error('Failed to save rating');
        }
        
        setSubmitting(false);
        Alert.alert(
          isEditing ? 'Rating Updated' : 'Thank You!',
          isEditing 
            ? 'Your rating has been updated successfully.'
            : 'Your feedback is important to us!',
          [{ text: 'OK', onPress: onRatingSubmitted }]
        );
      } catch (err: any) {
        setSubmitting(false);
        setError(err.message || 'Failed to submit rating');
      }
    }, 1000);
  };
  
  return (
    <View style={styles.container}>
      <Text style={styles.title}>
        {isEditing ? 'Edit Your Rating' : 'Rate Your Experience'}
      </Text>
      
      <View style={styles.ratingContainer}>
        <StarRating
          rating={rating}
          onRatingChange={setRating}
          size={32}
        />
        <Text style={styles.ratingText}>
          {rating ? `${rating} out of 5 stars` : 'Tap to rate'}
        </Text>
      </View>
      
      <Text style={styles.label}>Add a comment (optional)</Text>
      <TextInput
        style={styles.input}
        value={comment}
        onChangeText={setComment}
        placeholder="Share your experience..."
        multiline
        numberOfLines={4}
        maxLength={250}
      />
      
      {error && <Text style={styles.errorText}>{error}</Text>}
      
      <TouchableOpacity 
        style={styles.button}
        onPress={handleSubmit}
        disabled={submitting}
      >
        {submitting ? (
          <ActivityIndicator color="#FFFFFF" size="small" />
        ) : (
          <Text style={styles.buttonText}>{buttonText}</Text>
        )}
      </TouchableOpacity>
    </View>
  );
};

const styles = StyleSheet.create({
  container: {
    padding: 16,
    backgroundColor: '#FFFFFF',
    borderRadius: 8,
    marginVertical: 16,
  },
  title: {
    fontSize: 18,
    fontWeight: 'bold',
    marginBottom: 16,
    color: '#2C2E2F',
  },
  ratingContainer: {
    alignItems: 'center',
    marginBottom: 24,
  },
  ratingText: {
    marginTop: 8,
    color: '#666',
    fontSize: 14,
  },
  label: {
    fontSize: 14,
    fontWeight: '500',
    marginBottom: 8,
    color: '#2C2E2F',
  },
  input: {
    borderWidth: 1,
    borderColor: '#E0E0E0',
    borderRadius: 4,
    padding: 12,
    fontSize: 16,
    minHeight: 100,
    textAlignVertical: 'top',
    marginBottom: 16,
  },
  button: {
    backgroundColor: '#0070ba',
    paddingVertical: 12,
    borderRadius: 4,
    alignItems: 'center',
    justifyContent: 'center',
  },
  buttonText: {
    color: '#FFFFFF',
    fontSize: 16,
    fontWeight: '600',
  },
  errorText: {
    color: '#D32F2F',
    marginBottom: 12,
  },
});

export default RatingForm;