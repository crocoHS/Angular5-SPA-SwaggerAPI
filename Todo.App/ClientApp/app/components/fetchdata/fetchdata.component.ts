import { Component, OnInit, ViewChild, Inject } from "@angular/core";
import { IAppConfig } from '../../models/app-config.type';
import { APP_CONFIG } from '../../components/app/app.config';
import { GlobalService } from './../../services/global.service';
import { Consumer } from './../../models/consumer.type';
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

    displayedColumns = ['id', 'name', 'dob', 'salary', 'progress'];
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
        if ($event.nextId === 'tab-consumers') {
            this.getConsumersAsync();
        }
        else if ($event.nextId === 'tab-forecasts') {
            this.getWeatherForecasts();
        }
    };

    applyFilter(filterValue: string) {
        filterValue = filterValue.trim(); // Remove whitespace
        filterValue = filterValue.toLowerCase(); // MatTableDataSource defaults to lowercase matches
        this.dataSource.filter = filterValue;
    }

    getConsumersAsync() {
        this.service.getConsumersAsync()
            .subscribe(consumers => {
                consumers.map(c => c.dob = this.service.getRandomDate(new Date(1976, 0, 1), new Date()));
                consumers.map(c => c.salary = c.id * Math.random() * 1000);
                consumers.map(c => c.progress = Math.round(Math.random() * 100).toString());
                consumers.map(c => c.color = this.service.getRandomColor());

                for (let i = 11; i <= 100; i++) {
                    consumers.push(this.service.getRandomConsumer(i));
                }

                this.dataSource = new MatTableDataSource(consumers);
                this.dataSource.sort = this.sort;
                this.dataSource.paginator = this.paginator;
            });
    }

    getWeatherForecasts() {
        this.service.getWeatherForecasts()
            .subscribe(forecasts => {
                this.forecasts = forecasts.filter(v => v.temperatureF > 50);
            });
    }
}
