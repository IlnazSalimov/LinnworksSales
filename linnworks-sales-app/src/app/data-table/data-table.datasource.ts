import { DataSource, CollectionViewer } from '@angular/cdk/collections';
import { Observable, BehaviorSubject } from 'rxjs';
import { Sale } from '../models/sale';
import { SaleService } from '../services/sale.service';

/**
 * Data source for the DataTable view. This class should
 * encapsulate all logic for fetching and manipulating the displayed data
 * (including sorting, pagination, and filtering).
 */
export class DataTableDataSource extends DataSource<Sale> {
    private salesSubject = new BehaviorSubject<Sale[]>([]);
    private loadingSubject = new BehaviorSubject<boolean>(false);

    public loading$ = this.loadingSubject.asObservable();
    public pageInfo;

    constructor(public saleService: SaleService) {
        super();
    }

    /**
     * Connect this data source to the table. The table will only update when
     * the returned stream emits new items.
     * @returns A stream of the items to be rendered.
     */
    connect(collectionViewer: CollectionViewer): Observable<Sale[]> {
        return this.salesSubject.asObservable();
    }

    /**
     *  Called when the table is being destroyed. Use this function, to clean up
     * any open connections or free any held resources that were set up during connect.
     */
    disconnect(collectionViewer: CollectionViewer) {
        this.salesSubject.complete();
        this.loadingSubject.complete();
    }

    loadSales(pageIndex = 1, pageSize = 50, sortColumn = '', sortDirection = 'asc', country = "", error: Function) {
        this.loadingSubject.next(true);
        this.saleService.getAll(pageIndex, pageSize,
            sortColumn, sortDirection, country, result => {
                this.pageInfo = result;
                this.salesSubject.next(result.onePageItems);
                this.loadingSubject.next(false);
            }, (result) => {
                error(result);
                this.loadingSubject.next(false);
            })
    }
}