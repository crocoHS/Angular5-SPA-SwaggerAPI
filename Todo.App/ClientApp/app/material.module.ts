import { NgModule } from '@angular/core';
import { MatTableModule } from '@angular/material/table';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatInputModule, MatButtonModule, MatCheckboxModule, MatSlideToggleModule, MatPaginatorModule } from '@angular/material';

@NgModule({
    imports: [MatInputModule, MatButtonModule, MatCheckboxModule, MatSlideToggleModule, MatSnackBarModule,
        MatTableModule, MatPaginatorModule],
    exports: [MatInputModule, MatButtonModule, MatCheckboxModule, MatSlideToggleModule, MatSnackBarModule,
        MatTableModule, MatPaginatorModule]
})
export class MaterialModule { }