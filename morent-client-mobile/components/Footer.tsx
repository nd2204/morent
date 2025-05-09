import React from 'react';
import { View, Text, StyleSheet, TouchableOpacity } from 'react-native';
import { theme } from '../styles/theme';

const Footer: React.FC = () => {
  return (
    <View style={styles.container}>
      <View style={styles.columns}>
        <View style={styles.column}>
          <Text style={styles.columnTitle}>About</Text>
          <TouchableOpacity><Text style={styles.link}>Company</Text></TouchableOpacity>
          <TouchableOpacity><Text style={styles.link}>Team</Text></TouchableOpacity>
          <TouchableOpacity><Text style={styles.link}>Careers</Text></TouchableOpacity>
          <TouchableOpacity><Text style={styles.link}>Blog</Text></TouchableOpacity>
        </View>
        
        <View style={styles.column}>
          <Text style={styles.columnTitle}>Socials</Text>
          <TouchableOpacity><Text style={styles.link}>Twitter</Text></TouchableOpacity>
          <TouchableOpacity><Text style={styles.link}>Instagram</Text></TouchableOpacity>
          <TouchableOpacity><Text style={styles.link}>Facebook</Text></TouchableOpacity>
          <TouchableOpacity><Text style={styles.link}>LinkedIn</Text></TouchableOpacity>
        </View>
        
        <View style={styles.column}>
          <Text style={styles.columnTitle}>Community</Text>
          <TouchableOpacity><Text style={styles.link}>Events</Text></TouchableOpacity>
          <TouchableOpacity><Text style={styles.link}>Forums</Text></TouchableOpacity>
          <TouchableOpacity><Text style={styles.link}>Support</Text></TouchableOpacity>
          <TouchableOpacity><Text style={styles.link}>Contact Us</Text></TouchableOpacity>
        </View>
      </View>
      
      <View style={styles.copyright}>
        <Text style={styles.copyrightText}>
          © 2023 MOMENT. All rights reserved.
        </Text>
      </View>
    </View>
  );
};

const styles = StyleSheet.create({
  container: {
    backgroundColor: theme.colors.footerBackground,
    paddingVertical: 20,
    paddingHorizontal: 16,
  },
  columns: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    marginBottom: 20,
  },
  column: {
    flex: 1,
  },
  columnTitle: {
    fontSize: 16,
    fontWeight: 'bold',
    color: theme.colors.text,
    marginBottom: 10,
  },
  link: {
    fontSize: 14,
    color: theme.colors.textLight,
    marginBottom: 6,
  },
  copyright: {
    borderTopWidth: 1,
    borderTopColor: theme.colors.border,
    paddingTop: 16,
  },
  copyrightText: {
    fontSize: 12,
    color: theme.colors.textLight,
    textAlign: 'center',
  },
});

export default Footer;
