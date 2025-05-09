import React, { useState } from 'react';
import { ScrollView, StyleSheet, View, KeyboardAvoidingView, Platform, TouchableOpacity, Text } from 'react-native';
import { useNavigation } from '@react-navigation/native';
import { theme } from '../styles/theme';
import RentalSummary from '../components/RentalSummary';
import BillingInfo from '../components/BillingInfo';
import RecipientInfo from '../components/RecipientInfo';
import PaymentMethod from '../components/PaymentMethod';
import Confirmation from '../components/Confirmation';
import Header from '../components/Header';
import Footer from '../components/Footer';
import { PaymentMethodType } from '../types';
import { useRouter } from 'expo-router';

const CheckoutScreen: React.FC = () => {
  const router = useRouter();
  const navigation = useNavigation();
  const [billingInfo, setBillingInfo] = useState({
    name: '',
    address: '',
    phoneNumber: '',
    townCity: '',
  });

  const [recipientInfo, setRecipientInfo] = useState({
    name: '',
    address: '',
    phoneNumber: '',
    townCity: '',
  });

  const [selectedPayment, setSelectedPayment] = useState<PaymentMethodType>('CreditCard');
  
  const [agreements, setAgreements] = useState({
    termsConditions: false,
    subscribeNewsletter: false,
  });

  const updateBillingInfo = (field: string, value: string) => {
    setBillingInfo(prev => ({ ...prev, [field]: value }));
  };

  const updateRecipientInfo = (field: string, value: string) => {
    setRecipientInfo(prev => ({ ...prev, [field]: value }));
  };

  const toggleAgreement = (key: 'termsConditions' | 'subscribeNewsletter') => {
    setAgreements(prev => ({ ...prev, [key]: !prev[key] }));
  };

  const handlePayment = async () => {
    // Process payment based on selectedPayment
    console.log('Processing payment with method:', selectedPayment);
    console.log('Billing info:', billingInfo);
    console.log('Recipient info:', recipientInfo);
    console.log('Agreements:', agreements);
    
    try {
      // In a real app, this would be a unique user ID stored in app state/context
      const userId = 'user123';
      
      // Example of how to record a checkout in history
      // This would typically happen after successful payment
      const checkoutData = {
        user_id: userId,
        amount: rentalDetails.total,
        currency: 'USD',
        payment_method: selectedPayment,
        items: [
          {
            title: rentalDetails.title,
            description: rentalDetails.description,
            price: rentalDetails.price,
            quantity: 1
          }
        ],
        metadata: {
          billing_info: billingInfo,
          recipient_info: recipientInfo,
          source: 'mobile-app'
        }
      };
      
      // For demo purposes, record the checkout as successful
      // In a real app, this would happen after payment confirmation
      const response = await fetch('/api/record-checkout', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(checkoutData),
      });
      
      if (!response.ok) {
        throw new Error(`API request failed with status ${response.status}`);
      }
      
      await response.json();
      
      // Show success message
      alert('Payment successful! Order has been recorded.');
      
      // Navigate to history screen
      // @ts-ignore - navigation typing issue
      navigation.navigate('CheckoutHistory');
    } catch (error) {
      console.error('Error processing payment:', error);
      alert('Payment failed. Please try again.');
    }
  };

  const rentalDetails = {
    title: 'Nikon GT-R',
    description: 'All basic features',
    price: 80.00,
    subtotal: 80.00,
    total: 80.00,
  };

  return (
    <KeyboardAvoidingView
      style={styles.container}
      behavior={Platform.OS === 'ios' ? 'padding' : 'height'}
      keyboardVerticalOffset={Platform.OS === 'ios' ? 64 : 0}
    >
      <Header />
      <ScrollView style={styles.scrollView} showsVerticalScrollIndicator={false}>
        <View style={styles.content}>
          <RentalSummary details={rentalDetails} />
          
          <BillingInfo 
            info={billingInfo} 
            updateInfo={updateBillingInfo} 
          />
          
          <RecipientInfo 
            info={recipientInfo} 
            updateInfo={updateRecipientInfo} 
          />
          
          <PaymentMethod
            selectedMethod={selectedPayment}
            onSelectMethod={setSelectedPayment}
          />
          
          <Confirmation
            agreements={agreements}
            toggleAgreement={toggleAgreement}
            onPay={handlePayment}
            isValid={agreements.termsConditions}
            paymentMethod={selectedPayment}
          />
        </View>
      </ScrollView>
      <View style={styles.historyButtonContainer}>
        <TouchableOpacity 
          style={styles.historyButton}
          onPress={() => router.push({
            pathname: '/CheckoutHistoryScreen'
          })}
        >
          <Text style={styles.historyButtonText}>View Order History</Text>
        </TouchableOpacity>
      </View>
      <Footer />
    </KeyboardAvoidingView>
  );
};

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: theme.colors.background,
  },
  scrollView: {
    flex: 1,
  },
  content: {
    padding: 16,
  },
  historyButtonContainer: {
    padding: 16,
    backgroundColor: theme.colors.white,
    borderTopWidth: 1,
    borderTopColor: theme.colors.border,
  },
  historyButton: {
    backgroundColor: theme.colors.secondary || '#f0f0f0',
    paddingVertical: 12,
    paddingHorizontal: 20,
    borderRadius: 8,
    alignItems: 'center',
  },
  historyButtonText: {
    color: theme.colors.white || '#ffffff',
    fontSize: 16,
    fontWeight: '600',
  },
});

export default CheckoutScreen;
