import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

/**
 * Provide access to manipulate with item types via HTTP requests to Web API
 */
@Injectable()
export class ItemTypeService {
    readonly itemTypeApi = `${environment.apiUri}/itemtypes`;

    constructor(private http: HttpClient) { }

    getAll(success: Function = () => { }, error: Function = () => { }) {
        return this.http.get(this.itemTypeApi).
            subscribe(result => {
                success(result);
            }, (result) => {
                error(result)
            });
    }
}