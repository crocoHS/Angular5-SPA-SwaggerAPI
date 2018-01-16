import { Component, OnInit, ViewChild } from '@angular/core';
import { ToastContainerDirective, ToastrService } from 'ngx-toastr';
import { AuthService } from '../../services/auth.service';

@Component({
    selector: 'app',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
    @ViewChild(ToastContainerDirective) toastContainer: ToastContainerDirective;
    constructor(
        private toastrService: ToastrService,
        private authService: AuthService) {
        authService.handleAuthentication()
    }

    ngOnInit() {
        this.toastrService.overlayContainer = this.toastContainer;
    }
}
