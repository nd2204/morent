import React, { useState } from 'react';
import { View, StyleSheet } from 'react-native';
import FormInput from './FormInput';
import { theme } from '../styles/theme';

const CreditCardForm: React.FC = () => {
  const [cardInfo, setCardInfo] = useState({
    cardNumber: '',
    cardholderName: '',
    expiryDate: '',
    cvv: '',
  });

  const updateCardInfo = (field: string, value: string) => {
    setCardInfo(prev => ({ ...prev, [field]: value }));
  };

  // Format card number with spaces after every 4 digits
  const formatCardNumber = (text: string) => {
    // Remove all non-digits
    const cleaned = text.replace(/\D/g, '');
    // Insert space after every 4 digits
    const formatted = cleaned.replace(/(\d{4})(?=\d)/g, '$1 ');
    return formatted.substring(0, 19); // Limit to 16 digits + 3 spaces
  };

  // Format expiry date as MM/YY
  const formatExpiryDate = (text: string) => {
    // Remove all non-digits
    const cleaned = text.replace(/\D/g, '');
    // Insert / after 2 digits
    if (cleaned.length >= 2) {
      return `${cleaned.substring(0, 2)}/${cleaned.substring(2, 4)}`;
    }
    return cleaned;
  };

  return (
    <View style={styles.container}>
      <FormInput
        label="Card Number"
        value={cardInfo.cardNumber}
        onChangeText={(text) => {
          const formatted = formatCardNumber(text);
          updateCardInfo('cardNumber', formatted);
        }}
        placeholder="1234 5678 9012 3456"
        keyboardType="numeric"
        maxLength={19}
        required
      />
      
      <FormInput
        label="Cardholder Name"
        value={cardInfo.cardholderName}
        onChangeText={(text) => updateCardInfo('cardholderName', text)}
        placeholder="Name on card"
        required
      />
      
      <View style={styles.row}>
        <View style={styles.halfWidth}>
          <FormInput
            label="Expiry Date"
            value={cardInfo.expiryDate}
            onChangeText={(text) => {
              const formatted = formatExpiryDate(text);
              updateCardInfo('expiryDate', formatted);
            }}
            placeholder="MM/YY"
            keyboardType="numeric"
            maxLength={5}
            required
          />
        </View>
        
        <View style={styles.halfWidth}>
          <FormInput
            label="CVV"
            value={cardInfo.cvv}
            onChangeText={(text) => {
              // Only allow digits and limit to 3-4 characters
              const cleaned = text.replace(/\D/g, '').substring(0, 4);
              updateCardInfo('cvv', cleaned);
            }}
            placeholder="123"
            keyboardType="numeric"
            maxLength={4}
            secureTextEntry
            required
          />
        </View>
      </View>
    </View>
  );
};

const styles = StyleSheet.create({
  container: {
    marginTop: 8,
  },
  row: {
    flexDirection: 'row',
    justifyContent: 'space-between',
  },
  halfWidth: {
    width: '48%',
  },
});

export default CreditCardForm;
