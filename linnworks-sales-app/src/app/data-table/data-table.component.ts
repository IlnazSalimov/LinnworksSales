import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator, MatSort, MatTable, MatDialog, MatDialogConfig } from '@angular/material';
import { MatSnackBar } from '@angular/material/snack-bar';
import { EditFormComponent } from '../edit-form/edit-form.component';
import { SaleService } from '../services/sale.service';
import { Sale } from '../models/sale';
import { DataTableDataSource } from './data-table.datasource';
import { merge } from 'rxjs';
import { tap } from 'rxjs/operators';
import { ImportFormComponent } from '../import-form/import-form.component';
import { SelectionModel } from '@angular/cdk/collections';
import { Country } from '../models/country';
import { CountryService } from '../services/coutry.service';
import { ItemType } from '../models/item-type';
import { Validators, FormBuilder } from '@angular/forms';
import { StatisticService } from '../services/stat.service';
import { ItemTypeService } from '../services/item-type.service';

@Component({
  selector: 'app-data-table',
  templateUrl: './data-table.component.html',
  styleUrls: ['./data-table.component.css']
})
export class DataTableComponent implements OnInit {
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatTable) table: MatTable<Sale>;

  dataSource: DataTableDataSource;
  selection = new SelectionModel<Sale>(true, []);
  countries: Country[];
  itemTypes: ItemType[];
  selectedCountry = "";

  /** Columns displayed in the table. Columns IDs can be added, removed, or reordered. */
  displayedColumns = ['select', 'id', 'country', 'itemType', 'salesChanel', 'orderPriority', 'orderId', 'orderDate',
    'shipDate', 'unitsSold', 'unitPrice', 'unitCost', 'delete'];

  soldOrders = 0;
  isSoldOrdersFormLoading: boolean;
  soldOrdersForm = this.fb.group({
    country: [null, Validators.required],
    year: [2010, [Validators.required, Validators.pattern(/^\d{4}$/)]]
  });
  get soldOrdersFormCountryId() {
    return this.soldOrdersForm.get('country');
  }

  profit = 0;
  isProfitFormLoading: boolean;
  profitForm = this.fb.group({
    country: [null, Validators.required],
    year: [2010, [Validators.required, Validators.pattern(/^\d{4}$/)]]
  });
  get profitFormCountryId() {
    return this.profitForm.get('country');
  }

  constructor(private fb: FormBuilder, private saleService: SaleService, private countryService: CountryService,
    private statisticService: StatisticService, private itemTypeService: ItemTypeService,
    private dialog: MatDialog, private _snackBar: MatSnackBar) { }

  ngOnInit() {
    this.reloadData();
    this.reloadCountryCollection();
    this.reloadItemTypeCollection();
  }

  ngAfterViewInit() {
    this.sort.sortChange.subscribe(() => this.paginator.pageIndex = 0);

    merge(this.sort.sortChange, this.paginator.page).pipe(
      tap(() => this.reloadData())
    ).subscribe();
  }

  private onDelete(sale: Sale) {
    this.saleService.delete(sale, () => {
      this._snackBar.open('Sale deleted successfully.', '', { duration: 3000 });
      this.reloadData();
    }, (result) => {
      this._snackBar.open('Deleting a instructor failed.', '', { duration: 3000 });
    });
  }

  private onEdit(sale: Sale) {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.autoFocus = true;
    dialogConfig.width = '350px';
    dialogConfig.data = {
      sale,
      countries: this.countries,
      itemTypes: this.itemTypes
    }
    this.dialog.open(EditFormComponent, dialogConfig).
      afterClosed().subscribe((result) => {
        if(result != undefined && result.isSuccessUpdate)
          this.reloadData();
      });
  }

  private onReloadClick() {
    this._snackBar.dismiss();
    this.reloadData();
    this.reloadCountryCollection();
    this.reloadItemTypeCollection();
  }

  private reloadData() {
    this.selection.clear()
    this.dataSource = new DataTableDataSource(this.saleService);
    this.dataSource.loadSales(this.paginator.pageIndex + 1, this.paginator.pageSize,
      this.sort.active, this.sort.direction, this.selectedCountry,
      result => this._snackBar.open(`Loading sales failed. ${result.message}`)
    );
  }

  private reloadCountryCollection() {
    this.countryService.getAll((countries) => {
      this.countries = countries;
    }, result => this._snackBar.open(`Loading countries failed. ${result.message}`));
  }

  private reloadItemTypeCollection() {
    this.itemTypeService.getAll((itemTypes) => {
      this.itemTypes = itemTypes;
    }, result => this._snackBar.open(`Loading item types failed. ${result.message}`));
  }

  private openUploadDialog() {
    let dialogRef = this.dialog.open(ImportFormComponent, {
      width: '50%',
      height: '50%',
    }).afterClosed().subscribe((result) => {
      this.reloadData();
      this.reloadCountryCollection();
      this.reloadItemTypeCollection();
    });
  }

  private isAllSelected() {
    if (!this.dataSource.pageInfo) return;
    const numSelected = this.selection.selected.length;
    const numRows = this.dataSource.pageInfo.onePageItems.length;
    return numSelected === numRows;
  }

  /** Selects all rows if they are not all selected; otherwise clear selection. */
  private masterToggle() {
    this.isAllSelected() ?
      this.selection.clear() :
      this.dataSource.pageInfo.onePageItems.forEach(row => this.selection.select(row));
  }

  /** The label for the checkbox on the passed row */
  private checkboxLabel(row?: Sale): string {
    if (!row) {
      return `${this.isAllSelected() ? 'select' : 'deselect'} all`;
    }
    return `${this.selection.isSelected(row) ? 'deselect' : 'select'} row ${row.id + 1}`;
  }

  private applyCountryFilter() {
    this.reloadData();
  }

  private deleteBulk() {
    this.saleService.bulkDelete(this.selection.selected, () => {
      this.reloadData();
    });
  }

  private onSoldOrdersFormSubmit() {
    if (!this.soldOrdersForm.valid) {
      return;
    }

    this.isSoldOrdersFormLoading = true;
    this.statisticService.getSoldOrders(this.soldOrdersForm.value, result => {
      this.soldOrders = result;
      this.isSoldOrdersFormLoading = false;
    }, error => {
      this._snackBar.open(`Getting sold orders stat failed. ${error.message}`);
      this.isSoldOrdersFormLoading = false;
    });
  }

  private onProfitFormSubmit() {
    if (!this.profitForm.valid) {
      return;
    }

    this.isProfitFormLoading = true;
    this.statisticService.getProfit(this.profitForm.value, (result) => {
      this.profit = result;
      this.isProfitFormLoading = false;
    }, error => {
      this._snackBar.open(`Getting profit stat failed. ${error.message}`);
      this.isProfitFormLoading = false;
    });
  }

  private soldOrdersFormChangeCountry(e) {
    this.soldOrdersFormCountryId.setValue(e.target.value, {
      onlySelf: true
    })
  }

  private profitFormChangeCountry(e) {
    this.profitFormCountryId.setValue(e.target.value, {
      onlySelf: true
    })
  }
}

