import { Component, Inject, Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import 'rxjs/Rx';
import { Observable } from "rxjs/Observable";
import { Customer } from '../models/customer.type';
import { Technology } from '../models/technology.type';
import { Cacheable } from '../models/cacheable.type' 
import { IAppConfig } from '../models/app-config.type';
import { APP_CONFIG } from '../components/app/app.config';
import { AuthService } from './auth.service';

@Injectable()
export class CustomerService {
    headers = new HttpHeaders({ 'Authorization': 'Bearer ' + this.authService.getToken(), 'Content-Type': 'application/json' });
    options = { headers: this.headers };
    technologyList: Cacheable<Technology[]> = new Cacheable<Technology[]>();

    constructor(private http: HttpClient,
        private authService: AuthService,
        @Inject('API_URL') private apiUrl: string,
        @Inject(APP_CONFIG) public config: IAppConfig) {

        this.technologyList.getHandler = () => {
            return this.http.get(`${this.apiUrl}api/Customer/GetTechnologyList/`)
                .map(response => response as Technology[]);
        }
    }

    public getAll() {
        return this.http.get(`${this.apiUrl}api/Customer/GetAll/`, this.options)
            .catch(this.handleError())
            .map(response => response as Customer[])
            .map(response => response.slice(0, 7));
    }

    public get(id: number) {
        return this.http.get(`${this.apiUrl}api/Customer/Get/${id}`)
            .map(response => {
                let customer = response as Customer
                let date = new Date(customer.registrationDate);
                customer.registrationDate = new Date(date.getFullYear(), date.getMonth(), date.getDate() + 1);
                customer.isActive = true;
                return customer;
            });
    }

    public add(customer: Customer) {
        let body = JSON.stringify(customer);

        return this.http.post(`${this.apiUrl}api/Customer/Create/`, body, this.options);
    }

    public update(customer: Customer) {
        let body = JSON.stringify(customer);
        
        return this.http.put(`${this.apiUrl}api/Customer/Update/`, body, this.options);
    }

    public delete(id: number) {
        return this.http.delete(`${this.apiUrl}api/Customer/Delete/${id}`);
    }

    public getTechnologyList(): Observable<Technology[]> {
        return this.technologyList.getData();
    }

    public addTechnology(technology: Technology) {
        let body = JSON.stringify(technology);

        return this.http.post(`${this.apiUrl}api/Customer/AddTechnology/`, body, this.options);
    }

    public deleteTechnology(name: string) {
        return this.http.delete(`${this.apiUrl}api/Customer/DeleteTechnology/${name}`, this.options);
    }

    handleError() {
        return () => Observable.throw(this.onError());
    }

    onError() {
        this.authService.logout();
        return (response: Response) => {
            Observable.throw(console.error(`Something went wrong. ${response}`));
        }
    }
}
