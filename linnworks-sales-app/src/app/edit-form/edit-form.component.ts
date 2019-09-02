import { Component, OnInit, Inject } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA, MatSnackBar } from '@angular/material';
import { SaleService } from '../services/sale.service';
import { Sale } from '../models/sale';
import { Country } from '../models/country';
import { ItemType } from '../models/item-type';

@Component({
  selector: 'app-edit-form',
  templateUrl: './edit-form.component.html',
  styleUrls: ['./edit-form.component.css']
})
export class EditFormComponent {
  isLoading: boolean;
  editSaleForm = this.fb.group({
    id: [],
    unitCost: [null, Validators.required],
    unitPrice: [null, Validators.required],
    unitsSold: [null, Validators.required],
    countryId: [],
    itemTypeId: [],
    salesChanel: [],
    orderPriority: [],
    orderId: [],
    orderDate: [],
    shipDate: []
  });

  get countryId() {
    return this.editSaleForm.get('countryId');
  }

  get itemTypeId() {
    return this.editSaleForm.get('itemTypeId');
  }

  constructor(private fb: FormBuilder, private saleService: SaleService,
    private dialogRef: MatDialogRef<EditFormComponent>, @Inject(MAT_DIALOG_DATA) public data: any,
    private _snackBar: MatSnackBar) {
    this.editSaleForm.patchValue({
      "countryId": data.sale.country.id,
      "itemTypeId": data.sale.itemType.id,
    })
    this.editSaleForm.patchValue(data.sale);
  }

  onSubmit() {
    if (!this.editSaleForm.valid) {
      return;
    }
    let sale = Object.assign({}, this.editSaleForm.value);
    this.isLoading = true;
    this.saleService.update(sale,
      (result) => {
        this._snackBar.open('Sale updated successfully.', '', { duration: 3000 });
        this.dialogRef.close();
        this.isLoading = false
      },
      (result) => {
        this._snackBar.open(`Updating a sale failed. ${result.message}`, '', { duration: 3000 });
        this.dialogRef.close();
        this.isLoading = false
      });
  }

  changeCountry(e) {
    this.countryId.setValue(e.target.value, {
      onlySelf: true
    })
  }

  changeItemType(e) {
    this.itemTypeId.setValue(e.target.value, {
      onlySelf: true
    })
  }
}
