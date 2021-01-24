import { Component, OnInit } from '@angular/core';

import { AlertService } from '../_services/index';

@Component({
    selector: 'app-alert',
    templateUrl: 'alert.component.html'
})

export class AlertComponent implements OnInit {
    public alertMessage: string;
    public showError: boolean;
    public message: any;

    constructor(private alertService: AlertService){
    }

    ngOnInit(): void {
        this.alertService.getMessage().subscribe(message=>{
            if (message) {
                this.message = message;
                document.getElementById('btnOpenModal').click();
            }
        });
    }
}