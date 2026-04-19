import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatDividerModule } from '@angular/material/divider';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MenuItem } from '../../../models/menu-item.model';
import { Order } from '../../../models/order.model';
import { MenuItemService } from '../../../core/services/menu-item.service';
import { CurrencyPipe, NgClass } from '@angular/common';

export interface OrderDialogData {
  order?: Order;
}

@Component({
  selector: 'app-order-dialog',
  imports: [
    ReactiveFormsModule,
    MatDialogModule,
    MatButtonModule,
    MatDividerModule,
    MatProgressSpinnerModule,
    CurrencyPipe,
    NgClass
  ],
  templateUrl: './order-dialog.component.html',
  styleUrl: './order-dialog.component.scss'
})
export class OrderDialogComponent implements OnInit{

  private fb = inject(FormBuilder);
  private menuItemService = inject(MenuItemService);
  private dialogRef = inject(MatDialogRef<OrderDialogComponent>);
  data: OrderDialogData = inject(MAT_DIALOG_DATA);

  form!: FormGroup;

  menuItems = signal<MenuItem[]>([]);
  loading = signal(false);
  selectedIds = signal<string[]>([]);

  sandwiches = computed(() => this.menuItems().filter(i => i.type === 'Sandwich'));
  sides = computed(() => this.menuItems().filter(i => i.type === 'Fries' || i.type === 'Drink'));

  selectedItems = computed(() =>
    this.menuItems().filter(i => this.selectedIds().includes(i.id))
  );

  subtotal = computed(() =>
    this.selectedItems().reduce((sum, i) => sum + i.price, 0)
  );

  discountPercentage = computed(() => {
    const types = new Set(this.selectedItems().map(i => i.type));
    if (types.has('Sandwich') && types.has('Fries') && types.has('Drink')) return 0.2;
    if (types.has('Sandwich') && types.has('Drink')) return 0.15;
    if (types.has('Sandwich') && types.has('Fries')) return 0.1;
    return 0;
  });

  discount = computed(() =>
    Math.round(this.subtotal() * this.discountPercentage() * 100) / 100
  );

  total = computed(() => this.subtotal() - this.discount());

  discountLabel = computed(() => {
    const p = this.discountPercentage();
    if (p === 0.2) return 'Combo completo · 20% de desconto';
    if (p === 0.15) return 'Sanduíche + refrigerante · 15% de desconto';
    if (p === 0.1) return 'Sanduíche + batata · 10% de desconto';
    return null;
  });

  isEditMode = computed(() => !!this.data?.order);
  title = computed(() => this.isEditMode() ? 'Editar pedido' : 'Novo pedido');

  ngOnInit(): void {
    this.form = this.fb.group({});
    this.loadMenu();
  }

  private loadMenu(): void {
    this.loading.set(true);
    this.menuItemService.getAll().subscribe({
      next: (items) => {
        this.menuItems.set(items);

        if (this.data?.order) {
          const ids = this.data.order.items.map(i => i.menuItemId);
          this.selectedIds.set(ids);
        }

        this.loading.set(false);
      },
      error: () => this.loading.set(false)
    });
  }

  toggleItem(item: MenuItem): void {
    const current = this.selectedIds();
    const alreadySelected = current.includes(item.id);

    if (alreadySelected) {
      this.selectedIds.set(current.filter(id => id !== item.id));
      return;
    }

    const sameType = this.menuItems()
      .filter(m => m.type === item.type)
      .map(m => m.id);

    const withoutSameType = current.filter(id => !sameType.includes(id));
    this.selectedIds.set([...withoutSameType, item.id]);
  }

  isSelected(id: string): boolean {
    return this.selectedIds().includes(id);
  }

  confirm(): void {
    if (this.selectedIds().length === 0) return;
    this.dialogRef.close({ menuItemIds: this.selectedIds() });
  }

  cancel(): void {
    this.dialogRef.close();
  }
}
