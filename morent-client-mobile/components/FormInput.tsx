import React from 'react';
import { View, Text, TextInput, StyleSheet, TextInputProps } from 'react-native';
import { theme } from '../styles/theme';

interface FormInputProps extends TextInputProps {
  label: string;
  required?: boolean;
  error?: string;
}

const FormInput: React.FC<FormInputProps> = ({ 
  label, 
  required, 
  error, 
  ...props 
}) => {
  return (
    <View style={styles.container}>
      <View style={styles.labelContainer}>
        <Text style={styles.label}>{label}</Text>
        {required && <Text style={styles.required}>*</Text>}
      </View>
      
      <TextInput
        style={[
          styles.input,
          error ? styles.inputError : null,
          props.editable === false ? styles.inputDisabled : null,
        ]}
        placeholderTextColor={theme.colors.placeholder}
        {...props}
      />
      
      {error ? <Text style={styles.errorText}>{error}</Text> : null}
    </View>
  );
};

const styles = StyleSheet.create({
  container: {
    marginBottom: 16,
  },
  labelContainer: {
    flexDirection: 'row',
    marginBottom: 6,
  },
  label: {
    fontSize: 14,
    color: theme.colors.text,
    fontWeight: '500',
  },
  required: {
    fontSize: 14,
    color: theme.colors.error,
    marginLeft: 2,
  },
  input: {
    height: 44,
    borderWidth: 1,
    borderColor: theme.colors.border,
    borderRadius: 8,
    paddingHorizontal: 12,
    fontSize: 16,
    color: theme.colors.text,
    backgroundColor: theme.colors.white,
  },
  inputError: {
    borderColor: theme.colors.error,
  },
  inputDisabled: {
    backgroundColor: theme.colors.disabledBackground,
    color: theme.colors.textLight,
  },
  errorText: {
    fontSize: 12,
    color: theme.colors.error,
    marginTop: 4,
  },
});

export default FormInput;
