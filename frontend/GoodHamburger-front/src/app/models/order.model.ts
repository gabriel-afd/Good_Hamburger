import { OrderItem } from './order-item.model';

export interface Order {
  id: string;
  items: OrderItem[];
  subtotal: number;
  discountPercentage: number;
  discount: number;
  total: number;
  createdAt: string;
  updatedAt?: string;
}
