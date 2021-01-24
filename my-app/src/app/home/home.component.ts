import { Component } from '@angular/core';  
import { Router } from '@angular/router';
import { Title } from '@angular/platform-browser';

import { AppConfig, AppGlobals } from '../_services/index';

@Component ({  
   selector: 'app-home',  
   templateUrl: 'home.component.html'
})  

export class HomeComponent {
    public appTitle: string;
    public userDisplayName: string;
    public showLinkUser: boolean = true;

    constructor(private config: AppConfig, 
                private router: Router,
                private titleService: Title,
                private appGlobals: AppGlobals) {

        this.appTitle = config.getConfig("appTitle");
        this.titleService.setTitle(this.appTitle);

        let currentUserInfo = JSON.parse(localStorage.getItem("b-app-user"));
        appGlobals.currentUserInfo = currentUserInfo;
        this.userDisplayName = appGlobals.currentUserInfo.FirstName;

        if(appGlobals.currentUserInfo.AccessLevel != 1) {
            this.showLinkUser = false;
        }
    }

    logout() {
        localStorage.removeItem("b-app-user");
        this.router.navigate(['/login']);
    }
}