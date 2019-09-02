import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator, MatSort, MatTable, MatDialog, MatDialogConfig, MatDialogRef } from '@angular/material';
import { MatSnackBar } from '@angular/material/snack-bar';
import { AddFormComponent } from '../add-form/add-form.component';
import { EditFormComponent } from '../edit-form/edit-form.component';
import { SaleService } from '../services/sale.service';
import { Sale } from '../models/sale';
import { DataTableDataSource } from './data-table.datasource';
import { merge } from 'rxjs';
import { tap } from 'rxjs/operators';
import { ImportFormComponent } from '../import-form/import-form.component';
import { SelectionModel } from '@angular/cdk/collections';
import { Country } from '../models/country';
import { CountryService } from '../services/sale.service copy';
import { ItemType } from '../models/item-type';

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
  selectedCountry;

  /** Columns displayed in the table. Columns IDs can be added, removed, or reordered. */
  displayedColumns = ['select', 'id', 'country', 'itemType', 'salesChanel', 'orderPriority', 'orderId', 'orderDate',
    'shipDate', 'unitsSold', 'unitPrice', 'unitCost', 'delete'];

  constructor(private saleService: SaleService, private countryService: CountryService,
    private dialog: MatDialog, private _snackBar: MatSnackBar) { }

  ngOnInit() {
    this.reloadData();
    this.countryService.getAll((countries) => {
      this.countries = countries;
    });
  }

  ngAfterViewInit() {
    // reset the paginator after sorting
    this.sort.sortChange.subscribe(() => this.paginator.pageIndex = 0);

    merge(this.sort.sortChange, this.paginator.page).pipe(
      tap(() => this.reloadData())
    ).subscribe();
  }

  private onDelete(sale: Sale) {
    this.saleService.delete(sale, () => {
      this._snackBar.open('Sale deleted successfully.', '', { duration: 3000 });
      this.refreshTable();
    },
      (result) => {
        this._snackBar.open('Deleting a instructor failed.', '', { duration: 3000 });
      });
  }

  private onCreate() {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.autoFocus = true;
    this.dialog.open(AddFormComponent, dialogConfig).
      afterClosed().subscribe((result) => {
        this.refreshTable();
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
        this.reloadData();
      });
  }

  private refreshTable() {
    this.paginator._changePageSize(this.paginator.pageSize);
  }

  private reloadData() {
    this.selection.clear()
    this.dataSource = new DataTableDataSource(this.saleService);
    this.dataSource.loadSales(this.paginator.pageIndex + 1, this.paginator.pageSize,
      this.sort.active, this.sort.direction, this.selectedCountry,
      (result) => this._snackBar.open(`Loading sales failed. ${result.message}`)
    );
  }

  public openUploadDialog() {
    let dialogRef = this.dialog.open(ImportFormComponent, {
      width: '50%',
      height: '50%',
    })
  }

  isAllSelected() {
    if (!this.dataSource.pageInfo) return;
    const numSelected = this.selection.selected.length;
    const numRows = this.dataSource.pageInfo.onePageItems.length;
    return numSelected === numRows;
  }

  /** Selects all rows if they are not all selected; otherwise clear selection. */
  masterToggle() {
    this.isAllSelected() ?
      this.selection.clear() :
      this.dataSource.pageInfo.onePageItems.forEach(row => this.selection.select(row));
  }

  /** The label for the checkbox on the passed row */
  checkboxLabel(row?: Sale): string {
    if (!row) {
      return `${this.isAllSelected() ? 'select' : 'deselect'} all`;
    }
    return `${this.selection.isSelected(row) ? 'deselect' : 'select'} row ${row.id + 1}`;
  }

  applyCountryFilter() {
    this.reloadData();
  }
}

