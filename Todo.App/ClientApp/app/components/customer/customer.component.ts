import { Component, Input, OnInit, Inject } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { GlobalService } from '../../services/global.service';
import { CustomerService } from '../../services/customer.service';
import { AuthService } from '../../services/auth.service';
import { Customer } from '../../models/customer.type';
import { Technology } from "../../models/technology.type";
import { IAppConfig } from '../../models/app-config.type';
import { APP_CONFIG } from '../../components/app/app.config';
import { ToastrService } from 'ngx-toastr';

@Component({
    selector: 'customer',
    templateUrl: './customer.component.html'
})

export class CustomerComponent implements OnInit {
    public customer: Customer = new Customer({
        ownerId: this.authService.getUserId(),
        gender: "M",
        isActive: true
    });
    public selectedTechnologies: string[] = [];
    public technologyList: string[];
    public customerId: string;
    public isNew: boolean = false;

    constructor(
        private activatedRoute: ActivatedRoute,
        private router: Router,
        private authService: AuthService,
        private globalService: GlobalService,
        private customerService: CustomerService,
        private toastr: ToastrService,
        @Inject(APP_CONFIG) public config: IAppConfig) {
    }

    ngOnInit() {
        this.customerId = this.activatedRoute.snapshot.params['id'] as string;
        if (this.customerId == 'new') {
            this.isNew = true;
            this.getTechnologyList();
        }
        else {
            this.getCustomer(parseInt(this.customerId));
        }
    }

    private getCustomer(id: number) {
        this.customerService.get(id)
            .subscribe(customer => {
                this.customer = customer;
                this.selectedTechnologies = this.customer.technologyList.map(t => t.technologyName);
                this.getTechnologyList();
            });
    }

    private getTechnologyList() {
        this.customerService.getTechnologyList()
            .subscribe(technologyList => {
                this.technologyList = technologyList.map(t => t.technologyName);
            });
    }

    public addCustomer() {
        this.customer.technologyList = this.getCustomerTechnologies();
        this.customerService.add(this.customer)
            .subscribe(() => {
                this.router.navigate(["customer"]);
                this.toastr.success('Customer has been add successfully');
            });
    }

    public updateCustomer() {
        this.customer.technologyList = this.getCustomerTechnologies();
        this.customerService.update(this.customer)
            .subscribe(() => {
                this.router.navigate(['customer']);
                this.toastr.success('Customer has been updated successfully');
            });
    }

    public getCustomerTechnologies() {
        let technologies: Technology[] = [];
        this.customerService.getTechnologyList()
            .subscribe(technologyList => {
                technologyList.map(t => {
                    if (this.selectedTechnologies.indexOf(t.technologyName) != -1) {
                        technologies.push(new Technology({ technologyId: t.technologyId, technologyName: t.technologyName }));
                    }
                });
            });

        return technologies;
    }
}