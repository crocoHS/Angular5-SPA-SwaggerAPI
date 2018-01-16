import { Component, Inject } from '@angular/core';

@Component({
    selector: 'home',
    templateUrl: './home.component.html'
})
export class HomeComponent {
    public swaggerUrl: string;

    constructor( @Inject('API_URL') public apiUrl: string) { }

    ngOnInit(): void {
        this.swaggerUrl = `${this.apiUrl}swagger/`;
    }
}
