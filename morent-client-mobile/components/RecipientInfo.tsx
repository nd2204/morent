import React from 'react';
import { View, Text, StyleSheet } from 'react-native';
import { theme } from '../styles/theme';
import FormInput from './FormInput';

interface RecipientInfoProps {
  info: {
    name: string;
    address: string;
    phoneNumber: string;
    townCity: string;
  };
  updateInfo: (field: string, value: string) => void;
}

const RecipientInfo: React.FC<RecipientInfoProps> = ({ info, updateInfo }) => {
  return (
    <View style={styles.container}>
      <Text style={styles.header}>Recipient Info</Text>
      
      <FormInput
        label="Name"
        value={info.name}
        onChangeText={(text) => updateInfo('name', text)}
        placeholder="Enter recipient's full name"
        required
      />
      
      <FormInput
        label="Address"
        value={info.address}
        onChangeText={(text) => updateInfo('address', text)}
        placeholder="Enter recipient's address"
        required
      />
      
      <FormInput
        label="Phone Number"
        value={info.phoneNumber}
        onChangeText={(text) => updateInfo('phoneNumber', text)}
        placeholder="Enter recipient's phone number"
        keyboardType="phone-pad"
        required
      />
      
      <FormInput
        label="Town/City"
        value={info.townCity}
        onChangeText={(text) => updateInfo('townCity', text)}
        placeholder="Enter recipient's town or city"
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

export default RecipientInfo;
