import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import { AuthService } from '../../services/auth.service';
import { User } from '../../models/user.type';

@Component({
    selector: 'login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.css']
})

export class LoginComponent implements OnInit {
    user: User = new User();
    loading = false;
    error = '';
    EMAIL_REGEXP = "^[a-z0-9!#$%&'*+\/=?^_`{|}~.-]+@[a-z0-9]([a-z0-9-]*[a-z0-9])?(\.[a-z0-9]([a-z0-9-]*[a-z0-9])?)*$";

    constructor(
        private router: Router,
        private authService: AuthService) { }

    ngOnInit() {
        this.authService.logout();
    }

    login() {
        this.loading = true;
        this.authService.login(this.user)
            .catch(this.handleError())
            .subscribe(result => {
                if (result === true) {
                    this.router.navigate(['/profile']);
                } else {
                    this.error = `We didn't recognise those log-in details.Please check your email address and password and try again.`;
                    this.loading = false;
                }
            });
    }

    loginAuth0() {
        this.authService.loginAuth0();
    }

    handleError() {
        return () => Observable.throw(this.stopLoading());
    }

    stopLoading() {
        this.loading = false;
        this.error = 'Something went wrong';
    }
}