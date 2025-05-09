import React from 'react';
import { View, Text, StyleSheet, Image } from 'react-native';
import { theme } from '../styles/theme';

const Header: React.FC = () => {
  return (
    <View style={styles.container}>
      <View style={styles.content}>
        <Image
          source={{ uri: 'https://via.placeholder.com/40/3366FF/FFFFFF?text=M' }}
          style={styles.logo}
        />
        <Text style={styles.title}>MOMENT</Text>
      </View>
    </View>
  );
};

const styles = StyleSheet.create({
  container: {
    backgroundColor: theme.colors.white,
    paddingVertical: 16,
    paddingHorizontal: 16,
    borderBottomWidth: 1,
    borderBottomColor: theme.colors.border,
  },
  content: {
    flexDirection: 'row',
    alignItems: 'center',
  },
  logo: {
    width: 30,
    height: 30,
    borderRadius: 15,
  },
  title: {
    fontSize: 18,
    fontWeight: 'bold',
    color: theme.colors.primary,
    marginLeft: 10,
  },
});

export default Header;
