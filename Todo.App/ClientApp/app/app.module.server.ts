import { NgModule } from "@angular/core";
import { ServerModule } from "@angular/platform-server";
import { NoopAnimationsModule } from "@angular/platform-browser/animations";
import { ToastrModule } from 'ngx-toastr';
import { APP_CONFIG, APP_DI_CONFIG } from "../app/components/app/app.config";
import { GlobalService } from '../app/services/global.service';
import { AuthService } from './services/auth.service';
import { AuthGuardService } from './services/auth-guard.service';
import { ScopeGuardService } from '../app/services/scope-guard.service'
import { AccountService } from '../app/services/account.service';
import { CustomerService } from '../app/services/customer.service';
import { TodoService } from '../app/services/todo.service';
import { MatSnackBar } from '@angular/material';
import { sharedConfig } from "./app.module.shared";

@NgModule({
    bootstrap: sharedConfig.bootstrap,
    declarations: sharedConfig.declarations,
    imports: [
        ServerModule,
        NoopAnimationsModule,
        ToastrModule.forRoot({
            closeButton: true,
            positionClass: 'toast-top-right',
            preventDuplicates: false,
        }),
        ...sharedConfig.imports
    ],
    providers: [
        { provide: APP_CONFIG, useValue: APP_DI_CONFIG },
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
