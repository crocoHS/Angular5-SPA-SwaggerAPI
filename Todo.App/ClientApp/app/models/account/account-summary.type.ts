import { AccountType } from './account-type.enum';

export class AccountSummary {
    accountId: number;
    accountName: string;
    accountNumber: string;
    accountType: AccountType;
    balance: number;

    constructor() {
    }
}