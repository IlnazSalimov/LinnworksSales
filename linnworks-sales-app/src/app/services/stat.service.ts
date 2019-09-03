import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';


@Injectable()
export class StatisticService {
    readonly statApi = `${environment.apiUri}/stat`;

    constructor(private http: HttpClient) { }

    getSoldOrders(args, success: Function = () => { }, error: Function = () => { }) {

        let params = new HttpParams();
        params = params.append('country', args.country);
        params = params.append('year', args.year);
        return this.http.get(`${this.statApi}/getsoldorders`, { params }).
            subscribe((result) => {
                success(result);
            }, (result) => {
                error(result)
            });
    }

    getProfit(args, success: Function = () => { }, error: Function = () => { }) {
        let params = new HttpParams();
        params = params.append('country', args.country);
        params = params.append('year', args.year);
        return this.http.get(`${this.statApi}/getprofit`, { params }).
            subscribe((result) => {
                success(result);
            }, (result) => {
                error(result)
            });
    }
}