import { Component, Input, OnInit, Inject } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormControl } from '@angular/forms';
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
        isActive: true,
        registrationDate: new Date()
    });
    technologyList: Technology[];
    customerTechnology = new FormControl();
    selectedTechnologies: string[] = [];
    isNew: boolean = false;

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
        let customerId = this.activatedRoute.snapshot.params['id'] as string;
        if (customerId == 'new') {
            this.isNew = true;
            this.getTechnologyList();
        }
        else {
            this.getCustomer(parseInt(customerId));
        }
    }

    getCustomer(id: number) {
        this.customerService.get(id)
            .subscribe(customer => {
                this.customer = customer;
                this.getTechnologyList();
            });
    }

    getTechnologyList() {
        this.customerService.getTechnologyList()
            .subscribe(technologyList => {
                this.technologyList = technologyList;
                this.selectedTechnologies = this.customer.technologyList && this.customer.technologyList.map(t => t.technologyName);
                this.customerTechnology.setValue(this.getSelectedTechnologies());
            });
    }

    getSelectedTechnologies() {
        let technologies: Technology[] = [];
        if (!this.selectedTechnologies) return technologies;

        this.technologyList.map(t => {
            if (this.selectedTechnologies.indexOf(t.technologyName) != -1) {
                technologies.push(t);
            }
        });
        return technologies;
    }

    addCustomer() {
        this.customer.technologyList = this.customerTechnology.value ? this.customerTechnology.value : [];
        this.customerService.add(this.customer)
            .subscribe(() => {
                this.router.navigate(["customer"]);
                this.toastr.success('Customer has been add successfully');
            });
    }

    updateCustomer() {
        this.customer.technologyList = this.customerTechnology.value ? this.customerTechnology.value : [];
        this.customerService.update(this.customer)
            .subscribe(() => {
                this.router.navigate(['customer']);
                this.toastr.success('Customer has been updated successfully');
            });
    }
}