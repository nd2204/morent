import * as React from 'react';
import { View, Image, Dimensions } from 'react-native';
import { SafeAreaView } from 'react-native-safe-area-context';
import { Link, useRouter } from 'expo-router';
import { Text } from '~/components/ui/text';
import { Button } from '~/components/ui/button';
import { Input } from '~/components/ui/input';
import { Card } from '~/components/ui/card';
import { Mail, Lock, Eye, EyeOff, ChevronLeft } from 'lucide-react-native';
import { iconWithClassName } from '~/lib/icons/iconWithClassName';
import Animated, { FadeInDown, FadeInUp } from 'react-native-reanimated';

// Register icons with NativeWind
[Mail, Lock, Eye, EyeOff, ChevronLeft].forEach(iconWithClassName);

const { width } = Dimensions.get('window');

export default function LoginScreen() {
  const router = useRouter();
  const [email, setEmail] = React.useState('');
  const [password, setPassword] = React.useState('');
  const [showPassword, setShowPassword] = React.useState(false);
  const [isLoading, setIsLoading] = React.useState(false);

  const handleLogin = async () => {
    try {
      setIsLoading(true);
      // TODO: Implement actual login logic here
      await new Promise(resolve => setTimeout(resolve, 1000)); // Simulate API call
      router.replace('/');
    } catch (error) {
      console.error('Login failed:', error);
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <SafeAreaView className="flex-1 bg-background">
      {/* Logo and Back Button */}
      <View className="flex-row items-center justify-between px-4 py-2">
        <Button
          variant="ghost"
          size="icon"
          className="rounded-full"
          onPress={() => router.replace('/')}
        >
          <ChevronLeft size={24} className="text-foreground" />
        </Button>
        <Image 
          source={require('~/assets/images/logo.png')} 
          className="w-24 h-8 opacity-90"
          resizeMode="contain"
        />
        <View className="w-10" /> {/* gapr for alignment */}
      </View>

      <View className="flex-1 justify-center px-6 py-8 mx-6">
        <Animated.View entering={FadeInUp.delay(200).springify()}>
          <View className="mb-10">
            <Text className="text-3xl font-bold text-primary mb-3">Welcome back</Text>
            <Text className="text-lg text-muted-foreground mb-8">
              Your journey continues here
            </Text>
          </View>
        </Animated.View>

        <Animated.View entering={FadeInDown.delay(400).springify()}>
          <Card className="p-6 gap-y-6 bg-card/50">
            <View className="gap-y-4">
              {/*Email/Username Field*/}
              <View className="gap-y-2">
                <Text className="text-sm font-medium">Username</Text>
                <View className="relative flex-row">
                  <View className="relative justify-center border border-muted border-r-0 pl-2 pr-[-2]">
                    <Mail size={20} className="text-muted-foreground " />
                  </View>
                  <Input
                    placeholder="Enter your username or email"
                    keyboardType="email-address"
                    autoCapitalize="none"
                    value={email}
                    onChangeText={setEmail}
                    className="h-12 border-l-0 flex-1"
                  />
                </View>
              </View>

              {/*Password Field*/}
              <View className="gap-y-2">
                <Text className="text-sm font-medium">Password</Text>
                <View className="relative">
                  <View className="absolute left-3 h-full justify-center">
                    <Lock size={20} className="text-muted-foreground" />
                  </View>
                  <Input
                    placeholder="Enter your password"
                    secureTextEntry={!showPassword}
                    value={password}
                    onChangeText={setPassword}
                    className="h-12"
                  />
                  <Button
                    variant="ghost"
                    size="icon"
                    className="absolute right-2 h-full"
                    onPress={() => setShowPassword(!showPassword)}
                  >
                    {showPassword ? (
                      <EyeOff size={20} className="text-muted-foreground" />
                    ) : (
                      <Eye size={20} className="text-muted-foreground" />
                    )}
                  </Button>
                </View>
                <Button variant="link" className="p-0 self-end">
                  <Text className="text-sm text-primary">Forgot Password?</Text>
                </Button>
              </View>
            </View>

            <Button
              className="w-full h-12 mt-2"
              onPress={handleLogin}
              disabled={isLoading || !email || !password}
            >
              <Text className="text-primary-foreground font-semibold text-base">
                {isLoading ? 'Signing in...' : 'Sign In'}
              </Text>
            </Button>
          </Card>
        </Animated.View>

        <Animated.View 
          className="flex-row justify-center mt-8" 
          entering={FadeInUp.delay(600).springify()}
        >
          <Text className="text-muted-foreground">Don't have an account? </Text>
          <Link href="/signup" asChild>
            <Button variant="link" className="p-0">
              <Text className="text-primary font-semibold">Sign Up</Text>
            </Button>
          </Link>
        </Animated.View>
      </View>
    </SafeAreaView>
  );
}