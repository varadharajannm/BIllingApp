import { Inject, Injectable } from '@angular/core';
import { Http } from '@angular/http';
import { Observable } from 'rxjs/Observable';

@Injectable()
export class AppConfig {

    private config: Object = null;

    constructor(private http: Http) {

    }

    /**
     * Use to get the data found in the second file (config file)
     */
    public getConfig(key: any) {
        return this.config[key];
    }

    public load() {
        return new Promise((resolve, reject) => {
            this.http.get('assets/config/app.config.json').map( res => res.json() ).catch((error: any):any => {
                console.log('Configuration file "app.config.json" could not be read');
                resolve(true);
                return Observable.throw(error.json().error || 'Server error');
            }).subscribe( (data) => {
                this.config = data;
                resolve(true);
            });
        });
    }
}