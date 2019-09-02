import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Sale } from '../models/sale';

/**
 * Provide access to manipulate with sales via HTTP requests to Web API
 */
@Injectable()
export class SaleService {
    readonly apiUrl = "http://localhost:17258/api/sales";

    constructor(private http: HttpClient) { }

    /**
     * 
     * @param instructor 
     * @param success 
     * @param error 
     */
    create(instructor: Sale, success: Function = () => { },
        error: Function = () => { }) {
        return this.http.post(this.apiUrl, instructor).subscribe(
            (result: Sale) => {
                success(result);
            },(result) => { 
                error(result) 
            });
    }

    /**
     * 
     * @param page 
     * @param count 
     * @param sortColumn 
     * @param direction 
     * @param success 
     * @param error 
     */
    getAll(page: number, count: number, sortColumn: string = "", direction: string = "", country = "",
        success: Function = () => { }, error: Function = () => { }) {

        let params = new HttpParams();
        if(page > 0) params = params.append('page', page.toString());
        if(count > 0) params = params.append('count', count.toString());
        if(sortColumn != "") params = params.append('sortColumn', sortColumn);
        if(direction != "") params = params.append('direction', direction);
        if(country != "") params = params.append('country', country);

        return this.http.get(this.apiUrl, { params: params }).
            subscribe((result) => {
                    console.log(result);
                    success(result);
                }, (result) => {
                    error(result)
                });
    }

    /**
     * 
     * @param sale 
     * @param success 
     * @param error
     */
    delete(sale: Sale, success: Function = () => { }, error: Function = () => { }) {
        return this.http.delete(`${this.apiUrl}/${sale.id}`).
            subscribe((result: Sale) => {
                    success(result);
                },(result) => { 
                    error(result) 
                });
    }

    /**
     * 
     * @param sale 
     * @param success 
     * @param error 
     */
    update(sale: Sale, success: Function = () => { }, error: Function = () => { }) {
        return this.http.put(`${this.apiUrl}/${sale.id}`, sale).
            subscribe((result: Sale) => {
                    success(result);
                },(result) => { 
                    error(result) 
                });
    }
}