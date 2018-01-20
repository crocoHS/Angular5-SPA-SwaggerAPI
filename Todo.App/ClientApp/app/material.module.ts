import { NgModule } from '@angular/core';
import { MatTableModule } from '@angular/material/table';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatInputModule, MatButtonModule, MatCheckboxModule, MatSlideToggleModule, MatPaginatorModule, MatProgressBarModule } from '@angular/material';

@NgModule({
    imports: [MatInputModule, MatButtonModule, MatCheckboxModule, MatSlideToggleModule, MatSnackBarModule,
        MatTableModule, MatPaginatorModule, MatProgressBarModule],
    exports: [MatInputModule, MatButtonModule, MatCheckboxModule, MatSlideToggleModule, MatSnackBarModule,
        MatTableModule, MatPaginatorModule, MatProgressBarModule]
})
export class MaterialModule { }