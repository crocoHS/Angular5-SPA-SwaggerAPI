import { Component, Inject, Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import 'rxjs/Rx';
import { Observable } from "rxjs/Observable";
import { TodoItem } from '../models/todo-item.type';
import { IAppConfig } from '../models/app-config.type';
import { APP_CONFIG } from '../components/app/app.config';
import { AuthService } from './auth.service';

@Injectable()
export class TodoService {
    headers = new HttpHeaders({ 'Authorization': 'Bearer ' + this.authService.getToken(), 'Content-Type': 'application/json' });
    options = { headers: this.headers };

    constructor(private http: HttpClient,
        public authService: AuthService,
        @Inject('PROXY_URL') private proxyUrl: string,
        @Inject(APP_CONFIG) public config: IAppConfig) {
    }

    public getAll() {
        return this.http.get(`${this.proxyUrl}api/Todo`, this.options)
            .catch(this.handleError())
            .map(response => response as TodoItem[]);
    }

    public get(id: number) {
        return this.http.get(`${this.proxyUrl}api/Todo/Get/${id}`, this.options)
            .map(response => {
                let todo = response as TodoItem
                return todo;
            });
    }

    public add(todo: TodoItem) {
        let body = JSON.stringify(todo);

        return this.http.post(`${this.proxyUrl}api/Todo`, body, this.options);
    }

    public update(todo: TodoItem) {
        let body = JSON.stringify(todo);
        
        return this.http.put(`${this.proxyUrl}api/todo/${todo.id}`, body, this.options);
    }

    public delete(id: number) {
        return this.http.delete(`${this.proxyUrl}api/Todo/${id}`, this.options);
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
