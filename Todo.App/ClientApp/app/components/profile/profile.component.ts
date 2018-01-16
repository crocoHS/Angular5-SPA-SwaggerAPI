import { Component, OnInit, Inject } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { Theme } from "../../models/theme.type";
import { IAppConfig } from '../../models/app-config.type';
import { APP_CONFIG } from '../../components/app/app.config';

@Component({
    selector: 'app-profile',
    templateUrl: './profile.component.html',
    styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {
    profile: any;
    theme: Theme;

    constructor(
        public auth: AuthService,
        @Inject(APP_CONFIG) public config: IAppConfig) { }

    ngOnInit(): void {
        this.profile = {
            name: this.auth.getUsername(),
            nickname: this.auth.getUsername(),
            picture: '//ssl.gstatic.com/accounts/ui/avatar_2x.png'
        };

        this.getProfile();
        this.theme = this.config.THEMES[1];
    }

    getProfile() {
        if (this.auth.isFormsAuthentication()) 
            return;

        if (this.auth.isAuthenticated()) {
            if (this.auth.userProfile) {
                this.profile = this.auth.userProfile;
            }
            else {
                this.auth.getProfile((err: any, profile: any) => { this.profile = profile; });
            }
        }
    }
}
