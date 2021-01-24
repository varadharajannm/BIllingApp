import { BrowserModule } from '@angular/platform-browser';
import { NgModule, APP_INITIALIZER } from '@angular/core';
import { FormsModule, ReactiveFormsModule }   from '@angular/forms'; // <-- NgModel lives here
import { HttpModule } from '@angular/http';
import { RouterModule, Routes }   from '@angular/router';

//custom packages
import { NguiAutoCompleteModule } from '@ngui/auto-complete';
import { Daterangepicker } from 'ng2-daterangepicker';
import { ChartsModule } from "ng2-charts";

//components
import { AppComponent } from './app.component';
import { LoginComponent } from './login/login.component';
import { HomeComponent } from './home/home.component';
import { ItemComponent } from './item/item.component';
import { UserComponent } from './user/user.component';
import { BillingComponent } from './billing/billing.component';
import { AlertComponent } from './alert/alert.component';
import { ReportComponent } from './report/report.component';

//services
import { AppConfig, AppGlobals, AppPermissions, AlertService, BillingService } from './_services/index';

//Routing configurations
const appRoutes: Routes = [
    { path: 'login', component: LoginComponent },
    { path: 'home', component: HomeComponent, canActivate: [AppPermissions],
      children : [
        { path: '', component: BillingComponent },
        { path: 'billing', component: BillingComponent },
        { path: 'item', component: ItemComponent },
        { path: 'user', component: UserComponent },
        { path: 'report', component: ReportComponent }
      ]
    },

    //default page
    { path: '', redirectTo: '/login', pathMatch: 'full' },

    // otherwise redirect to login
    { path: '**', redirectTo: '' }
];
const routing = RouterModule.forRoot(appRoutes);

export function initConfig(config: AppConfig){
 return () => config.load() 
}

@NgModule({
  imports: [
    BrowserModule,
    FormsModule,
    ReactiveFormsModule,
    HttpModule,
    routing,
    NguiAutoCompleteModule,
    Daterangepicker,
    ChartsModule
  ],
  declarations: [
    AppComponent,
    LoginComponent,
    HomeComponent,
    ItemComponent,
    UserComponent,
    BillingComponent,
    AlertComponent,
    ReportComponent
  ],
  providers: [
    AppConfig,
    { provide: APP_INITIALIZER, useFactory: initConfig, deps: [AppConfig], multi: true },
    AppPermissions,
    AppGlobals,
    AlertService,
    BillingService
  ],
  bootstrap: [AppComponent]
})

export class AppModule { }
