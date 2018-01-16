import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
    selector: 'welcome',
    templateUrl: './welcome.component.html',
    styleUrls: ['./welcome.component.css']
})
export class WelcomeComponent {
    email: string;

    constructor(
        private router: Router,
        public authService: AuthService) {
        this.authService.subjectUser.subscribe(user => {
            this.email = user && user.email;
        });
    }

    ngOnInit() {
        this.email = this.authService.getUsername();
    }
}
