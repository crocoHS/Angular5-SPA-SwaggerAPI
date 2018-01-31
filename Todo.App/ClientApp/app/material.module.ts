import { NgModule } from '@angular/core';
import { MatTableModule } from '@angular/material/table';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatInputModule,
    MatButtonModule,
    MatCheckboxModule,
    MatSelectModule,
    MatSlideToggleModule,
    MatPaginatorModule,
    MatProgressBarModule
} from '@angular/material';

@NgModule({
    imports: [
        MatInputModule,
        MatButtonModule,
        MatCheckboxModule,
        MatSelectModule,
        MatSlideToggleModule,
        MatSnackBarModule,
        MatTableModule,
        MatPaginatorModule,
        MatProgressBarModule,
        FormsModule,
        ReactiveFormsModule],
    exports: [
        MatInputModule,
        MatButtonModule,
        MatCheckboxModule,
        MatSelectModule,
        MatSlideToggleModule,
        MatSnackBarModule,
        MatTableModule,
        MatPaginatorModule,
        MatProgressBarModule,
        FormsModule,
        ReactiveFormsModule]
})

export class MaterialModule { }