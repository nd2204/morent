import React, { useState } from 'react';
import { TextInput, View } from 'react-native';
import clsx from 'clsx';
import "@/global.css"

// import { Container } from './styles';

export default function Input({
  className, onFocus, onBlur,
  ...otherProps
}: React.ComponentProps<typeof TextInput>) {
  const [focused, setFocused] = useState<boolean>(false);

  return (
    <TextInput 
      className={clsx(
        // Default
        'flex rounded-md border px-3 py-1 h-9 w-full min-w-0 bg-transparent text-black-100',
        focused && "border-pink-500",
        className
      )}
      onFocus={
        (e) => {
          setFocused(true);
          onFocus && onFocus(e);
        }
      }
      onBlur={
        (e) => {
          setFocused(false);
          onBlur && onBlur(e);
        }
      }
      {...otherProps}
    />
  );
}


// import * as React from "react"

// import { cn } from "@/lib/utils"

// function Input({ className, type, ...props }: React.ComponentProps<"input">) {
//   return (
//     <input
//       type={type}
//       data-slot="input"
//       className={cn(
//         "file:text-foreground placeholder:text-muted-foreground selection:bg-primary selection:text-primary-foreground dark:bg-input/30 border-input flex h-9 w-full min-w-0 rounded-md border bg-transparent px-3 py-1 text-base shadow-xs transition-[color,box-shadow] outline-none file:inline-flex file:h-7 file:border-0 file:bg-transparent file:text-sm file:font-medium disabled:pointer-events-none disabled:cursor-not-allowed disabled:opacity-50 md:text-sm",
//         "focus-visible:border-ring focus-visible:ring-ring/50 focus-visible:ring-[3px]",
//         "aria-invalid:ring-destructive/20 dark:aria-invalid:ring-destructive/40 aria-invalid:border-destructive",
//         className
//       )}
//       {...props}
//     />
//   )
// }

// export { Input }
