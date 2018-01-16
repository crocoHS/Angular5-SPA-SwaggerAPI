import { Component, Input, Output, EventEmitter } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { AccountSummary } from "../../../models/account/account-summary.type";
import { AccountService } from '../../../services/account.service';

@Component({
    selector: 'account-summary',
    templateUrl: './account-summary.component.html',
    styleUrls: ['./account-summary.component.css']
})
export class AccountSummaryComponent {
    @Input() customerId: number;
    @Input() account: AccountSummary;
    @Output() onDelete = new EventEmitter<number>();

    public isAccountDetails: boolean;

    constructor(private activatedRoute: ActivatedRoute, private router: Router, private accountService: AccountService) {
        this.isAccountDetails = this.activatedRoute.snapshot.params['accid'] !== undefined;
    }

    navigateToDetail() {
        this.router.navigate(['customer', this.customerId, 'account',  this.account.accountId]);
    }

    public deleteAccount(event: any) {
        this.accountService.deleteAccount(this.account.accountId)
            .subscribe(result => {
                if (this.isAccountDetails) {
                    this.router.navigate(['/customer', this.customerId, 'account']);
                }
                else {
                    this.onDelete.emit(this.customerId);
                }
            });
        event.stopPropagation();
    }
}
    