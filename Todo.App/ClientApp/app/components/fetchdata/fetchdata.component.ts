import { Component, OnInit, ViewChild, Inject } from "@angular/core";
import { IAppConfig } from '../../models/app-config.type';
import { APP_CONFIG } from '../../components/app/app.config';
import { GlobalService } from './../../services/global.service';
import { Consumer } from './../../models/consumer.type';
import { UserData } from "../../models/user-data.type";
import { WeatherForecast } from './../../models/weather-forecast.type';
import { NgbTabChangeEvent } from "@ng-bootstrap/ng-bootstrap";
import { MatPaginator, MatSort, MatTableDataSource, Sort } from '@angular/material';

@Component({
    selector: "fetchdata",
    templateUrl: "./fetchdata.component.html",
    styleUrls: ['./fetchdata.component.css']
})

export class FetchDataComponent implements OnInit {
    @ViewChild(MatPaginator) paginator: MatPaginator;
    @ViewChild(MatSort) sort: MatSort;

    displayedColumns = ['id', 'name', 'dob', 'salary'];
    displayedColumn = ['id', 'name', 'progress', 'color'];

    dataSourcer: MatTableDataSource<UserData>;
    dataSource: MatTableDataSource<Consumer>;
    forecasts: WeatherForecast[];

    constructor(
        private service: GlobalService,
        @Inject(APP_CONFIG) public config: IAppConfig) { }

    ngOnInit(): void {
        if (window == undefined) return; // avoid browser crash on page refresh
        this.getConsumersAsync();
    }

    beforeChange($event: NgbTabChangeEvent) {
        if ($event.nextId === 'tab-consumers' && !this.dataSource) {
            this.getConsumersAsync();
        }
        if ($event.nextId === 'tab-users' && !this.dataSourcer) {
            this.getRandomUsers();
        }
        else if ($event.nextId === 'tab-forecasts' && !this.forecasts) {
            this.getWeatherForecasts();
        }
    };

    applyFilter_consumers(filterValue: string) {
        filterValue = filterValue.trim(); // Remove whitespace
        filterValue = filterValue.toLowerCase(); // MatTableDataSource defaults to lowercase matches
        this.dataSource.filter = filterValue;
    }

    applyFilter_users(filterValue: string) {
        filterValue = filterValue.trim(); // Remove whitespace
        filterValue = filterValue.toLowerCase(); // Datasource defaults to lowercase matches
        this.dataSourcer.filter = filterValue;
    }

    getConsumersAsync() {
        this.service.getConsumersAsync()
            .subscribe(consumers => {
                consumers.map(c => c.dob = this.service.getRandomDate(new Date(1976, 0, 1), new Date()));
                consumers.map(c => c.salary = c.id * Math.random() * 1000);

                this.dataSource = new MatTableDataSource(consumers);
                this.dataSource.sort = this.sort;
                this.dataSource.paginator = this.paginator;
            });
    }

    getRandomUsers() {
        const users: UserData[] = [];
        for (let i = 1; i <= 100; i++) { users.push(this.service.createNewUser(i)); }

        this.dataSourcer = new MatTableDataSource(users);
        this.dataSourcer.sort = this.sort;
        this.dataSourcer.paginator = this.paginator;
    }

    getWeatherForecasts() {
        this.service.getWeatherForecasts()
            .subscribe(forecasts => {
                this.forecasts = forecasts.filter(v => v.temperatureF > 50);
            });
    }
}
