import { NgModule } from '@angular/core';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import { ToastrModule } from 'ngx-toastr';
import { HttpClientModule } from '@angular/common/http';
import { GlobalService } from '../app/services/global.service';
import { AuthService } from './services/auth.service';
import { AuthGuardService } from './services/auth-guard.service';
import { ScopeGuardService } from '../app/services/scope-guard.service'
import { AccountService } from '../app/services/account.service';
import { CustomerService } from '../app/services/customer.service';
import { TodoService } from '../app/services/todo.service';
import { MatSnackBar } from '@angular/material';
import { APP_CONFIG, APP_DI_CONFIG } from "../app/components/app/app.config";
import { sharedConfig } from './app.module.shared';

@NgModule({
    bootstrap: sharedConfig.bootstrap,
    declarations: sharedConfig.declarations,
    imports: [
        BrowserModule,
        BrowserAnimationsModule,
        NgbModule.forRoot(),
        ToastrModule.forRoot({
            closeButton: true,
            positionClass: 'toast-top-right',
            preventDuplicates: false,
        }),
        FormsModule,
        HttpClientModule,
        ...sharedConfig.imports
    ],
    providers: [
        { provide: APP_CONFIG, useValue: APP_DI_CONFIG },
        { provide: 'ORIGIN_URL', useValue: location.origin },
        { provide: 'BASE_URL', useFactory: getBaseUrl },
        { provide: 'API_URL', useFactory: getApiUrl },
        { provide: 'PROXY_URL', useFactory: getProxyUrl },
        MatSnackBar,
        GlobalService,
        AuthService,
        AuthGuardService,
        ScopeGuardService,
        AccountService,
        CustomerService,
        TodoService
    ]
})
export class AppModule {
}

export function getBaseUrl() {
    return document.getElementsByTagName('base')[0].href;
}

export function getApiUrl() {
    return document.getElementsByTagName('base')[0].getAttribute('api');
}

export function getProxyUrl() {
    return document.getElementsByTagName('base')[0].getAttribute('proxy');
}