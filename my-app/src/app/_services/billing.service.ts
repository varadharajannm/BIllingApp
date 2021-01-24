import { Injectable } from '@angular/core';
import { Http, Headers, RequestOptions, Response, ResponseContentType } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/observable/throw';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/map';

import { AppConfig } from './app.config';

@Injectable()
export class BillingService {
	private serviceURL: string;
	
	constructor(private http: Http, private config: AppConfig) { 
		this.serviceURL = config.getConfig("serviceUrl");
	}

	login(parameters: string): Observable<any> {
		return this.http.post(this.serviceURL + '/BillingService.svc/GetUserInfo', parameters, this.requestOptions())
			.map(this.extractData).catch(this.handleError);
	}

	getItems(): Observable<any> {
		return this.http.post(this.serviceURL + '/BillingService.svc/GetItems', '', this.requestOptions())
			.map(this.extractData).catch(this.handleError);
	}

	addOrUpdateItem(parameters: string): Observable<any> {
		return this.http.post(this.serviceURL + '/BillingService.svc/InsertOrUpdateItem', parameters, this.requestOptions())
			.map(this.extractData).catch(this.handleError);
	}

	deleteItem(parameters: string): Observable<any> {
		return this.http.post(this.serviceURL + '/BillingService.svc/DeleteItem', parameters, this.requestOptions())
			.map(this.extractData).catch(this.handleError);
	}

	getUsers(): Observable<any> {
		return this.http.post(this.serviceURL + '/BillingService.svc/GetUsers', '', this.requestOptions())
			.map(this.extractData).catch(this.handleError);
	}

	addOrUpdateUser(parameters: string): Observable<any> {
		return this.http.post(this.serviceURL + '/BillingService.svc/InsertOrUpdateUser', parameters, this.requestOptions())
			.map(this.extractData).catch(this.handleError);
	}

	deleteUser(parameters: string): Observable<any> {
		return this.http.post(this.serviceURL + '/BillingService.svc/DeleteUser', parameters, this.requestOptions())
			.map(this.extractData).catch(this.handleError);
	}

	insertBillingInfo(parameters: string): Observable<any> {
		return this.http.post(this.serviceURL+'/BillingService.svc/InsertBillingInfo', parameters, this.requestOptions())
			.map(this.extractData).catch(this.handleError);
	}

	downloadBillingInfo(parameters: string): Observable<any> {
		return this.http.post(this.serviceURL+'/BillingService.svc/DownloadBillingInfo', parameters,this.requestOptions())
			.map(this.extractData).catch(this.handleError);
	}

	downloadFile(fileName: string) {
		let headers = new Headers({ 'Content-Type': 'application/json' });
		let options = new RequestOptions({ headers: headers, responseType: ResponseContentType.Blob });
		return this.http.get(this.serviceURL+'/DownloadHandler.ashx?file='+fileName, options)
			.map(response => response.blob()).catch(this.handleError);
	}

	getReportData(parameters: string): Observable<any> {
		return this.http.post(this.serviceURL+'/BillingService.svc/GetReportData', parameters, this.requestOptions())
			.map(this.extractData).catch(this.handleError);
	}

	private requestOptions() {
		let headers = new Headers({ 'Content-Type': 'application/json' });
		return new RequestOptions({ headers: headers });
	}

	private extractData(res: Response) {
		let body = res.json();
		return body || {};
	}

	private handleError(error: Response | any) {
		// In a real world app, you might use a remote logging infrastructure
		let errMsg: string;
		if (error instanceof Response) {
			const body = error.json() || '';
			const err = body.error || JSON.stringify(body);
			errMsg = `${error.status} - ${error.statusText || ''} ${err}`;
		} else {
			errMsg = error.message ? error.message : error.toString();
		}
		console.error(errMsg);
		return Observable.throw(errMsg);
	}
}