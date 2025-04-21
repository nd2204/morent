import { StyleSheet } from 'react-native';
import { View } from '@/components/Themed';
import Input from '@/components/ui/input';
import Button from '@/components/ui/button';
import "@/global.css"

export default function TabOneScreen() {
  return (
    <Button
      size="md"
      label="Hello world"
      iconPosition='only'
    />
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    alignItems: 'center',
    justifyContent: 'center',
  },
  title: {
    fontSize: 20,
    fontWeight: 'bold',
  },
  separator: {
    marginVertical: 30,
    height: 1,
    width: '80%',
  },
});
