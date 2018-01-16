import { Injectable, Inject } from '@angular/core';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, Subject } from 'rxjs';
import 'rxjs/add/operator/map';
import { AuthResult } from '../../app/models/auth-result.type';
import { User } from '../models/user.type';
import * as auth0 from 'auth0-js';
import { IAppConfig } from '../models/app-config.type';
import { APP_CONFIG } from '../components/app/app.config';

@Injectable()
export class AuthService {
    public token: string;
    public subjectUser: Subject<User> = new Subject<User>();
    userProfile: any;
    requestedScopes: string = 'openid profile read:customers add:technology';

    constructor(private http: HttpClient,
        private router: Router,
        private activatedRoute: ActivatedRoute,
        @Inject('BASE_URL') private originUrl: string,
        @Inject('API_URL') public apiUrl: string,
        @Inject(APP_CONFIG) private config: IAppConfig) {
        if (typeof window === 'undefined' || localStorage.getItem('currentUser') == null)
            return;
    }

    login(user: User): Observable<boolean> {
        let body = JSON.stringify({ email: user.email, password: user.password });
        let headers = new HttpHeaders({ 'Content-Type': 'application/json' });
        let options = { headers: headers };

        return this.http.post(`${this.apiUrl}api/Account/Login`, body, options)
            .map((response: any) => {
                // login successful if there's a jwt token in the response
                let token = response.token;
                let userId = response.userId;
                if (token) {
                    this.subjectUser.next(user);
                    this.userProfile = user;
                    localStorage.setItem('access_token', token);
                    localStorage.setItem('current_user', JSON.stringify({ userId: userId, username: user.email, isFormsAuthentication: true }));
                    return true;
                }
                return false;
            });
    }

    logout(): void {
        this.userProfile = undefined;
        this.subjectUser.next(undefined);

        localStorage.removeItem('access_token');
        localStorage.removeItem('current_user');
        localStorage.removeItem('id_token');
        localStorage.removeItem('expires_at');
        localStorage.removeItem('scopes');

        this.router.navigate(['/login']);
    }

    getUsername(): string {
        if (typeof window === 'undefined') return '';
        let currentUser = JSON.parse(localStorage.getItem('current_user') || '[]');
        return currentUser && currentUser.username;
    }

    getUserId(): string {
        if (typeof window === 'undefined') return '';
        let currentUser = JSON.parse(localStorage.getItem('current_user') || '[]');
        return currentUser && currentUser.userId;
    }

    getToken(): string {
        if (typeof window === 'undefined') return '';
        return localStorage.getItem('access_token') || '';
    }

    getUserProfile(): string {
        if (typeof window === 'undefined') return '';
        let currentUser = JSON.parse(localStorage.getItem('current_user') || '[]');
        return currentUser && currentUser.userProfile;
    }

    isFormsAuthentication(): boolean {
        if (typeof window === 'undefined') return false;
        let currentUser = JSON.parse(localStorage.getItem('current_user') || '[]');
        return currentUser && currentUser.isFormsAuthentication;
    }

    auth0 = new auth0.WebAuth({
        clientID: this.config.CLIENT_ID,
        domain: this.config.DOMAIN,
        audience: this.config.AUDIENCE,
        redirectUri: `${this.originUrl}callback`,
        responseType: 'token id_token',
        scope: this.requestedScopes
    });

    loginAuth0(): void {
        this.auth0.authorize();
    }

    handleAuthentication(): void {
        this.activatedRoute.fragment.subscribe((fragment: string) => {
            let authResult = this.parseHash(fragment);
            if (authResult.accessToken != null) {
                this.setSession(authResult);
                this.getProfile((error: any, profile: any) => {
                    this.subjectUser.next(new User({ email: profile.name }));
                    this.router.navigate(['/profile']);
                });
            }
        });
    }

    parseHash(fragment: string): AuthResult {
        let authResult: AuthResult = new AuthResult();

        if (fragment == null || fragment == '')
            return authResult;

        let arr = fragment.split('&');

        for (var i = 0; i < arr.length; i++) {
            let param = arr[i].split('=');
            switch (param[0]) {
                case 'access_token':
                    authResult.accessToken = param[1];
                    break;
                case 'expires_in':
                    authResult.expiresIn = parseInt(param[1]);
                    break;
                case 'id_token':
                    authResult.idToken = param[1];
                    break;
                default:
                    break;
            }
        }

        return authResult;
    }

    getProfile(cb: any): void {
        const accessToken = localStorage.getItem('access_token');
        if (!accessToken) {
            throw new Error('Access token must exist to fetch profile');
        }
        const self = this;
        this.auth0.client.userInfo(accessToken, (err, profile) => {
            if (profile) {
                self.userProfile = profile;
                localStorage.setItem('current_user', JSON.stringify({ userId: profile.sub.split('|')[1], username: profile.name, userProfile: profile }));
            }
            cb(err, profile);
        });
    }

    setSession(authResult: any): void {
        // Set the time that the access token will expire at
        const expiresAt = JSON.stringify((authResult.expiresIn * 1000) + new Date().getTime());
        const scopes = authResult.scope || this.requestedScopes || '';
        localStorage.setItem('access_token', authResult.accessToken);
        localStorage.setItem('id_token', authResult.idToken);
        localStorage.setItem('expires_at', expiresAt);
        localStorage.setItem('scopes', JSON.stringify(scopes));
    }

    userHasScopes(scopes: Array<string>): boolean {
        if (typeof window === 'undefined') return false;
        let stScopes: string = localStorage.getItem('scopes') || '';
        if (stScopes == '') return false;

        const grantedScopes = JSON.parse(stScopes).split(' ');
        return scopes.every(scope => grantedScopes.includes(scope));
    }

    isAuthenticated(): boolean {
        // DO NOT remove this line to avoid freezing the browser when manually refreshing the page
        if (typeof window === 'undefined')
            return false;

        if (this.isFormsAuthentication()) {
            return true;
        }

        return this.isValidToken();
    }

    isValidToken(): boolean {
        // Check whether the current time is past the access token's expiry time
        let expire = localStorage.getItem('expires_at');
        if (expire == undefined) return false;

        const expiresAt = JSON.parse(expire);
        return new Date().getTime() < expiresAt;
    }

    isAdmin(): boolean {
        let profile = this.getUserProfile();
        if (!profile) return false;

        this.userProfile = profile;
        let p = JSON.stringify(this.userProfile);
        let role: any = p.split('http://core2app.com/roles')[1];
        return role && role.indexOf('admin') != -1;
    }
}