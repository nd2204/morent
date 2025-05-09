export type PaymentMethodType = 'CreditCard' | 'PayPal' | 'Stripe' | 'VNPAY' | 'MoMo';

export interface BillingInfo {
  name: string;
  address: string;
  phoneNumber: string;
  townCity: string;
}

export interface RecipientInfo {
  name: string;
  address: string;
  phoneNumber: string;
  townCity: string;
}

export interface CardInfo {
  cardNumber: string;
  cardholderName: string;
  expiryDate: string;
  cvv: string;
}

export interface PaymentAgreements {
  termsConditions: boolean;
  subscribeNewsletter: boolean;
}
