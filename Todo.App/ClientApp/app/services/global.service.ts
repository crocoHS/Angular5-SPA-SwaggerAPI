import { Component, Inject, Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import 'rxjs/Rx';
import { Observable } from "rxjs/Observable";
import { IAppConfig } from '../models/app-config.type';
import { APP_CONFIG } from '../components/app/app.config';
import { Consumer } from "../models/consumer.type";
import { PlainText } from "../models/plain-text.type";
import { WeatherForecast } from "../models/weather-forecast.type";
import { EmailMessage } from "../models/email-message.type";
import { ToastrService } from 'ngx-toastr';
import { MatSnackBar } from '@angular/material';

@Injectable()
export class GlobalService {
    constructor(
        private http: HttpClient,
        private router: Router,
        private toastr: ToastrService,
        public snackBar: MatSnackBar,
        @Inject('API_URL') private apiUrl: string,
        @Inject(APP_CONFIG) public config: IAppConfig) {
    }

    public openSnackBar(message: string, action: string) {
        this.snackBar.open(message, action, {
            duration: 5000,
        });
    }

    public getRandomDate(start: Date, end: Date): Date {
        return new Date(start.getTime() + Math.random() * (end.getTime() - start.getTime()));
    }

    public getWeatherForecasts() {
        return this.http.get(`${this.apiUrl}api/Global/WeatherForecasts`)
            .catch(this.handleError())
            .map(response => response as WeatherForecast[]);
    }

    public getConsumersAsync() {
        return this.http.get(`${this.apiUrl}api/Global/GetConsumersAsync`)
            .catch(this.handleError())
            .map(response => response as Consumer[]);
    }

    public encryptText(plainText: PlainText) {
        let body = JSON.stringify(plainText);
        let headers = new HttpHeaders({ 'Content-Type': 'application/json' });

        return this.http.post(`${this.apiUrl}api/Global/Encrypt/`, body, { headers: headers })
            .catch(this.handleError())
            .map(response => response as PlainText);
    }

    public decryptText(plainText: PlainText) {
        let body = JSON.stringify(plainText);
        let headers = new HttpHeaders({ 'Content-Type': 'application/json' });

        return this.http.post(`${this.apiUrl}api/Global/Decrypt/`, body, { headers: headers })
            .catch(this.handleError())
            .map(response => response as PlainText);
    }

    public sendEmailMessage(emailMessage: EmailMessage) {
        let body = JSON.stringify(emailMessage);
        let headers = new HttpHeaders({ 'Content-Type': 'application/json' });

        return this.http.post(`${this.apiUrl}api/Global/SendMailAsync/`, body, { headers: headers })
            .catch(this.handleError());
    }

    public sendEmailWebhook(emailMessage: EmailMessage) {
        let body = JSON.stringify(emailMessage);
        let headers = new HttpHeaders({ 'Content-Type': 'application/json' });

        return this.http.post(`${this.apiUrl}api/Global/SendEmailWebhook/`, body, { headers: headers })
            .catch(this.handleError());
    }

    handleError() {
        return (response: Response) =>
            Observable.throw(this.openSnackBar(`Something went wrong. ${response}`, 'Error'));
    }
}