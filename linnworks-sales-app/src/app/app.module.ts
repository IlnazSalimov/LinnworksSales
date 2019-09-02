import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AddFormComponent } from './add-form/add-form.component';
import { EditFormComponent } from './edit-form/edit-form.component';
import { DataTableComponent } from './data-table/data-table.component';
import { HttpClientModule } from '@angular/common/http';
import { MatTableModule, MatPaginatorModule, MatSortModule, MatIconModule, MatInputModule, MatButtonModule, MatSelectModule, MatRadioModule, MatCardModule, MatDialogModule, MatToolbarModule, MatSnackBarModule, MatListModule, MatProgressBarModule, MatCheckboxModule, MatSpinner, MatProgressSpinnerModule } from '@angular/material';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { SaleService } from './services/sale.service';
import { ImportFormComponent } from './import-form/import-form.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { UploadService } from './services/upload.service';
import { CountryService } from './services/sale.service copy';

@NgModule({
  declarations: [
    AppComponent,
    AddFormComponent,
    EditFormComponent,
    DataTableComponent,
    ImportFormComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    HttpClientModule,
    MatTableModule,
    MatPaginatorModule,
    MatSortModule,
    MatIconModule,
    MatInputModule,
    MatButtonModule,
    MatSelectModule,
    MatRadioModule,
    MatCardModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatToolbarModule,
    MatSnackBarModule,
    FormsModule,
    MatListModule,
    MatProgressBarModule,
    MatCheckboxModule,
    MatProgressSpinnerModule
  ],
  providers: [SaleService, UploadService, CountryService],
  bootstrap: [AppComponent],
  entryComponents: [ImportFormComponent, AddFormComponent, EditFormComponent]
})
export class AppModule { }
