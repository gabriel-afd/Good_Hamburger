import { Component, inject, OnInit, PLATFORM_ID, signal } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatChipsModule } from '@angular/material/chips';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatTooltipModule } from '@angular/material/tooltip';
import { CurrencyPipe, DatePipe, isPlatformBrowser } from '@angular/common';
import { OrderService } from '../../core/services/order.service';
import { Order } from '../../models/order.model';
import { OrderDialogComponent } from '../../shared/components/order-dialog/order-dialog.component';

@Component({
  selector: 'app-orders',
  imports: [
    MatButtonModule,
    MatCardModule,
    MatChipsModule,
    MatDialogModule,
    MatIconModule,
    MatProgressSpinnerModule,
    MatSnackBarModule,
    MatTooltipModule,
    CurrencyPipe,
    DatePipe
  ],
  templateUrl: './orders.component.html',
  styleUrl: './orders.component.scss'
})
export class OrdersComponent implements OnInit{

  private orderService = inject(OrderService);
  private dialog = inject(MatDialog);
  private snackBar = inject(MatSnackBar);
  private platformId = inject(PLATFORM_ID);

  orders = signal<Order[]>([]);
  loading = signal(false);

  ngOnInit(): void {
    if (isPlatformBrowser(this.platformId)) {
      this.loadOrders();
    }
  }

  loadOrders(): void {
    this.loading.set(true);
    this.orderService.getAll().subscribe({
      next: (data) => {
        this.orders.set(data);
        this.loading.set(false);
      },
      error: () => {
        this.showError('Erro ao carregar pedidos.');
        this.loading.set(false);
      }
    });
  }

  openCreateDialog(): void {
    const ref = this.dialog.open(OrderDialogComponent, {
      width: '480px',
      data: {}
    });

    ref.afterClosed().subscribe(result => {
      if (!result) return;

      this.orderService.create(result).subscribe({
        next: () => {
          this.showSuccess('Pedido criado com sucesso!');
          this.loadOrders();
        },
        error: (err) => this.showError(err.error?.error ?? 'Erro ao criar pedido.')
      });
    });
  }

  openEditDialog(order: Order): void {
    const ref = this.dialog.open(OrderDialogComponent, {
      width: '480px',
      data: { order }
    });

    ref.afterClosed().subscribe(result => {
      if (!result) return;

      this.orderService.update(order.id, result).subscribe({
        next: () => {
          this.showSuccess('Pedido atualizado com sucesso!');
          this.loadOrders();
        },
        error: (err) => this.showError(err.error?.error ?? 'Erro ao atualizar pedido.')
      });
    });
  }

  deleteOrder(order: Order): void {
    if (!confirm('Deseja excluir este pedido?')) return;

    this.orderService.delete(order.id).subscribe({
      next: () => {
        this.showSuccess('Pedido excluído.');
        this.loadOrders();
      },
      error: () => this.showError('Erro ao excluir pedido.')
    });
  }

  discountLabel(order: Order): string {
    const p = order.discountPercentage;
    if (p === 0.2) return 'Combo completo · desconto: 20%';
    if (p === 0.15) return 'Sanduíche + refri · desconto: 15%';
    if (p === 0.1) return 'Sanduíche + batata · desconto: 10%';
    return 'Sem desconto';
  }

  discountColor(order: Order): string {
    const p = order.discountPercentage;
    if (p === 0.2) return 'combo';
    if (p > 0) return 'partial';
    return 'none';
  }

  private showSuccess(msg: string): void {
    this.snackBar.open(msg, 'Fechar', { duration: 3000 });
  }

  private showError(msg: string): void {
    this.snackBar.open(msg, 'Fechar', { duration: 4000, panelClass: 'snack-error' });
  }

}
