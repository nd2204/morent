import React from 'react';
import { View, Text, StyleSheet, Image, Platform } from 'react-native';
import { theme } from '../styles/theme';
import { paypalIcon } from '../assets/payment-icons';
import PayPalButton from './PayPalButton';

const PayPalForm: React.FC = () => {
  // Only render the PayPalButton component on web, since PayPal SDK is web-only
  const isWeb = Platform.OS === 'web';
  
  return (
    <View style={styles.container}>
      <View style={styles.infoBox}>
        <Text style={styles.infoText}>
          You will be redirected to PayPal to complete your payment securely.
        </Text>
      </View>
      
      <Image source={{ uri: paypalIcon }} style={styles.paypalLogo} />
      
      {isWeb ? (
        <View style={styles.paypalButtonContainer}>
          <PayPalButton 
            amount="80.00" 
            currency="USD" 
            intent="CAPTURE" 
          />
        </View>
      ) : (
        <View style={styles.mobileMessage}>
          <Text style={styles.mobileText}>
            PayPal checkout is only available in the web version.
            Please open this app in a web browser to proceed with PayPal payment.
          </Text>
        </View>
      )}
    </View>
  );
};

const styles = StyleSheet.create({
  container: {
    alignItems: 'center',
    padding: 16,
  },
  infoBox: {
    backgroundColor: theme.colors.infoBackground,
    padding: 12,
    borderRadius: 8,
    marginBottom: 20,
  },
  infoText: {
    color: theme.colors.infoText,
    fontSize: 14,
    textAlign: 'center',
  },
  paypalLogo: {
    width: 120,
    height: 60,
    resizeMode: 'contain',
    marginBottom: 20,
  },
  paypalButtonContainer: {
    width: '100%',
    height: 50,
    marginVertical: 10,
    // Additional styles for web
  },
  mobileMessage: {
    backgroundColor: theme.colors.warning,
    padding: 12,
    borderRadius: 8,
    marginTop: 10,
  },
  mobileText: {
    color: theme.colors.text,
    fontSize: 14,
    textAlign: 'center',
  }
});

export default PayPalForm;
