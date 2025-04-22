import * as React from 'react';
import { View, ScrollView } from 'react-native';
import { SafeAreaView } from 'react-native-safe-area-context';
import { useRouter } from 'expo-router';
import { Text } from '~/components/ui/text';
import { Button } from '~/components/ui/button';
import { Card } from '~/components/ui/card';
import { Avatar, AvatarFallback, AvatarImage } from '~/components/ui/avatar';
import { ChevronLeft, ChevronRight, User, Bell, Moon, LogOut, Shield, HelpCircle } from 'lucide-react-native';
import { iconWithClassName } from '~/lib/icons/iconWithClassName';
import { ThemeToggle } from '~/components/ThemeToggle';

// Register icons with NativeWind
[ChevronLeft, ChevronRight, User, Bell, Moon, LogOut, Shield, HelpCircle].forEach(iconWithClassName);

const MOCK_USER = {
  name: "John Doe",
  email: "john.doe@example.com",
  avatarUrl: "https://github.com/shadcn.png",
};

interface SettingItemProps {
  icon: React.ReactNode;
  title: string;
  onPress?: () => void;
  endElement?: React.ReactNode;
}

function SettingItem({ icon, title, onPress, endElement }: SettingItemProps) {
  return (
    <Button
      variant="ghost"
      className="flex-row items-center justify-between py-4 px-0"
      onPress={onPress}
    >
      <View className="flex-row items-center">
        {icon}
        <Text className="ml-3 text-base">{title}</Text>
      </View>
      {endElement || <ChevronRight size={20} className="text-muted-foreground" />}
    </Button>
  );
}

export default function ProfileScreen() {
  const router = useRouter();

  const handleSignOut = () => {
    // TODO: Implement sign out logic
    router.replace('/');
  };

  return (
    <SafeAreaView className="flex-1 bg-background">
      {/* Header */}
      <View className="flex-row items-center px-4 py-3 border-b border-border">
        <Button
          variant="ghost"
          size="icon"
          className="rounded-full mr-4"
          onPress={() => router.back()}
        >
          <ChevronLeft size={24} className="text-foreground" />
        </Button>
        <Text className="text-xl font-bold text-primary">Profile</Text>
      </View>

      <ScrollView>
        {/* Profile Card */}
        <Card className="m-4 p-4">
          <View className="flex-row items-center">
            <Avatar className="h-16 w-16 mr-4" alt="User avatar">
              <AvatarImage source={{ uri: MOCK_USER.avatarUrl }} />
              <AvatarFallback>
                <Text className="text-lg">{MOCK_USER.name.charAt(0)}</Text>
              </AvatarFallback>
            </Avatar>
            <View>
              <Text className="text-lg font-semibold">{MOCK_USER.name}</Text>
              <Text className="text-muted-foreground">{MOCK_USER.email}</Text>
            </View>
          </View>
        </Card>

        {/* Settings Sections */}
        <View className="px-4">
          {/* Account Settings */}
          <View className="mb-6">
            <Text className="text-base font-semibold mb-2 text-muted-foreground">Account Settings</Text>
            <Card className="divide-y divide-border">
              <SettingItem
                icon={<User size={20} className="text-foreground" />}
                title="Edit Profile"
                onPress={() => {}}
              />
              <SettingItem
                icon={<Bell size={20} className="text-foreground" />}
                title="Notifications"
                onPress={() => {}}
              />
              <SettingItem
                icon={<Moon size={20} className="text-foreground" />}
                title="Dark Mode"
                endElement={<ThemeToggle />}
              />
            </Card>
          </View>

          {/* Support & About */}
          <View className="mb-6">
            <Text className="text-base font-semibold mb-2 text-muted-foreground">Support & About</Text>
            <Card className="divide-y divide-border">
              <SettingItem
                icon={<Shield size={20} className="text-foreground" />}
                title="Privacy Policy"
                onPress={() => {}}
              />
              <SettingItem
                icon={<HelpCircle size={20} className="text-foreground" />}
                title="Help & Support"
                onPress={() => {}}
              />
            </Card>
          </View>

          {/* Sign Out */}
          <Button
            variant="destructive"
            className="w-full mb-8 p-4"
            onPress={handleSignOut}
          >
            <LogOut size={20} className="mr-2 text-destructive-foreground" />
            <Text className="text-destructive-foreground font-semibold">Sign Out</Text>
          </Button>
        </View>
      </ScrollView>
    </SafeAreaView>
  );
}