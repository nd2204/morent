import * as React from 'react';
import { Heart } from 'lucide-react-native';
import { Button } from '~/components/ui/button';
import { cn } from '~/lib/utils';
import { iconWithClassName } from '~/lib/icons/iconWithClassName';

// Register icons with NativeWind
iconWithClassName(Heart);

interface FavoriteButtonProps {
  isFavorite: boolean;
  onToggleFavorite: () => void;
}

export const FavoriteButton = React.memo(function FavoriteButton({
  isFavorite,
  onToggleFavorite,
}: FavoriteButtonProps) {
  return (
    <Button
      variant="ghost"
      className="p-2 rounded-full bg-transparent"
      onPress={onToggleFavorite}
    >
      <Heart
        size={18}
        className={cn(
          isFavorite ? 'text-destructive fill-destructive' : 'text-ring fill-background'
        )}
      />
    </Button>
  );
});