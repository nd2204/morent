import React from 'react';
import { View, Text, StyleSheet, TouchableOpacity, Image } from 'react-native';
import { theme } from '../styles/theme';
import { PaymentMethodType } from '../types';
import CreditCardForm from './CreditCardForm';
import PayPalForm from './PayPalForm';
import StripeForm from './StripeForm';
import VNPayForm from './VNPayForm';
import MoMoForm from './MoMoForm';
import { 
  creditCardIcon, 
  paypalIcon, 
  stripeIcon, 
  vnpayIcon, 
  momoIcon 
} from '../assets/payment-icons';

interface PaymentMethodProps {
  selectedMethod: PaymentMethodType;
  onSelectMethod: (method: PaymentMethodType) => void;
}

const PaymentMethod: React.FC<PaymentMethodProps> = ({ 
  selectedMethod, 
  onSelectMethod 
}) => {
  const renderPaymentForm = () => {
    switch (selectedMethod) {
      case 'CreditCard':
        return <CreditCardForm />;
      case 'PayPal':
        return <PayPalForm />;
      case 'Stripe':
        return <StripeForm />;
      case 'VNPAY':
        return <VNPayForm />;
      case 'MoMo':
        return <MoMoForm />;
      default:
        return null;
    }
  };

  const PaymentOption = ({ 
    method, 
    icon, 
    label 
  }: { 
    method: PaymentMethodType; 
    icon: string; 
    label: string 
  }) => (
    <TouchableOpacity
      style={[
        styles.paymentOption,
        selectedMethod === method && styles.selectedPaymentOption
      ]}
      onPress={() => onSelectMethod(method)}
    >
      <Image source={{ uri: icon }} style={styles.paymentIcon} />
      <Text style={styles.paymentLabel}>{label}</Text>
    </TouchableOpacity>
  );

  return (
    <View style={styles.container}>
      <Text style={styles.header}>Payment Method</Text>
      
      <View style={styles.paymentOptions}>
        <PaymentOption method="CreditCard" icon={creditCardIcon} label="Credit Card" />
        <PaymentOption method="PayPal" icon={paypalIcon} label="PayPal" />
        <PaymentOption method="Stripe" icon={stripeIcon} label="Stripe" />
        <PaymentOption method="VNPAY" icon={vnpayIcon} label="VNPAY" />
        <PaymentOption method="MoMo" icon={momoIcon} label="MoMo" />
      </View>
      
      <View style={styles.formContainer}>
        {renderPaymentForm()}
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
  paymentOptions: {
    flexDirection: 'row',
    flexWrap: 'wrap',
    justifyContent: 'space-between',
    marginBottom: 16,
  },
  paymentOption: {
    width: '30%',
    alignItems: 'center',
    padding: 10,
    borderRadius: 8,
    borderWidth: 1,
    borderColor: theme.colors.border,
    marginBottom: 10,
  },
  selectedPaymentOption: {
    borderColor: theme.colors.primary,
    backgroundColor: theme.colors.primaryLight,
  },
  paymentIcon: {
    width: 40,
    height: 40,
    resizeMode: 'contain',
    marginBottom: 5,
  },
  paymentLabel: {
    fontSize: 12,
    color: theme.colors.text,
    textAlign: 'center',
  },
  formContainer: {
    marginTop: 16,
  },
});

export default PaymentMethod;
