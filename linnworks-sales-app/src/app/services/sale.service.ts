import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Sale } from '../models/sale';
import { environment } from 'src/environments/environment';

/**
 * Provide access to manipulate with sales via HTTP requests to Web API
 */
@Injectable()
export class SaleService {
    readonly saleApi = `${environment.apiUri}/sales`;

    constructor(private http: HttpClient) { }

    create(instructor: Sale, success: Function = () => { },
        error: Function = () => { }) {
        return this.http.post(this.saleApi, instructor).subscribe(
            (result: Sale) => {
                success(result);
            }, (result) => {
                error(result)
            });
    }

    getAll(page: number, count: number, sortColumn: string = "", direction: string = "", country = "",
        success: Function = () => { }, error: Function = () => { }) {

        let params = new HttpParams();
        if (page > 0) params = params.append('page', page.toString());
        if (count > 0) params = params.append('count', count.toString());
        if (sortColumn != "") params = params.append('sortColumn', sortColumn);
        if (direction != "") params = params.append('direction', direction);
        if (country != "") params = params.append('country', country);

        return this.http.get(this.saleApi, { params: params }).
            subscribe((result) => {
                success(result);
            }, (result) => {
                error(result)
            });
    }

    delete(sale: Sale, success: Function = () => { }, error: Function = () => { }) {
        return this.http.delete(`${this.saleApi}/${sale.id}`).
            subscribe((result: Sale) => {
                success(result);
            }, (result) => {
                error(result)
            });
    }

    bulkDelete(sales: Sale[], success: Function = () => { }, error: Function = () => { }) {
        if (sales.length == 0) return;
        let params = new HttpParams();
        sales.forEach((value, index) => {
            params = params.append('ids', value.id.toString());
        })
        
        return this.http.delete(`${this.saleApi}`, { params }).
            subscribe((result: Sale) => {
                success(result);
            }, (result) => {
                error(result)
            });
    }

    update(sale: Sale, success: Function = () => { }, error: Function = () => { }) {
        return this.http.put(`${this.saleApi}/${sale.id}`, sale).
            subscribe((result: Sale) => {
                success(result);
            }, (result) => {
                error(result)
            });
    }
}