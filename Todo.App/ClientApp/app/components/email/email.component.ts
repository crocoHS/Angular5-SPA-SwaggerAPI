import { Component, Input, Inject } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import { IAppConfig } from '../../models/app-config.type';
import { APP_CONFIG } from '../../components/app/app.config';
import { GlobalService } from '../../services/global.service';
import { EmailMessage } from '../../models/email-message.type';

@Component({
    selector: 'email',
    templateUrl: './email.component.html'
})

export class EmailComponent {
    loading = false;
    enableButton = true;
    public useWebHook = false;
    public emailMessage: EmailMessage = new EmailMessage({
        toEmail: this.config.MAIL4SOLLY
    });
    
    constructor(private router: Router,
        @Inject(APP_CONFIG) public config: IAppConfig,
        private service: GlobalService) {
    }

    public sendEmail() {
        this.enableButton = false;
        this.loading = true;

        this.useWebHook ?
            this.sendEmailWebhook() :
            this.sendEmailMessage();
    }

    sendEmailMessage() {
        this.service.sendEmailMessage(this.emailMessage)
            .catch(this.handleError())
            .subscribe((result: string) => {
                this.service.openSnackBar('Your message has been sent!', 'Success');
                this.stopLoading();
            });
    }

    sendEmailWebhook() {
        this.service.sendEmailWebhook(this.emailMessage)
            .catch(this.handleError())
            .subscribe((result: string) => {
                console.log(result);
                this.service.openSnackBar('Your message has been sent by Azure FunctionApp', 'Success');
                this.stopLoading();
                // this.router.navigate(['home']).then(() => { this.service.openSnackBar('Your message has been sent by Azure FunctionApp', 'Success') });
            });
    }

    handleError() {
        return () => Observable.throw(this.stopLoading());
    }

    stopLoading() {
        this.loading = false;
        this.enableButton = true;
    }

}