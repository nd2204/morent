import * as React from 'react';
import { View, Image, ScrollView } from 'react-native';
import { SafeAreaView } from 'react-native-safe-area-context';
import { Link, useRouter } from 'expo-router';
import { Text } from '~/components/ui/text';
import { Button } from '~/components/ui/button';
import { Input } from '~/components/ui/input';
import { Card } from '~/components/ui/card';
import { Mail, Lock, Eye, EyeOff, ChevronLeft, User } from 'lucide-react-native';
import { iconWithClassName } from '~/lib/icons/iconWithClassName';
import Animated, { FadeInDown, FadeInUp } from 'react-native-reanimated';

// Register icons with NativeWind
[Mail, Lock, Eye, EyeOff, ChevronLeft, User].forEach(iconWithClassName);

export default function SignupScreen() {
  const router = useRouter();
  const [fullName, setFullName] = React.useState('');
  const [email, setEmail] = React.useState('');
  const [password, setPassword] = React.useState('');
  const [confirmPassword, setConfirmPassword] = React.useState('');
  const [showPassword, setShowPassword] = React.useState(false);
  const [showConfirmPassword, setShowConfirmPassword] = React.useState(false);
  const [isLoading, setIsLoading] = React.useState(false);

  const handleSignup = async () => {
    try {
      setIsLoading(true);
      // TODO: Implement actual signup logic here
      await new Promise(resolve => setTimeout(resolve, 1000)); // Simulate API call
      router.replace('/');
    } catch (error) {
      console.error('Signup failed:', error);
    } finally {
      setIsLoading(false);
    }
  };

  const isFormValid = 
    fullName.length > 0 && 
    email.length > 0 && 
    password.length > 0 && 
    password === confirmPassword;

  return (
    <SafeAreaView className="flex-1 bg-background">
      {/* Logo and Back Button */}
      <View className="flex-row items-center justify-between px-4 py-2">
        <Button
          variant="ghost"
          size="icon"
          className="rounded-full"
          onPress={() => router.back()}
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

      <ScrollView 
        className="flex-1" 
        contentContainerClassName="flex-grow justify-center px-6 py-8"
        showsVerticalScrollIndicator={false}
      >
        <Animated.View entering={FadeInUp.delay(200).springify()}>
          <View className="mb-10">
            <Text className="text-4xl font-bold text-primary mb-3">Create Account</Text>
            <Text className="text-lg text-muted-foreground">
              Start your car rental journey
            </Text>
          </View>
        </Animated.View>

        <Animated.View entering={FadeInDown.delay(400).springify()}>
          <Card className="p-6 gap-y-6 bg-card/50 backdrop-blur-lg">
            <View className="gap-y-4">
              <View className="gap-y-2">
                <Text className="text-sm font-medium">Full Name</Text>
                <View className="relative">
                  <View className="absolute left-3 h-full justify-center">
                    <User size={20} className="text-muted-foreground" />
                  </View>
                  <Input
                    placeholder="Enter your full name"
                    value={fullName}
                    onChangeText={setFullName}
                    className="pl-10 h-12"
                  />
                </View>
              </View>

              <View className="gap-y-2">
                <Text className="text-sm font-medium">Email</Text>
                <View className="relative">
                  <View className="absolute left-3 h-full justify-center border-border">
                    <Mail size={20} className="text-muted-foreground" />
                  </View>
                  <Input
                    placeholder="Enter your email"
                    keyboardType="email-address"
                    autoCapitalize="none"
                    value={email}
                    onChangeText={setEmail}
                    className="pl-10 h-12"
                  />
                </View>
              </View>

              <View className="gap-y-2">
                <Text className="text-sm font-medium">Password</Text>
                <View className="relative">
                  <View className="absolute left-3 h-full justify-center">
                    <Lock size={20} className="text-muted-foreground" />
                  </View>
                  <Input
                    placeholder="Create a password"
                    secureTextEntry={!showPassword}
                    value={password}
                    onChangeText={setPassword}
                    className="pl-10 h-12"
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
              </View>

              <View className="gap-y-2">
                <Text className="text-sm font-medium">Confirm Password</Text>
                <View className="relative">
                  <View className="absolute left-3 h-full justify-center">
                    <Lock size={20} className="text-muted-foreground" />
                  </View>
                  <Input
                    placeholder="Confirm your password"
                    secureTextEntry={!showConfirmPassword}
                    value={confirmPassword}
                    onChangeText={setConfirmPassword}
                    className="pl-10 h-12"
                  />
                  <Button
                    variant="ghost"
                    size="icon"
                    className="absolute right-2 h-full"
                    onPress={() => setShowConfirmPassword(!showConfirmPassword)}
                  >
                    {showConfirmPassword ? (
                      <EyeOff size={20} className="text-muted-foreground" />
                    ) : (
                      <Eye size={20} className="text-muted-foreground" />
                    )}
                  </Button>
                </View>
              </View>
            </View>

            <Button
              className="w-full h-12 mt-2"
              onPress={handleSignup}
              disabled={isLoading || !isFormValid}
            >
              <Text className="text-primary-foreground font-semibold text-base">
                {isLoading ? 'Creating Account...' : 'Create Account'}
              </Text>
            </Button>
          </Card>
        </Animated.View>

        <Animated.View 
          className="flex-row justify-center mt-8" 
          entering={FadeInUp.delay(600).springify()}
        >
          <Text className="text-muted-foreground">Already have an account? </Text>
          <Link href="/login" asChild>
            <Button variant="link" className="p-0">
              <Text className="text-primary font-semibold">Sign In</Text>
            </Button>
          </Link>
        </Animated.View>
      </ScrollView>
    </SafeAreaView>
  );
}