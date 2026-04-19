import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { MenuItem } from '../../models/menu-item.model';

@Injectable({
  providedIn: 'root'
})
export class MenuItemService {

  private readonly apiUrl = 'https://localhost:7055/api/Menu';

  constructor(private http: HttpClient) { }

  getAll(): Observable<MenuItem[]>{
    return this.http.get<MenuItem[]>(this.apiUrl);
  }
}
