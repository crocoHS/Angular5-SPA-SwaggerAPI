import { Component, Inject, Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import 'rxjs/Rx';

import { AccountSummary } from '../models/account/account-summary.type';
import { AccountDetail } from '../models/account/account-detail.type';
import { AccountType } from '../models/account/account-type.enum';

@Injectable()
export class AccountService {
    constructor(
        private http: HttpClient,
        @Inject('API_URL') private originUrl: string) { }

    public getAccountSummaryAll() {
        return this.http.get(`${this.originUrl}api/Bank/GetAccountSummaryAll`)
            .map(response => response as AccountSummary[]);
    }

    public getAccountSummary(id: number) {
        return this.http.get(`${this.originUrl}api/Bank/GetAccountSummary/${id}`)
            .map(response => response as AccountSummary[]);
    }

    public getAccountDetail(id: string) {
        return this.http.get(`${this.originUrl}api/Bank/GetAccountDetail/${id}`)
            .map(response => response as AccountDetail);
    }

    public addAccount(customerId: number, accountType: number) {
        return this.http.get(`${this.originUrl}api/Bank/AddAccount/${customerId}/${accountType}`)
            .map(response => {
                response as AccountSummary[];
            });
    }

    public deleteAccount(accountId: number) {
        return this.http.get(`${this.originUrl}api/Bank/DeleteAccount/${accountId}`)
            .map(response => {
                response as AccountSummary[];
            });
    }
}
