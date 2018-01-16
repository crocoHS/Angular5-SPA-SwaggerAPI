import { NgModule } from "@angular/core";
import { RouterModule } from "@angular/router";
import { MaterialModule } from '../app/material.module';
import { AppComponent } from "./components/app/app.component";
import { FormsModule } from '@angular/forms';
import { NavMenuComponent } from "./components/navmenu/navmenu.component";
import { DateValueAccessorModule } from 'angular-date-value-accessor';
import { WelcomeComponent } from './components/welcome/welcome.component';
import { CallbackComponent } from './components/callback/callback.component';
import { LoginComponent } from './components/login/login.component';
import { SettingsComponent } from './components/settings/settings.component';
import { ProfileComponent } from './components/profile/profile.component';
import { TodoListComponent } from './components/todo-list/todo-list.component';
import { FormatAccountNumberPipe } from './models/format-account-number.pipe';
import { AccountListComponent } from './components/account/account-list/account-list.component';
import { AccountSummaryComponent } from './components/account/account-summary/account-summary.component';
import { AccountDetailComponent } from './components/account/account-detail/account-detail.component';
import { AccountActivityComponent } from './components/account/acccount-activity/account-activity.component';
import { CustomerComponent } from './components/customer/customer.component';
import { CustomerListComponent } from './components/customer/customer-list.component';
import { HomeComponent } from "./components/home/home.component";
import { FetchDataComponent } from "./components/fetchdata/fetchdata.component";
import { CryptoComponent } from "./components/crypto/crypto.component";
import { EmailComponent } from "./components/email/email.component";
import { AuthGuardService as AuthGuard } from './services/auth-guard.service';
import { ScopeGuardService as ScopeGuard } from "./services/scope-guard.service";

export const sharedConfig: NgModule = {
    bootstrap: [ AppComponent ],
    declarations: [
        AppComponent,
        NavMenuComponent,
        FetchDataComponent,
        CryptoComponent,
        EmailComponent,
        HomeComponent,
        LoginComponent,
        CallbackComponent,
        WelcomeComponent,
        TodoListComponent,
        ProfileComponent,
        SettingsComponent,
        AccountListComponent,
        AccountSummaryComponent,
        AccountDetailComponent,
        AccountActivityComponent,
        FormatAccountNumberPipe,
        CustomerComponent,
        CustomerListComponent
    ],
    imports: [
        DateValueAccessorModule,
        FormsModule,
        MaterialModule,
        RouterModule.forRoot([
            { path: "", redirectTo: "home", pathMatch: "full" },
            { path: "home", component: HomeComponent },
            { path: "fetch-data", component: FetchDataComponent },
            { path: 'crypto', component: CryptoComponent },
            { path: 'email', component: EmailComponent },
            { path: 'settings', component: SettingsComponent, canActivate: [ScopeGuard], data: { expectedScopes: ['add:technology'] } },
            { path: 'login', component: LoginComponent },
            { path: 'callback', component: CallbackComponent },
            { path: 'profile', component: ProfileComponent, canActivate: [AuthGuard] },
            { path: 'todo', component: TodoListComponent, canActivate: [AuthGuard] },
            { path: 'customer', component: CustomerListComponent, canActivate: [AuthGuard] },
            { path: 'customer/:id', component: CustomerComponent },
            { path: 'customer/new', component: CustomerComponent },
            { path: 'customer/:id/account', component: AccountListComponent },
            { path: 'customer/:id/account/:accid', component: AccountDetailComponent },
            { path: "**", redirectTo: "home" }
        ])
    ]
};
