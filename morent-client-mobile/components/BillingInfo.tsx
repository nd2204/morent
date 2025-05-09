import React from 'react';
import { View, Text, StyleSheet } from 'react-native';
import { theme } from '../styles/theme';
import FormInput from './FormInput';

interface BillingInfoProps {
  info: {
    name: string;
    address: string;
    phoneNumber: string;
    townCity: string;
  };
  updateInfo: (field: string, value: string) => void;
}

const BillingInfo: React.FC<BillingInfoProps> = ({ info, updateInfo }) => {
  return (
    <View style={styles.container}>
      <Text style={styles.header}>Billing Info</Text>
      
      <FormInput
        label="Name"
        value={info.name}
        onChangeText={(text) => updateInfo('name', text)}
        placeholder="Enter your full name"
        required
      />
      
      <FormInput
        label="Address"
        value={info.address}
        onChangeText={(text) => updateInfo('address', text)}
        placeholder="Enter your address"
        required
      />
      
      <FormInput
        label="Phone Number"
        value={info.phoneNumber}
        onChangeText={(text) => updateInfo('phoneNumber', text)}
        placeholder="Enter your phone number"
        keyboardType="phone-pad"
        required
      />
      
      <FormInput
        label="Town/City"
        value={info.townCity}
        onChangeText={(text) => updateInfo('townCity', text)}
        placeholder="Enter your town or city"
        required
      />
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
});

export default BillingInfo;
