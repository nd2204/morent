import React from 'react';
import { View, Text, StyleSheet, Image, TouchableOpacity } from 'react-native';
import { theme } from '../styles/theme';
import { momoIcon } from '../assets/payment-icons';

const MoMoForm: React.FC = () => {
  return (
    <View style={styles.container}>
      <View style={styles.infoBox}>
        <Text style={styles.infoText}>
          You will be redirected to MoMo to complete your payment securely.
        </Text>
      </View>
      
      <Image source={{ uri: momoIcon }} style={styles.momoLogo} />
      
      <TouchableOpacity style={styles.connectButton}>
        <Text style={styles.connectButtonText}>Pay with MoMo</Text>
      </TouchableOpacity>
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
  momoLogo: {
    width: 120,
    height: 60,
    resizeMode: 'contain',
    marginBottom: 20,
  },
  connectButton: {
    backgroundColor: theme.colors.momo,
    paddingVertical: 12,
    paddingHorizontal: 24,
    borderRadius: 8,
    alignItems: 'center',
    justifyContent: 'center',
    width: '100%',
  },
  connectButtonText: {
    color: theme.colors.white,
    fontSize: 16,
    fontWeight: '600',
  },
});

export default MoMoForm;
