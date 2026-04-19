import { Component } from '@angular/core';
import { OrdersComponent } from './pages/orders/orders.component';

@Component({
  selector: 'app-root',
  imports: [OrdersComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'good-hamburger-front';
}
