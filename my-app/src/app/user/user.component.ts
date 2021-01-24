import { Component, OnInit, ViewChild } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';

import { User } from '../_models/billing';
import { AppGlobals, AlertService, BillingService } from '../_services/index';

@Component({
	selector: 'app-user',
	templateUrl: 'user.component.html'
})

export class UserComponent implements OnInit {
	private appParam: any = {};
	public users: User[] = [];
	public user: User;
	public addButtonText: string;
	public userForm: FormGroup;
	public isDisabled: boolean;
	public showProgress: boolean;

	constructor(private fb: FormBuilder,
				private billingService: BillingService,
				private alertService: AlertService,
				private appGlobals: AppGlobals) { 
		this.appParam.UserID = appGlobals.currentUserInfo.UserID;
	}

	ngOnInit(): void {
		this.showProgress = true;
		//get all users
		this.billingService.getUsers().subscribe(
			data => {
				this.showProgress = false;
				if (data.length > 0) {
					this.loadUsers(data);
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

	loadUsers(itemArray) {
		for (let i = 0; i < itemArray.length; i++) {
			let element = itemArray[i];
			let user: any = {};
			user.id = element.UserID;
			user.firstname = element.FirstName;
			user.lastname = element.LastName;
			user.username = element.UserName;
			user.password = element.Password;
			this.users.push(user);
		}
	}

	buildForm(): void {
		this.userForm = this.fb.group({
			'id': [this.user.id],
			'firstname': [this.user.firstname, Validators.required],
			'lastname': [this.user.lastname, Validators.required],
			'username': [this.user.username, Validators.required],
			'password': [this.user.password, Validators.required]
		});

		this.userForm.valueChanges
			.subscribe(data => this.onValueChanged(data));

		this.onValueChanged(); // (re)set validation messages now
	}

	newForm() {
		this.user = { id: -1, firstname: '', lastname: '', username: '', password: '' };
		this.buildForm();
		this.addButtonText = 'Add';
		this.isDisabled = false;
	}

	addUser() {
		this.user = this.userForm.value;

		let userInfo: any = {};
		userInfo.UserID = this.user.id;
		userInfo.FirstName = this.user.firstname;
		userInfo.LastName = this.user.lastname;
		userInfo.UserName = this.user.username;
		userInfo.Password = this.user.password;
		userInfo.LoginUserID = this.appParam.UserID;

		var parameters = JSON.stringify({ userInfo: userInfo });
		this.billingService.addOrUpdateUser(parameters).subscribe(
			data=>{
				if(data == -1){
					this.alertService.showErrorAlert("Username already exists.");
				}
				else if (data > 0) {
					if (this.user.id > 0) { //update
						for (let i = 0; i < this.users.length; i++) {
							if (this.users[i].id == this.user.id) {
								this.alertService.showAlert("User updated successfully");
								this.users[i] = JSON.parse(JSON.stringify(this.user));
								break;
							}
						}
					}
					else { //add
						this.alertService.showAlert("User added successfully");
						this.user.id = data;
						this.users.push(JSON.parse(JSON.stringify(this.user)));
						this.newForm();
					}
				}
			},
			error=>{
				this.alertService.showErrorAlert(error);
			}
		);
	}

	editUser(user) {
		this.isDisabled = true;
		this.user = JSON.parse(JSON.stringify(user));
		this.addButtonText = "Update";
	}

	deleteUser(user) {
		let index = this.users.indexOf(user);
		if (index !== -1) {
			let result = confirm("Are you sure want to delete this user?");
			if (result == true) {
				let userFilter: any = {};
				userFilter.SelectedUserID = user.id;
				userFilter.UserID = this.appParam.UserID;

				//delete user
				var parameters = JSON.stringify({ filter: userFilter });
				this.billingService.deleteUser(parameters).subscribe(
					data=>{
						if(data == true){
							let index = this.users.indexOf(user);
							if(this.user.id == this.users[index].id){
								this.newForm();
							}
							this.users.splice(index, 1);
						}
					},
					error=>{
						this.alertService.showErrorAlert(error);
					}
				);
			}
		}
	}

	onValueChanged(data?: any) {
		if (!this.userForm) { return; }
		const form = this.userForm;

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
		'firstname': '',
		'lastname': '',
		'username': '',
		'password': ''
	};

	validationMessages = {
		'firstname': {
			'required': 'Firstname is required.'
		},
		'lastname': {
			'required': 'Lastname is required.'
		},
		'username': {
			'required': 'Username is required.'
		},
		'password': {
			'required': 'Password is required.'
		}
	};

}