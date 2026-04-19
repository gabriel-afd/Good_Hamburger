import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Order } from '../../models/order.model';

export interface OrderRequest {
  menuItemIds: string[];
}

@Injectable({
  providedIn: 'root'
})
export class OrderService {

  private readonly apiUrl = 'https://localhost:7055/api/Orders';

  constructor(private http: HttpClient) { }

  getAll(): Observable<Order[]>{
    return this.http.get<Order[]>(this.apiUrl);
  }

  getById(id: string): Observable<Order>{
    return this.http.get<Order>(`${this.apiUrl}/${id}`);
  }

  create(request: OrderRequest): Observable<Order>{
    return this.http.post<Order>(this.apiUrl, request)
  }

  update(id: string, request: OrderRequest): Observable<Order> {
    return this.http.put<Order>(`${this.apiUrl}/${id}`, request);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
