import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

/**
 * Provide access to manipulate with countries via HTTP requests to Web API
 */
@Injectable()
export class CountryService {
    readonly countryApi = `${environment.apiUri}/countries`;

    constructor(private http: HttpClient) { }

    getAll(success: Function = () => { }, error: Function = () => { }) {
        return this.http.get(this.countryApi).
            subscribe((result) => {
                success(result);
            }, (result) => {
                error(result)
            });
    }
}