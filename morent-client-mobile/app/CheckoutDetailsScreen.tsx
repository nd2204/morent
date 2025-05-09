import React, { useState, useEffect } from 'react';
import { 
  View, 
  Text, 
  StyleSheet, 
  ScrollView, 
  ActivityIndicator, 
  TouchableOpacity,
  Share,
  Image
} from 'react-native';
import { useNavigation, useRoute, RouteProp } from '@react-navigation/native';
import { PaymentMethodType } from '../types';
import { getCheckoutById, paymentMethodIcons } from '../data/checkoutHistory';
import RatingDisplay from '../components/RatingDisplay';
import RatingForm from '../components/RatingForm';

// Format date
const formatDate = (dateString: string) => {
  const date = new Date(dateString);
  return date.toLocaleDateString('en-US', {
    year: 'numeric',
    month: 'short',
    day: 'numeric',
    hour: '2-digit',
    minute: '2-digit',
  });
};

type RouteParams = {
  CheckoutDetails: {
    checkoutId: number;
  };
};

// Using paymentMethodIcons from checkoutHistory.ts

const CheckoutDetailsScreen = () => {
  const [checkout, setCheckout] = useState<any | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [isEditingRating, setIsEditingRating] = useState(false);
  
  const navigation = useNavigation();
  const route = useRoute<RouteProp<RouteParams, 'CheckoutDetails'>>();
  const { checkoutId } = route.params;

  useEffect(() => {
    loadCheckoutDetails();
  }, [checkoutId]);

  const loadCheckoutDetails = () => {
    try {
      setLoading(true);
      setError(null);
      
      // Use the static data from our checkoutHistory
      setTimeout(() => {
        const data = getCheckoutById(checkoutId);
        if (data) {
          setCheckout(data);
        } else {
          setError('Checkout not found');
        }
        setLoading(false);
      }, 500); // Simulate network delay
    } catch (err: any) {
      setError(err.message || 'Failed to load checkout details');
      console.error(err);
      setLoading(false);
    }
  };

  const shareReceipt = async () => {
    if (!checkout) return;
    
    try {
      const result = await Share.share({
        message: `Receipt for Order #${checkout.id}\n` +
          `Amount: ${checkout.currency} ${parseFloat(checkout.amount).toFixed(2)}\n` +
          `Payment Method: ${checkout.payment_method}\n` +
          `Date: ${formatDate(checkout.created_at)}\n` +
          `Status: ${checkout.status.charAt(0).toUpperCase() + checkout.status.slice(1)}\n` +
          `Thank you for your purchase!`
      });
    } catch (error) {
      console.error('Error sharing receipt:', error);
    }
  };

  if (loading) {
    return (
      <View style={styles.centered}>
        <ActivityIndicator size="large" color="#0070ba" />
        <Text style={styles.loadingText}>Loading checkout details...</Text>
      </View>
    );
  }

  if (error) {
    return (
      <View style={styles.centered}>
        <Text style={styles.errorText}>{error}</Text>
        <TouchableOpacity style={styles.retryButton} onPress={loadCheckoutDetails}>
          <Text style={styles.retryButtonText}>Retry</Text>
        </TouchableOpacity>
      </View>
    );
  }

  if (!checkout) {
    return (
      <View style={styles.centered}>
        <Text style={styles.errorText}>Checkout not found</Text>
        <TouchableOpacity 
          style={styles.backButton} 
          onPress={() => navigation.goBack()}
        >
          <Text style={styles.backButtonText}>Back to History</Text>
        </TouchableOpacity>
      </View>
    );
  }

  // Determine payment method icon
  const paymentMethodIcon = checkout.payment_method in paymentMethodIcons 
    ? paymentMethodIcons[checkout.payment_method as PaymentMethodType] 
    : paymentMethodIcons.CreditCard;

  return (
    <ScrollView style={styles.container}>
      <View style={styles.header}>
        <Text style={styles.headerText}>Order #{checkout.id}</Text>
        <Text style={styles.dateText}>{formatDate(checkout.created_at)}</Text>
      </View>

      <View style={styles.card}>
        <View style={styles.statusContainer}>
          <Text style={[
            styles.statusText, 
            checkout.status === 'completed' ? styles.statusCompleted : styles.statusPending
          ]}>
            {checkout.status.charAt(0).toUpperCase() + checkout.status.slice(1)}
          </Text>
        </View>
        
        <View style={styles.amountContainer}>
          <Text style={styles.amountLabel}>Total Amount</Text>
          <Text style={styles.amountValue}>
            {checkout.currency} {parseFloat(checkout.amount).toFixed(2)}
          </Text>
        </View>
        
        <View style={styles.paymentMethodContainer}>
          <Text style={styles.sectionTitle}>Payment Method</Text>
          <View style={styles.paymentMethodContent}>
            <Image 
              source={{ uri: paymentMethodIcon }}
              style={styles.paymentIcon}
            />
            <Text style={styles.paymentMethodText}>{checkout.payment_method}</Text>
          </View>
        </View>
      </View>

      {checkout.items && checkout.items.length > 0 && (
        <View style={styles.card}>
          <Text style={styles.sectionTitle}>Items</Text>
          {checkout.items.map((item: any, index: number) => (
            <View key={index} style={styles.itemContainer}>
              <View style={styles.itemHeader}>
                <Text style={styles.itemTitle}>{item.title}</Text>
                <Text style={styles.itemPrice}>
                  {checkout.currency} {parseFloat(item.price).toFixed(2)}
                </Text>
              </View>
              {item.description && (
                <Text style={styles.itemDescription}>{item.description}</Text>
              )}
              {item.quantity > 1 && (
                <Text style={styles.itemQuantity}>Quantity: {item.quantity}</Text>
              )}
            </View>
          ))}
        </View>
      )}
      
      {/* Rating Section */}
      <View style={styles.card}>
        <Text style={styles.sectionTitle}>Product Rating</Text>
        
        {checkout.rating && !isEditingRating ? (
          <RatingDisplay 
            rating={checkout.rating} 
            onEdit={() => setIsEditingRating(true)}
          />
        ) : isEditingRating ? (
          <RatingForm
            checkoutId={checkout.id}
            existingRating={checkout.rating}
            onRatingSubmitted={() => {
              setIsEditingRating(false);
              loadCheckoutDetails();
            }}
          />
        ) : (
          <RatingForm
            checkoutId={checkout.id}
            onRatingSubmitted={loadCheckoutDetails}
          />
        )}
      </View>

      <View style={styles.actionsContainer}>
        <TouchableOpacity 
          style={styles.actionButton}
          onPress={shareReceipt}
        >
          <Text style={styles.actionButtonText}>Share Receipt</Text>
        </TouchableOpacity>
        
        <TouchableOpacity 
          style={[styles.actionButton, styles.secondaryButton]}
          onPress={() => navigation.goBack()}
        >
          <Text style={styles.secondaryButtonText}>Back to History</Text>
        </TouchableOpacity>
      </View>
    </ScrollView>
  );
};

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: '#F5F7FA',
  },
  header: {
    padding: 16,
    backgroundColor: '#FFFFFF',
    borderBottomWidth: 1,
    borderBottomColor: '#F0F0F0',
  },
  headerText: {
    fontSize: 22,
    fontWeight: 'bold',
    color: '#2C2E2F',
    marginBottom: 4,
  },
  dateText: {
    fontSize: 14,
    color: '#666',
  },
  card: {
    backgroundColor: '#FFFFFF',
    borderRadius: 10,
    padding: 16,
    margin: 16,
    shadowColor: '#000',
    shadowOffset: { width: 0, height: 2 },
    shadowOpacity: 0.1,
    shadowRadius: 4,
    elevation: 2,
  },
  statusContainer: {
    marginBottom: 16,
    alignItems: 'flex-start',
  },
  statusText: {
    fontSize: 14,
    fontWeight: '600',
    paddingHorizontal: 12,
    paddingVertical: 6,
    borderRadius: 16,
  },
  statusCompleted: {
    backgroundColor: '#E8F5E9',
    color: '#28A745',
  },
  statusPending: {
    backgroundColor: '#FFF8E1',
    color: '#FFC107',
  },
  amountContainer: {
    marginBottom: 24,
    borderBottomWidth: 1,
    borderBottomColor: '#F0F0F0',
    paddingBottom: 16,
  },
  amountLabel: {
    fontSize: 14,
    color: '#666',
    marginBottom: 4,
  },
  amountValue: {
    fontSize: 26,
    fontWeight: 'bold',
    color: '#2C2E2F',
  },
  sectionTitle: {
    fontSize: 16,
    fontWeight: '600',
    color: '#2C2E2F',
    marginBottom: 12,
  },
  paymentMethodContainer: {
    marginBottom: 8,
  },
  paymentMethodContent: {
    flexDirection: 'row',
    alignItems: 'center',
  },
  paymentIcon: {
    width: 32,
    height: 32,
    marginRight: 12,
    borderRadius: 4,
  },
  paymentMethodText: {
    fontSize: 16,
    color: '#2C2E2F',
  },
  itemContainer: {
    borderTopWidth: 1,
    borderTopColor: '#F0F0F0',
    paddingTop: 12,
    marginTop: 12,
  },
  itemHeader: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    marginBottom: 4,
  },
  itemTitle: {
    fontSize: 16,
    fontWeight: '500',
    color: '#2C2E2F',
    flex: 1,
  },
  itemPrice: {
    fontSize: 16,
    fontWeight: '600',
    color: '#2C2E2F',
  },
  itemDescription: {
    fontSize: 14,
    color: '#666',
    marginBottom: 4,
  },
  itemQuantity: {
    fontSize: 14,
    color: '#666',
    fontStyle: 'italic',
  },
  actionsContainer: {
    padding: 16,
    marginBottom: 20,
  },
  actionButton: {
    backgroundColor: '#0070ba',
    borderRadius: 8,
    padding: 16,
    alignItems: 'center',
    marginBottom: 12,
  },
  actionButtonText: {
    color: '#FFFFFF',
    fontSize: 16,
    fontWeight: '600',
  },
  secondaryButton: {
    backgroundColor: 'transparent',
    borderWidth: 1,
    borderColor: '#0070ba',
  },
  secondaryButtonText: {
    color: '#0070ba',
    fontSize: 16,
    fontWeight: '600',
  },
  centered: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
    padding: 20,
  },
  loadingText: {
    marginTop: 12,
    fontSize: 16,
    color: '#666',
  },
  errorText: {
    fontSize: 16,
    color: '#FF3B30',
    textAlign: 'center',
    marginBottom: 16,
  },
  retryButton: {
    backgroundColor: '#0070ba',
    paddingHorizontal: 20,
    paddingVertical: 10,
    borderRadius: 5,
  },
  retryButtonText: {
    color: '#FFFFFF',
    fontSize: 16,
    fontWeight: '600',
  },
  backButton: {
    backgroundColor: '#0070ba',
    paddingHorizontal: 20,
    paddingVertical: 10,
    borderRadius: 5,
    marginTop: 8,
  },
  backButtonText: {
    color: '#FFFFFF',
    fontSize: 16,
    fontWeight: '600',
  },
});

export default CheckoutDetailsScreen;