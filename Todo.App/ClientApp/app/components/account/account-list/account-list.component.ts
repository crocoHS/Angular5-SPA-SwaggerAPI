import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { AccountSummary } from '../../../models/account/account-summary.type';
import { AccountType } from '../../../models/account/account-type.enum';
import { AccountService } from '../../../services/account.service';

@Component({
    selector: 'account-list',
    templateUrl: './account-list.component.html'
})
export class AccountListComponent implements OnInit {
    public customerId: number;
    public hasCashAccounts: boolean = false;
    public hasCreditAccounts: boolean = false;
    public cashAccounts: AccountSummary[];
    public creditAccounts: AccountSummary[];

    constructor(private activatedRoute: ActivatedRoute,
        private router: Router,
        private accountService: AccountService) {
    }

    ngOnInit(): void {
        this.customerId = this.activatedRoute.snapshot.params['id'] as number;
        this.getAccountSummary(this.customerId);
    }

    private getAccountSummary(id: number) {
        this.accountService.getAccountSummary(id)
            .subscribe(accounts => {
                this.cashAccounts = accounts.filter(x => x.accountType == AccountType.Checking || x.accountType == AccountType.Savings);
                this.creditAccounts = accounts.filter(x => x.accountType == AccountType.Credit);
                this.hasCashAccounts = this.cashAccounts.length > 0;
                this.hasCreditAccounts = this.creditAccounts.length > 0;
            });
    }

    public addAccount(accountType: number) {
        this.accountService.addAccount(this.customerId, accountType)
            .subscribe(result => {
                this.getAccountSummary(this.customerId);
            });
    }
}
