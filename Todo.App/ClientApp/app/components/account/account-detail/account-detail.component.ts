import { Component, Input } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AccountDetail } from '../../../models/account/account-detail.type';
import { AccountService } from '../../../services/account.service';

@Component({
    selector: 'account-detail',
    templateUrl: './account-detail.component.html'
})

export class AccountDetailComponent {
    @Input() accountDetail: AccountDetail;

    constructor(
        private activatedRoute: ActivatedRoute, private accountService: AccountService) {
        let id = this.activatedRoute.snapshot.params['accid'] as string;
        this.getAccountDetail(id);
    }

    private getAccountDetail(id: string) {
        this.accountService.getAccountDetail(id)
            .subscribe(accountDetail => {
                this.accountDetail = accountDetail;
            });
    }
}