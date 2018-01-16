import { Component, OnInit } from "@angular/core";
import { GlobalService } from './../../services/global.service';
import { Consumer } from './../../models/consumer.type';
import { WeatherForecast } from './../../models/weather-forecast.type';

@Component({
    selector: "fetchdata",
    templateUrl: "./fetchdata.component.html"
})
export class FetchDataComponent implements OnInit {
    public consumers: Consumer[];
    public forecasts: WeatherForecast[];

    constructor(private service: GlobalService) { }

    ngOnInit(): void {
        this.getConsumersAsync();
        this.getWeatherForecasts();
    }

    getConsumersAsync() {
        this.service.getConsumersAsync()
            .subscribe(consumers => {
                consumers.map(c => c.dob = this.service.getRandomDate(new Date(1976, 0, 1), new Date()));
                consumers.map(c => c.salary = c.id * Math.random() * 1000);
                this.consumers = consumers.slice(0, 5);
            });
    }

    getWeatherForecasts() {
        this.service.getWeatherForecasts()
            .subscribe(forecasts => {
                this.forecasts = forecasts.filter(v => v.temperatureF > 50);
            });
    }
}