import { Component, Inject, OnInit } from '@angular/core';
import { CustomerService } from '../../services/customer.service';
import { GlobalService } from '../../services/global.service';
import { Technology } from "../../models/technology.type";

@Component({
    selector: 'settings',
    templateUrl: './settings.component.html'
})

export class SettingsComponent implements OnInit {
    public isTechnolgySelected: boolean = false;
    public technology: Technology = new Technology();
    public technologyList: string[];

    constructor(
        private customerService: CustomerService,
        private service: GlobalService) {
    }

    ngOnInit() {
        this.getTechnologyList();
    }

    private getTechnologyList() {
        this.customerService.getTechnologyList()
            .subscribe(technologyList => {
                this.technologyList = technologyList.map(t => t.technologyName);
            });
    }

    public addTechnology() {
        if (this.technology.technologyName.trim() == '') {
            this.service.openSnackBar('Technology cannot be empty!', 'Error');
            return;
        }
        let index = this.technologyList.findIndex(item => (this.technology.technologyName.toLowerCase() === item.toLowerCase()));
        if (index != -1) {
            this.service.openSnackBar(`'${this.technology.technologyName}' has been already added!`, 'Error');
            return;
        }
        this.customerService.addTechnology(this.technology)
            .subscribe(() => {
                this.customerService.technologyList.refresh(); // Refresh technology list cache
                this.technologyList.push(this.technology.technologyName);
                this.service.openSnackBar(`'${this.technology.technologyName}' has been added`, 'Success');
                this.technology = new Technology();
            });
    }

    public deleteTechnology(technologyName: string) {
        if (!technologyName) {
            return;
        }

        if (confirm(`Are you sure you want to delete '${technologyName}' from the list?`)) {
            this.customerService.deleteTechnology(technologyName)
                .subscribe(() => {
                    this.customerService.technologyList.refresh(); // Refresh technology list cache
                    this.isTechnolgySelected = false;
                    this.service.openSnackBar(`'${technologyName}' has been deleted`, 'Success');
                });
        }
    }
}