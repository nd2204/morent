import * as React from "react"
import { cva, type VariantProps } from "class-variance-authority"
import { cn } from "@/lib/utils"

const customBadgeVariants = cva(
  "inline-flex items-center rounded-full border px-2.5 py-0.5 text-xs font-semibold transition-colors focus:outline-none focus:ring-2 focus:ring-ring focus:ring-offset-2",
  {
    variants: {
      variant: {
        default:
          "border-transparent bg-primary text-primary-foreground hover:bg-primary/80",
        secondary:
          "border-transparent bg-secondary text-secondary-foreground hover:bg-secondary/80",
        destructive:
          "border-transparent bg-destructive text-destructive-foreground hover:bg-destructive/80",
        outline: "text-foreground",
        success:
          "border-transparent bg-green-100 text-green-800 hover:bg-green-100",
        warning:
          "border-transparent bg-yellow-100 text-yellow-800 hover:bg-yellow-100",
      },
    },
    defaultVariants: {
      variant: "default",
    },
  }
)

export interface CustomBadgeProps
  extends React.HTMLAttributes<HTMLDivElement>,
    VariantProps<typeof customBadgeVariants> {}

function CustomBadge({ className, variant, ...props }: CustomBadgeProps) {
  return (
    <div className={cn(customBadgeVariants({ variant }), className)} {...props} />
  )
}

export { CustomBadge, customBadgeVariants }