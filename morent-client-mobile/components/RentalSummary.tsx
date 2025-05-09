import React from 'react';
import { View, Text, StyleSheet } from 'react-native';
import { theme } from '../styles/theme';

interface RentalDetails {
  title: string;
  description: string;
  price: number;
  subtotal: number;
  total: number;
}

interface RentalSummaryProps {
  details: RentalDetails;
}

const RentalSummary: React.FC<RentalSummaryProps> = ({ details }) => {
  return (
    <View style={styles.container}>
      <Text style={styles.header}>Rental Summary</Text>
      
      <View style={styles.productCard}>
        <View style={styles.productInfo}>
          <Text style={styles.productTitle}>{details.title}</Text>
          <Text style={styles.productDescription}>{details.description}</Text>
        </View>
        <Text style={styles.productPrice}>${details.price.toFixed(2)}</Text>
      </View>
      
      <View style={styles.divider} />
      
      <View style={styles.row}>
        <Text style={styles.label}>Subtotal/Price</Text>
        <Text style={styles.value}>${details.subtotal.toFixed(2)}</Text>
      </View>
      
      <View style={styles.row}>
        <Text style={styles.labelTotal}>Total</Text>
        <Text style={styles.valueTotal}>${details.total.toFixed(2)}</Text>
      </View>
    </View>
  );
};

const styles = StyleSheet.create({
  container: {
    backgroundColor: theme.colors.white,
    borderRadius: 8,
    padding: 16,
    marginVertical: 8,
    shadowColor: theme.colors.shadow,
    shadowOffset: { width: 0, height: 2 },
    shadowOpacity: 0.1,
    shadowRadius: 4,
    elevation: 2,
  },
  header: {
    fontSize: 18,
    fontWeight: 'bold',
    color: theme.colors.text,
    marginBottom: 16,
  },
  productCard: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
    marginBottom: 16,
  },
  productInfo: {
    flex: 1,
  },
  productTitle: {
    fontSize: 16,
    fontWeight: '600',
    color: theme.colors.text,
  },
  productDescription: {
    fontSize: 14,
    color: theme.colors.textLight,
    marginTop: 4,
  },
  productPrice: {
    fontSize: 16,
    fontWeight: '600',
    color: theme.colors.primary,
  },
  divider: {
    height: 1,
    backgroundColor: theme.colors.border,
    marginVertical: 16,
  },
  row: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    marginBottom: 8,
  },
  label: {
    fontSize: 14,
    color: theme.colors.textLight,
  },
  value: {
    fontSize: 14,
    color: theme.colors.text,
    fontWeight: '500',
  },
  labelTotal: {
    fontSize: 16,
    fontWeight: 'bold',
    color: theme.colors.text,
  },
  valueTotal: {
    fontSize: 16,
    fontWeight: 'bold',
    color: theme.colors.primary,
  },
});

export default RentalSummary;
