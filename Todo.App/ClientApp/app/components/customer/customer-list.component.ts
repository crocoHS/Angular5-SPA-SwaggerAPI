import { Component, Inject, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { CustomerService } from '../../services/customer.service';
import { Customer } from '../../models/customer.type';
import { Technology } from '../../models/technology.type';
import { ToastrService } from 'ngx-toastr';

@Component({
    selector: 'customer-list',
    templateUrl: './customer-list.component.html',
    styleUrls: ['./customer-list.component.css']
})

export class CustomerListComponent implements OnInit {
    public customers: Customer[];
    public customersAll: Customer[];
    public technologyList: Technology[];

    constructor(private router: Router,
        private authService: AuthService,
        private toastr: ToastrService,
        private customerService: CustomerService) { }

    ngOnInit(): void {
        this.getCustomers();
    }

    private getCustomers() {
        this.customerService.getAll()
            .subscribe(customers => {
                this.getTechnologyList();
                customers.map(c => c.fullName = c.firstName + ' ' + c.lastName);
                this.customers = customers;
                this.customersAll = customers;
            });
    }

    private findCustomer = function (haystack: string[], arr: string[]) {       
        return arr.some(function (t) {
            return haystack.indexOf(t) >= 0;
        });
    };

    private getTechnologyList() {
        this.customerService.getTechnologyList()
            .subscribe(technologyList => {
                this.technologyList = technologyList;
            });
    }

    private deleteCustomer(id: number, event: any) {
        if (confirm("Are you sure you want to delete this customer?")) {
            this.customerService.delete(id)
                .subscribe(() => {
                    this.toastr.success('Customer has been deleted successfully');
                    this.getCustomers();
                });
        }
        event.stopPropagation();
    }

    private editCustomer(customerId: number, event: any) {
        this.router.navigate(['customer', customerId]);
        event.stopPropagation();
    }

    public getTechnologyNames(technologyList: Technology[]): string {
        if (technologyList) {
            var technologies = technologyList.map(t => t.technologyName);
            return technologies.join(',').toString();
        }
        return '';
    }

    public getTechnologyListNames(technologyList: Technology[]): string[] {
        if (technologyList) {
            return technologyList.map(t => t.technologyName);
        }
        return [];
    }

    private searchCustomer(technology: string, searchString: string): void {
        searchString = searchString.toLowerCase();

        if (technology != "All") {
            this.customers = this.customersAll.filter(c =>
                this.findCustomer(this.getTechnologyListNames(c.technologyList), [technology]));

            if (searchString != '') {
                this.customers = this.customers.filter(c =>
                    c.fullName.toLowerCase().includes(searchString) ||
                    c.email.toLowerCase().includes(searchString));
            }
        }

        if (technology == "All") {
            if (searchString != '') {
                this.customers = this.customersAll.filter(c =>
                    c.fullName.toLowerCase().includes(searchString) ||
                    c.email.toLowerCase().includes(searchString));            
            }
            else {
                this.customers = this.customersAll;
            }
        }
    }

    isOwner(ownerId: string): boolean {
        if (this.authService.isAdmin())
            return true;

        return this.authService.getUserId() == ownerId;
    }
}
