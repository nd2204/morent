import React from 'react';
import { View, Text, StyleSheet, TouchableOpacity } from 'react-native';
import { theme } from '../styles/theme';
import { Feather } from '@expo/vector-icons';
import { PaymentMethodType } from '../types';

interface ConfirmationProps {
  agreements: {
    termsConditions: boolean;
    subscribeNewsletter: boolean;
  };
  toggleAgreement: (key: 'termsConditions' | 'subscribeNewsletter') => void;
  onPay: () => void;
  isValid: boolean;
  paymentMethod: PaymentMethodType;
}

const Confirmation: React.FC<ConfirmationProps> = ({ 
  agreements, 
  toggleAgreement, 
  onPay, 
  isValid,
  paymentMethod 
}) => {
  // Determine the button colors based on payment method
  const getPayButtonStyle = () => {
    if (!isValid) return styles.payButtonDisabled;
    
    switch (paymentMethod) {
      case 'PayPal':
        return { backgroundColor: theme.colors.paypal };
      case 'Stripe':
        return { backgroundColor: theme.colors.stripe };
      case 'VNPAY':
        return { backgroundColor: theme.colors.vnpay };
      case 'MoMo':
        return { backgroundColor: theme.colors.momo };
      default:
        return { backgroundColor: theme.colors.primary };
    }
  };

  // Determine button text based on payment method
  const getPayButtonText = () => {
    switch (paymentMethod) {
      case 'PayPal':
        return 'Pay with PayPal';
      case 'Stripe':
        return 'Pay with Stripe';
      case 'VNPAY':
        return 'Pay with VNPAY';
      case 'MoMo':
        return 'Pay with MoMo';
      default:
        return 'Pay $80.00';
    }
  };

  return (
    <View style={styles.container}>
      <View style={styles.agreementRow}>
        <TouchableOpacity
          style={styles.checkbox}
          onPress={() => toggleAgreement('termsConditions')}
        >
          {agreements.termsConditions ? (
            <Feather name="check" size={18} color={theme.colors.primary} />
          ) : null}
        </TouchableOpacity>
        <Text style={styles.agreementText}>
          I agree to the Terms of Service and Privacy Policy
        </Text>
      </View>
      
      <View style={styles.agreementRow}>
        <TouchableOpacity
          style={styles.checkbox}
          onPress={() => toggleAgreement('subscribeNewsletter')}
        >
          {agreements.subscribeNewsletter ? (
            <Feather name="check" size={18} color={theme.colors.primary} />
          ) : null}
        </TouchableOpacity>
        <Text style={styles.agreementText}>
          Subscribe to newsletter for latest products and promotions
        </Text>
      </View>
      
      <TouchableOpacity 
        style={[
          styles.payButton,
          getPayButtonStyle(),
          !isValid && styles.payButtonDisabled,
        ]}
        onPress={onPay}
        disabled={!isValid}
      >
        <Text style={styles.payButtonText}>{getPayButtonText()}</Text>
      </TouchableOpacity>
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
  agreementRow: {
    flexDirection: 'row',
    alignItems: 'center',
    marginBottom: 16,
  },
  checkbox: {
    width: 22,
    height: 22,
    borderWidth: 1,
    borderColor: theme.colors.border,
    borderRadius: 4,
    marginRight: 10,
    justifyContent: 'center',
    alignItems: 'center',
  },
  agreementText: {
    flex: 1,
    fontSize: 14,
    color: theme.colors.text,
  },
  payButton: {
    paddingVertical: 14,
    borderRadius: 8,
    alignItems: 'center',
    justifyContent: 'center',
    marginTop: 8,
  },
  payButtonDisabled: {
    backgroundColor: theme.colors.disabled,
  },
  payButtonText: {
    color: theme.colors.white,
    fontSize: 16,
    fontWeight: 'bold',
  },
});

export default Confirmation;
