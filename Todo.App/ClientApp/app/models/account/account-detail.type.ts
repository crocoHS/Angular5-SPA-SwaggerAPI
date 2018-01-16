import { AccountSummary } from './account-summary.type';
import { AccountTransaction } from './account-transaction.type';

export class AccountDetail {
    customerId: number;
    accountSummary: AccountSummary;
    accountTransactions: AccountTransaction[];
}