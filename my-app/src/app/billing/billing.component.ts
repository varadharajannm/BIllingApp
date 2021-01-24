import { Component, OnInit, ViewChild } from '@angular/core';
import { DomSanitizer, SafeHtml } from "@angular/platform-browser";

import { BillingItem } from '../_models/billing';
import { AppGlobals, AlertService, BillingService } from '../_services/index'

@Component({
  selector: 'app-billing',
  templateUrl: 'billing.component.html',
})

export class BillingComponent implements OnInit {
  private appParam: any = {};
  public billingItems: BillingItem[] = [];
  public checkoutItems: BillingItem[] = [];
  public totalPrice: number = 0;
  public noItemFound: string = "No Item Found";
  public comments: string = "";

  constructor(private _sanitizer: DomSanitizer,
              private billingService: BillingService,
              private appGlobals: AppGlobals,
              private alertService: AlertService) {
    this.appParam.UserID = appGlobals.currentUserInfo.UserID;
  }

  ngOnInit(): void {
    this.billingService.getItems().subscribe(
      data => {
        if (data.length > 0) {
          this.loadItems(data);
        }
      },
      error => {
        this.alertService.showErrorAlert(error);
      }
    );
  }

  loadItems(itemArray) {
    for (let i = 0; i < itemArray.length; i++) {
      let element = itemArray[i];
      let item: any = {};
      item.id = element.ItemID;
      item.name = element.ItemName;
      item.desc = element.ItemDescription;
      item.price = element.Price;
      item.quantity = 1;
      this.billingItems.push(item);
    }
  }

  public onSelectItem(event) {
    if (event !== "") {
      let selectedItems = this.checkoutItems.filter(i => i.name == event.name);
      if (selectedItems.length == 0) {
        this.checkoutItems.push(JSON.parse(JSON.stringify(event)));
      }
      document.getElementById("txtItemAutoComplete").innerHTML = "";
    }
  }

  increment(selItem) {
    selItem.quantity++;
  }

  decrement(selItem) {
    if (selItem.quantity > 1) {
      selItem.quantity--;
    }
  }

  autocompleListFormatter = (data: any): SafeHtml => {
    let html = `<span>${data.name}</span>`;
    return this._sanitizer.bypassSecurityTrustHtml(html);
  }

  myValueFormatter(data: any): string {
    return ``;
  }

  getTotal() {
    let total = 0;
    for (var i = 0; i < this.checkoutItems.length; i++) {
      let tempItem = this.checkoutItems[i];
      if (tempItem.price) {
        total += (tempItem.quantity * tempItem.price);
      }
    }
    this.totalPrice = total;
    return total;
  }

  proceedCheckout(){
    let itemArray = [];
      this.checkoutItems.forEach(element => {
        let itemInfo: any = {};
        itemInfo.ItemID = element.id;
        itemInfo.ItemPrice = element.price;
        itemInfo.Quantity = element.quantity;
        itemArray.push(itemInfo);
      });

      let billingInfo: any = {};
      billingInfo.TotalPrice = this.totalPrice;
      billingInfo.BillingItemList = itemArray;
      billingInfo.UserID = this.appParam.UserID;
      billingInfo.Comments = this.comments;

      let parameters = JSON.stringify({ billingInfo: billingInfo });
      this.billingService.insertBillingInfo(parameters).subscribe(
        data=>{
          if(data > 0){
            var receiptNo = data;
            document.getElementById('spnModalSuccess').innerHTML = "Receipt Created";
            this.reset();
          }
        },
        error=>{
          document.getElementById('spnModalError').innerHTML = error;
        }
      );
  }

  reset(){
    this.checkoutItems = [];
  }

  clearCheckoutModal(){
      document.getElementById('spnModalSuccess').innerHTML = "";
      document.getElementById('spnModalError').innerHTML = "";
      this.comments = "";
  }

  removeItem(item){
    let index = this.checkoutItems.indexOf(item);
    if(index !== -1){
      this.checkoutItems.splice(index, 1);
    }
  }
}