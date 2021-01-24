import { Component, OnInit } from '@angular/core';

import { BillingService, AlertService } from '../_services/index'
import * as FileSaver from 'file-saver';

declare var moment:any; //using moment.js

@Component({
    selector: 'app-report',
    templateUrl: 'report.component.html'
})

export class ReportComponent implements OnInit {
    public showProgress: boolean = false;
    public daterange: any = {};
    public options: any = {
        locale: { format: 'YYYY-MM-DD' },
        alwaysShowCalendars: false,
        startDate: moment().startOf('month'),
        endDate: moment(),
        ranges: {
               'Last Month': [moment().subtract(1, 'month'), moment()],
               'Last 3 Months': [moment().subtract(4, 'month'), moment()],
               'Last 6 Months': [moment().subtract(6, 'month'), moment()],
               'Last 12 Months': [moment().subtract(12, 'month'), moment()],
        }
    };

    public chartOptions: any = {};
    public chartLabels: string[] = [];
    public chartData: any[] = [];
    public chartDataSet: any[]=[];
    public chartType: string = '';
    public chartLegend: boolean = true;
    public showBarChart: boolean=false;
    public isDrillDown: boolean = false;

    constructor(private billingService: BillingService, private alertService: AlertService){
        this.daterange.start = this.options.startDate;
        this.daterange.end = this.options.endDate;
    }

    ngOnInit(): void {
        this.loadReport();
    }
 
    public selectedDate(value: any) {
        this.daterange.start = value.start;
        this.daterange.end = value.end;
        this.daterange.label = value.label;
    }

    downloadExcel(){
        this.showProgress = true;
        let exportFilter: any = {};
        exportFilter.FromDate = parseInt(this.daterange.start.format('YYYYMMDD'), 10);
        exportFilter.ToDate = parseInt(this.daterange.end.format('YYYYMMDD'),10);
        let parameters = JSON.stringify({ filter: exportFilter });
        this.billingService.downloadBillingInfo(parameters).subscribe(
            data=>{
                this.showProgress = false;
                if (data.length > 0) {
                    this.billingService.downloadFile(data).subscribe(
                        res => {
                            var blob = new Blob([res], { type: 'application/octet-stream' });
                            FileSaver.saveAs(blob, data);
                        },
                        er => {
                            this.alertService.showErrorAlert(er);
                        }
                    );
                }
                else {
                    this.alertService.showErrorAlert("No record found");
                }
            },
            error =>{
                this.showProgress = false;
                this.alertService.showErrorAlert(error);
            }
        );
    }

    loadReport() {
        this.showProgress = true;
        this.showBarChart = false;
        let exportFilter: any = {};
        exportFilter.FromDate = parseInt(this.daterange.start.format('YYYYMMDD'), 10);
        exportFilter.ToDate = parseInt(this.daterange.end.format('YYYYMMDD'), 10);
        exportFilter.IsDrillDown = this.isDrillDown;
        let parameters = JSON.stringify({ filter: exportFilter });
        this.billingService.getReportData(parameters).subscribe(
            data => {
                this.showProgress = false;
                if (data!=null) {
                    if (data.XValues.length > 0 && data.YValues.length > 0 && data.SeriesNames.length > 0) {
                        this.chartOptions = {
                            scaleShowVerticalLines: false,
                            responsive: true,
                            responsiveAnimationDuration: 1,
                            layout: {
                                padding: {
                                    left: 10,
                                    right: 0,
                                    top: 0,
                                    bottom: 10
                                }
                            },
                            title: {
                                display: true,
                                text: 'Sales Revenue'
                            },
                            scales: {
                                yAxes: [{
                                    scaleLabel :{
                                        display: true,
                                        labelString: 'Revenue in Rupees',
                                    },
                                    ticks: {
                                        min: 0,
                                        maxTicksLimit: 11
                                    },
                                    gridLines: {
                                        display: true
                                    }
                                }],
                                xAxes: [{
                                    // type: 'time',
                                    // time: {
                                    //     unit: 'day',
                                    //     tooltipFormat: 'MMM DD, YYYY'
                                    // },
                                    scaleLabel :{
                                        display: true,
                                        labelString: 'Days',
                                    },
                                    gridLines: {
                                        display: true
                                    }
                                }]
                            }
                        };
                        this.chartLabels = data.XValues;
                        var dataForChart = [];
                        for (let i = 0; i < data.YValues.length; i++) {
                            let item: any = {};
                            item.data = data.YValues[i];
                            item.label = data.SeriesNames[i];
                            dataForChart.push(item);
                        }
                        this.chartDataSet = dataForChart;
                        this.chartData = [];
                        this.chartType = "bar";
                        this.showBarChart = true;
                    }
                    else{
                        this.alertService.showErrorAlert("No data available");
                    }
                }
                else {
                    this.alertService.showErrorAlert("No data available");
                }
            },
            error => {
                this.showProgress = false;
                this.alertService.showErrorAlert(error);
            }
        );
    }

    // events
    public chartClicked(e: any): void {
        if (this.isDrillDown == false) {
            this.isDrillDown = true;
            this.loadReport();
        }
    }

    public goUp(){
        this.isDrillDown = false;
        this.loadReport();
    }
}
