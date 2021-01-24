import { Component, OnInit, ViewChild } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';

import { BillingService } from '../_services/index';

@Component({
	selector: 'app-login',
	templateUrl: 'login.component.html'
})

export class LoginComponent implements OnInit {
	public model: any = {};
	public loginForm: FormGroup;

	constructor(
		private fb: FormBuilder,
		private route: ActivatedRoute,
		private router: Router,
		private billingService: BillingService) { 

		if(localStorage.getItem("b-app-user")){
			this.router.navigate(['/home']);
		}
	}

	ngOnInit(): void {
		// get return url from route parameters or default to '/'
		//alert(this.route.snapshot.queryParams['returnUrl'] || '/');
		this.buildForm();
	}

	buildForm(): void {
		this.model.username = '';
		this.model.password = '';
		this.model.errorMessage = '';

		this.loginForm = this.fb.group({
			'username': [this.model.username, Validators.required],
			'password': [this.model.password, Validators.required]
		});

		this.loginForm.valueChanges
			.subscribe(data => this.onValueChanged(data));

		this.onValueChanged(); // (re)set validation messages now
	}

	login() {
		let parameters = JSON.stringify({ username: this.model.username, password: this.model.password });
		this.billingService.login(parameters).subscribe(
			data => {
				if (data.UserID > 0) {
					localStorage.setItem("b-app-user", JSON.stringify(data));
					this.router.navigate(['/home']);
				}
				else {
					this.model.errorMessage = "Invalid username or password."
				}
			},
			error => {
				this.model.errorMessage = error;
			}
		);
	}

	onValueChanged(data?: any) {
		if (!this.loginForm) { return; }
		const form = this.loginForm;

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
		'username': '',
		'password': ''
	};

	validationMessages = {
		'username': {
			'required': 'Username is required.'
		},
		'password': {
			'required': 'Password is required.'
		}
	};
}


