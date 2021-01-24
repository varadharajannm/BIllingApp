import { Component, OnInit, ViewChild } from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormControl } from '@angular/forms';

import { Item } from '../_models/billing';
import { AppGlobals, AlertService, BillingService } from '../_services/index';

@Component({
	selector: 'app-item',
	templateUrl: 'item.component.html'
})

export class ItemComponent implements OnInit {
	private appParam: any = {};
	public items: Item[] = [];
	public item: Item;
	public addButtonText: string;
	public itemForm: FormGroup;
	public showProgress: boolean;

	constructor(
		private fb: FormBuilder,
		private billingService: BillingService,
		private alertService: AlertService,
		private appGlobals: AppGlobals) {
		this.appParam.UserID = appGlobals.currentUserInfo.UserID;
	}

	ngOnInit(): void {
		this.showProgress = true;
		//get all items
		this.billingService.getItems().subscribe(
			data => {
				this.showProgress = false;
				if (data.length > 0) {
					this.loadItems(data);
				}
			},
			error => {
				this.showProgress = false;
				this.alertService.showErrorAlert(error);
			}
		);

		//create new form
		this.newForm();
	}

	loadItems(itemArray) {
		for (let i = 0; i < itemArray.length; i++) {
			let element = itemArray[i];
			let item: any = {};
			item.id = element.ItemID;
			item.name = element.ItemName;
			item.desc = element.ItemDescription;
			item.price = element.Price;
			this.items.push(item);
		}
	}

	buildForm(): void {
		this.itemForm = this.fb.group({
			'id': [this.item.id],
			'name': [this.item.name, [Validators.required]],
			'desc': [this.item.desc],
			'price': [this.item.price, [Validators.required, Validators.min(1)]]
		});

		this.itemForm.valueChanges
			.subscribe(data => this.onValueChanged(data));

		this.onValueChanged(); // (re)set validation messages now
	}

	newForm() {
		this.item = { id: -1, name: '', desc: '', price: 0 }
		this.buildForm();
		this.addButtonText = 'Add';
	}

	addItem() {
		this.item = this.itemForm.value;

		let itemInfo: any = {};
		itemInfo.ItemID = this.item.id;
		itemInfo.ItemName = this.item.name;
		itemInfo.ItemDescription = this.item.desc;
		itemInfo.Price = this.item.price;
		itemInfo.UserID = this.appParam.UserID;
		var parameters = JSON.stringify({ itemInfo: itemInfo });

		this.billingService.addOrUpdateItem(parameters).subscribe(
			data => {
				if (data == -1) {
					this.alertService.showErrorAlert("Item name already exists.");
				}
				else if (data > 0) {
					if (this.item.id > 0) { //update
						let selItems = this.items.filter(i => i.id == this.item.id);
						if (selItems.length > 0) {
							this.alertService.showAlert("Item updated successfully");
							let index = this.items.indexOf(selItems[0]);
							this.items[index] = JSON.parse(JSON.stringify(this.item));
						}
					}
					else {
						this.alertService.showAlert("Item added successfully");
						this.item.id = data;
						this.items.push(JSON.parse(JSON.stringify(this.item)));
						this.newForm();
					}
				}
			},
			error => {
				this.alertService.showErrorAlert(error);
			}
		);
	}

	editItem(item) {
		this.item = JSON.parse(JSON.stringify(item));
		this.addButtonText = 'Update';
	}

	deleteItem(item) {
		let index = this.items.indexOf(item);
		if (index !== -1) {
			let result = confirm("Are you sure want to delete this item?");
			if (result == true) {
				let itemFilter: any = {};
				itemFilter.ItemID = item.id;
				itemFilter.UserID = this.appParam.UserID;

				//delete item
				var parameters = JSON.stringify({ filter: itemFilter });
				this.billingService.deleteItem(parameters).subscribe(
					data => {
						if (data == true){
							let index = this.items.indexOf(item);
							if(this.item.id == this.items[index].id){
								this.newForm();
							}
							this.items.splice(index, 1);
						}
					},
					error => {
						this.alertService.showErrorAlert(error);
					}
				);
			}
		}
	}

	onValueChanged(data?: any) {
		if (!this.itemForm) { return; }
		const form = this.itemForm;

		for (const field in this.formErrors) {
			// clear previous error message (if any)
			this.formErrors[field] = '';
			const control = form.get(field);

			if (control && control.dirty && !control.valid) {
				const messages = this.validationMessages[field];
				for (const key in control.errors) {
					this.formErrors[field] += messages[key] + ' ';
				}
			}
		}
	}

	formErrors = {
		'name': '',
		'price': ''
	};

	validationMessages = {
		'name': {
			'required': 'Name is required.'
		},
		'price': {
			'required': 'Price is required.',
			'min': 'Price should not be negative.'
		}
	};
}