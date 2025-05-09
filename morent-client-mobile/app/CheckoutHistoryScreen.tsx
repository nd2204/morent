import React, { useState, useEffect } from 'react';
import { View, Text, StyleSheet, FlatList, TouchableOpacity, ActivityIndicator } from 'react-native';
import { useNavigation } from '@react-navigation/native';
import { checkoutHistory, Checkout } from '../data/checkoutHistory';

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

// Payment method colors
const methodColors = {
  Stripe: '#6772e5',
  PayPal: '#0070ba',
  CreditCard: '#2C2E2F',
  VNPAY: '#004A9F',
  MoMo: '#D82F8A',
};

const CheckoutHistoryScreen = () => {
  const [history, setHistory] = useState<any[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const navigation = useNavigation();

  useEffect(() => {
    loadCheckoutHistory();
  }, []);

  const loadCheckoutHistory = () => {
    try {
      setLoading(true);
      setError(null);

      // Use static data instead of API call
      setTimeout(() => {
        setHistory(checkoutHistory);
        setLoading(false);
      }, 500); // Simulate network delay
    } catch (err: any) {
      setError(err.message || 'Failed to load checkout history');
      console.error(err);
      setLoading(false);
    }
  };

  const navigateToDetails = (checkoutId: number) => {
    // @ts-ignore - navigation typing issue
    navigation.navigate('CheckoutDetailsScreen', { checkoutId });
  };

  if (loading) {
    return (
      <View style={styles.centered}>
        <ActivityIndicator size="large" color="#0070ba" />
        <Text style={styles.loadingText}>Loading checkout history...</Text>
      </View>
    );
  }

  if (error) {
    return (
      <View style={styles.centered}>
        <Text style={styles.errorText}>{error}</Text>
        <TouchableOpacity style={styles.retryButton} onPress={loadCheckoutHistory}>
          <Text style={styles.retryButtonText}>Retry</Text>
        </TouchableOpacity>
      </View>
    );
  }

  if (history.length === 0) {
    return (
      <View style={styles.container}>
        <Text style={styles.headerText}>Checkout History</Text>
        <View style={styles.centered}>
          <Text style={styles.emptyText}>No checkout history found</Text>
          <Text style={styles.emptySubText}>Your completed purchases will appear here</Text>
        </View>
      </View>
    );
  }

  return (
    <View style={styles.container}>
      <Text style={styles.headerText}>Checkout History</Text>
      <FlatList
        data={history}
        keyExtractor={(item) => item.id.toString()}
        renderItem={({ item }) => (
          <TouchableOpacity 
            style={styles.historyItem}
            onPress={() => navigateToDetails(item.id)}
          >
            <View style={styles.historyItemHeader}>
              <Text style={styles.historyItemId}>Order #{item.id}</Text>
              <View style={[
                styles.methodBadge, 
                { backgroundColor: methodColors[item.payment_method as keyof typeof methodColors] || '#666' }
              ]}>
                <Text style={styles.methodText}>{item.payment_method}</Text>
              </View>
            </View>
            
            <View style={styles.historyItemDetails}>
              <Text style={styles.historyItemAmount}>
                {item.currency} {parseFloat(item.amount).toFixed(2)}
              </Text>
              <Text style={styles.historyItemDate}>{formatDate(item.created_at)}</Text>
            </View>
            
            <View style={styles.historyItemFooter}>
              <Text style={[
                styles.statusText, 
                item.status === 'completed' ? styles.statusCompleted : styles.statusPending
              ]}>
                {item.status.charAt(0).toUpperCase() + item.status.slice(1)}
              </Text>
              <Text style={styles.viewDetailsText}>View Details →</Text>
            </View>
          </TouchableOpacity>
        )}
        contentContainerStyle={styles.listContent}
      />
    </View>
  );
};

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: '#F5F7FA',
    padding: 16,
  },
  headerText: {
    fontSize: 24,
    fontWeight: 'bold',
    marginVertical: 16,
    color: '#2C2E2F',
  },
  listContent: {
    paddingBottom: 20,
  },
  historyItem: {
    backgroundColor: '#FFFFFF',
    borderRadius: 10,
    padding: 16,
    marginBottom: 12,
    shadowColor: '#000',
    shadowOffset: { width: 0, height: 2 },
    shadowOpacity: 0.1,
    shadowRadius: 4,
    elevation: 2,
  },
  historyItemHeader: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
    marginBottom: 12,
  },
  historyItemId: {
    fontSize: 16,
    fontWeight: '600',
    color: '#2C2E2F',
  },
  methodBadge: {
    paddingHorizontal: 8,
    paddingVertical: 4,
    borderRadius: 4,
  },
  methodText: {
    color: '#FFFFFF',
    fontSize: 12,
    fontWeight: '500',
  },
  historyItemDetails: {
    marginBottom: 12,
  },
  historyItemAmount: {
    fontSize: 20,
    fontWeight: 'bold',
    color: '#2C2E2F',
    marginBottom: 4,
  },
  historyItemDate: {
    fontSize: 14,
    color: '#666',
  },
  historyItemFooter: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
    borderTopWidth: 1,
    borderTopColor: '#F0F0F0',
    paddingTop: 12,
  },
  statusText: {
    fontSize: 14,
    fontWeight: '500',
  },
  statusCompleted: {
    color: '#28A745',
  },
  statusPending: {
    color: '#FFC107',
  },
  viewDetailsText: {
    fontSize: 14,
    color: '#0070ba',
    fontWeight: '500',
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
  emptyText: {
    fontSize: 18,
    fontWeight: '600',
    color: '#2C2E2F',
    marginBottom: 8,
  },
  emptySubText: {
    fontSize: 14,
    color: '#666',
    textAlign: 'center',
  },
});

export default CheckoutHistoryScreen;