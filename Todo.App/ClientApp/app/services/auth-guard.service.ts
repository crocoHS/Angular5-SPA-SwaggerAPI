import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { Observable } from 'rxjs/Observable';

@Injectable()
export class AuthGuardService implements CanActivate {
    constructor(private router: Router) { }

    canActivate(): boolean {
        if (typeof window === 'undefined') return false;
        if (localStorage.getItem('access_token')) return true;

        // Not logged in so redirect to login page
        this.router.navigate(['/login']);
        return false;
    }
}