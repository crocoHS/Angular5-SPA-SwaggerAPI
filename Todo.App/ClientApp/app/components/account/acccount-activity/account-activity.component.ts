import { Component, Input, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { AccountTransaction } from '../../../models/account/account-transaction.type';
import { AccountService } from '../../../services/account.service';

@Component({
    selector: 'account-activity',
    templateUrl: './account-activity.component.html'
})
export class AccountActivityComponent implements OnInit {
    @Input() accountTransactions: AccountTransaction[];
    @Input() customerId: number;
    public accountId: number;

    constructor(private activatedRoute: ActivatedRoute,
        private router: Router,
        private accountService: AccountService) { }

    ngOnInit(): void {
        this.accountId = this.activatedRoute.snapshot.params['accid'] as number;
    }
}
