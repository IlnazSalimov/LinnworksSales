import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';

/**
 * Provide access to manipulate with sales via HTTP requests to Web API
 */
@Injectable()
export class CountryService {
    readonly apiUrl = "http://localhost:17258/api/countries";

    constructor(private http: HttpClient) { }

    /**
     * 
     * @param success 
     * @param error 
     */
    getAll(success: Function = () => { }, error: Function = () => { }) {
        return this.http.get(this.apiUrl).
            subscribe((result) => {
                    console.log(result);
                    success(result);
                }, (result) => {
                    error(result)
                });
    }
}