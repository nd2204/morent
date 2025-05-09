import * as React from 'react';
import { View, TextInput } from 'react-native';
import { Text } from '~/components/ui/text';
import { Button } from '~/components/ui/button';
import { Input } from '~/components/ui/input';
import { Avatar, AvatarFallback, AvatarImage } from '~/components/ui/avatar';
import { Menu, Search, Filter, LogIn } from 'lucide-react-native';
import { iconWithClassName } from '~/lib/icons/iconWithClassName';
import { cn } from '~/lib/utils';
import { ThemeToggle } from './ThemeToggle';
import { useRouter } from 'expo-router';

// Register icons with NativeWind
[Menu, Search, Filter, LogIn].forEach(iconWithClassName);

interface HeaderProps {
  showMenu?: boolean;
  showSearchBar?: boolean;
  onPressMenu?: () => void;
  onPressAvatar?: () => void;
  onSearch?: (query: string) => void;
  onFilterPress?: () => void;
  isAuthenticated?: boolean;
}

export function Header({
  showMenu = true,
  showSearchBar = false,
  onPressMenu,
  onPressAvatar,
  onSearch,
  onFilterPress,
  isAuthenticated = false,
}: HeaderProps) {
  const router = useRouter();
  const [searchQuery, setSearchQuery] = React.useState('');

  return (
    <View className="bg-background border-b border-border">
      {/* First Row: Menu, Brand, Avatar, Theme Toggle */}
      <View className="flex-row items-center justify-between px-4 py-3">
        <View className="flex-row items-center gap-3">
          {showMenu && (
            <Button
              variant="ghost"
              size="icon"
              className="rounded-full"
              onPress={onPressMenu}
            >
              <Menu size={24} className="text-foreground" />
            </Button>
          )}
          <Text className="text-xl font-bold text-primary">MORENT</Text>
        </View>

        <View className="flex-row items-center gap-2">
          <ThemeToggle />
          {isAuthenticated ? (
            <Button
              variant="ghost"
              className="p-0 rounded-full"
              onPress={() => router.push('/profile')}
            >
              <Avatar className="h-9 w-9" alt="user avatar">
                <AvatarImage
                  source={{ uri: "https://github.com/shadcn.png" }}
                  className="bg-muted"
                />
                <AvatarFallback>
                  <Text className="text-xs">CN</Text>
                </AvatarFallback>
              </Avatar>
            </Button>
          ) : (
            <Button
              variant="ghost"
              size="icon"
              className="rounded-full"
              onPress={() => router.push('/login')}
            >
              <LogIn size={24} className="text-foreground" />
            </Button>
          )}
        </View>
      </View>

      {/* Second Row: Search Bar */}
      {showSearchBar && (
        <View className="px-4 pb-6">
          <View className="flex-row items-center bg-muted rounded-md">
            <View className="px-3">
              <Search size={20} className="text-muted-foreground" />
            </View>
            <Input
              value={searchQuery}
              onChangeText={(text) => {
                setSearchQuery(text);
                onSearch?.(text);
              }}
              placeholder="Search something here"
              placeholderTextColor="gray"
              className="flex-1 text-base text-foreground bg-transparent border-0 rounded-none"
            />
            <Button
              variant="ghost"
              size="icon"
              onPress={onFilterPress}
              className="h-12 aspect-square rounded-r-md border-l border-border bg-transparent"
            >
              <Filter size={20} className="text-muted-foreground" />
            </Button>
          </View>
        </View>
      )}
    </View>
  );
}

export default Header;