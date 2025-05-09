import { 
  creditCardIcon,
  paypalIcon,
  stripeIcon,
  vnpayIcon,
  momoIcon 
} from '../assets/payment-icons';

export interface CheckoutItem {
  id: number;
  title: string;
  description: string;
  price: string;
  quantity: number;
}

export interface Rating {
  score: number;
  comment?: string;
  created_at: string;
}

export interface Checkout {
  id: number;
  user_id: string;
  amount: string;
  currency: string;
  payment_method: string;
  status: string;
  created_at: string;
  payment_intent_id: string | null;
  metadata: {
    source: string;
    billing_info?: {
      name: string;
      address: string;
      phoneNumber: string;
      townCity: string;
    };
  };
  items?: CheckoutItem[];
  rating?: Rating;
}

// Sample checkout history data
export const checkoutHistory: Checkout[] = [
  {
    id: 1,
    user_id: 'user123',
    amount: '80.00',
    currency: 'USD',
    payment_method: 'CreditCard',
    status: 'completed',
    created_at: '2025-05-09T07:13:47.336Z',
    payment_intent_id: null,
    metadata: {
      source: 'mobile-app',
      billing_info: {
        name: 'John Smith',
        address: '123 Main St',
        phoneNumber: '555-1234',
        townCity: 'New York'
      }
    },
    items: [
      {
        id: 1,
        title: 'Nikon GT-R',
        description: 'All basic features',
        price: '80.00',
        quantity: 1
      }
    ],
    rating: {
      score: 4,
      comment: 'Great camera, fast delivery!',
      created_at: '2025-05-10T09:15:22.336Z'
    }
  },
  {
    id: 2,
    user_id: 'user123',
    amount: '149.99',
    currency: 'USD',
    payment_method: 'PayPal',
    status: 'completed',
    created_at: '2025-05-09T07:13:52.809Z',
    payment_intent_id: null,
    metadata: {
      source: 'mobile-app',
      billing_info: {
        name: 'John Smith',
        address: '123 Main St',
        phoneNumber: '555-1234',
        townCity: 'New York'
      }
    },
    items: [
      {
        id: 2,
        title: 'Canon EOS R5',
        description: 'Professional mirrorless camera',
        price: '149.99',
        quantity: 1
      }
    ]
  },
  {
    id: 3,
    user_id: 'user123',
    amount: '289.99',
    currency: 'USD',
    payment_method: 'Stripe',
    status: 'completed',
    created_at: '2025-05-09T07:13:59.505Z',
    payment_intent_id: null,
    metadata: {
      source: 'mobile-app',
      billing_info: {
        name: 'John Smith',
        address: '123 Main St',
        phoneNumber: '555-1234',
        townCity: 'New York'
      }
    },
    items: [
      {
        id: 3,
        title: 'Sony Alpha A7III',
        description: 'Full-frame mirrorless camera',
        price: '199.99',
        quantity: 1
      },
      {
        id: 4,
        title: '24-70mm f/2.8 Lens',
        description: 'Standard zoom lens',
        price: '90.00',
        quantity: 1
      }
    ],
    rating: {
      score: 5,
      comment: 'Absolutely fantastic camera! Perfect for professional work.',
      created_at: '2025-05-10T16:42:11.505Z'
    }
  },
  {
    id: 4,
    user_id: 'user123',
    amount: '99.99',
    currency: 'USD',
    payment_method: 'VNPAY',
    status: 'completed',
    created_at: '2025-05-09T07:14:05.562Z',
    payment_intent_id: null,
    metadata: {
      source: 'mobile-app',
      billing_info: {
        name: 'John Smith',
        address: '123 Main St',
        phoneNumber: '555-1234',
        townCity: 'New York'
      }
    },
    items: [
      {
        id: 5,
        title: 'Fujifilm X-T4',
        description: 'APS-C mirrorless camera',
        price: '99.99',
        quantity: 1
      }
    ]
  },
  {
    id: 5,
    user_id: 'user123',
    amount: '75.00',
    currency: 'USD',
    payment_method: 'MoMo',
    status: 'completed',
    created_at: '2025-05-09T07:14:11.224Z',
    payment_intent_id: null,
    metadata: {
      source: 'mobile-app',
      billing_info: {
        name: 'John Smith',
        address: '123 Main St',
        phoneNumber: '555-1234',
        townCity: 'New York'
      }
    },
    items: [
      {
        id: 6,
        title: 'GoPro HERO10',
        description: 'Action camera',
        price: '75.00',
        quantity: 1
      }
    ]
  }
];

// Payment method icons mapping
export const paymentMethodIcons = {
  CreditCard: creditCardIcon,
  PayPal: paypalIcon,
  Stripe: stripeIcon,
  VNPAY: vnpayIcon,
  MoMo: momoIcon
};

// Get checkout by ID
export const getCheckoutById = (id: number): Checkout | undefined => {
  return checkoutHistory.find(checkout => checkout.id === id);
};

// Add a new checkout to history (for client-side state management)
export const addCheckout = (checkout: Omit<Checkout, 'id' | 'created_at'>): Checkout => {
  const newCheckout: Checkout = {
    ...checkout,
    id: checkoutHistory.length + 1,
    created_at: new Date().toISOString(),
  };
  
  // In a real app, this would be persisted to a backend
  checkoutHistory.unshift(newCheckout);
  return newCheckout;
};

// Add or update a rating for a checkout
export const addRating = (checkoutId: number, score: number, comment?: string): Checkout | null => {
  const index = checkoutHistory.findIndex(checkout => checkout.id === checkoutId);
  
  if (index === -1) {
    return null;
  }
  
  const checkout = { ...checkoutHistory[index] };
  
  checkout.rating = {
    score,
    comment,
    created_at: new Date().toISOString()
  };
  
  // Update the checkout in the array
  checkoutHistory[index] = checkout;
  
  return checkout;
};

// Get average rating across all checkouts
export const getAverageRating = (): number => {
  const checkoutsWithRating = checkoutHistory.filter(checkout => checkout.rating);
  
  if (checkoutsWithRating.length === 0) {
    return 0;
  }
  
  const totalScore = checkoutsWithRating.reduce(
    (sum, checkout) => sum + (checkout.rating?.score || 0), 
    0
  );
  
  return totalScore / checkoutsWithRating.length;
};