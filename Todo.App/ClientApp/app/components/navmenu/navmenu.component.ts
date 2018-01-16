import { Component } from '@angular/core';
import { AuthService } from '../../services/auth.service';

@Component({
    selector: 'nav-menu',
    templateUrl: './navmenu.component.html',
    styleUrls: ['./navmenu.component.css']
})

export class NavMenuComponent {
    isCollapsed: boolean = true;

    constructor(public auth: AuthService) { }

    toggleCollapse(): void {
        this.isCollapsed = !this.isCollapsed;
    }
}
