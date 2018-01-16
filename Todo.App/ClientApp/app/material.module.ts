import { NgModule } from '@angular/core';
import { MatButtonModule, MatCheckboxModule, MatSlideToggleModule } from '@angular/material';
import { MatSnackBarModule } from '@angular/material/snack-bar';

@NgModule({
    imports: [MatButtonModule, MatCheckboxModule, MatSlideToggleModule, MatSnackBarModule],
    exports: [MatButtonModule, MatCheckboxModule, MatSlideToggleModule, MatSnackBarModule]
})
export class MaterialModule { }