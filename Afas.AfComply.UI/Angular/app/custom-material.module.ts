import { NgModule } from '@angular/core';
import { MatSelectModule } from '@angular/material/select';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatDialogModule, MatTableModule, MatSortModule, MatPaginatorModule, MatButtonModule, MatCheckboxModule, MatFormFieldModule, MatInputModule } from '@angular/material';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BlockUIModule } from 'ng-block-ui';
import { AlertService, AuthenticationService } from './Base/base';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatTreeModule } from '@angular/material/tree';
import { MatIconModule } from '@angular/material/icon';



@NgModule({
    imports:
        [
            MatSelectModule,
            MatDialogModule,
            BlockUIModule.forRoot(),
            MatSortModule,
            MatTableModule,
            MatButtonModule,
            MatCheckboxModule,
            MatPaginatorModule,
            FormsModule,
            ReactiveFormsModule,
            MatFormFieldModule,
            MatInputModule,
            MatExpansionModule,
            MatProgressBarModule,
            MatTooltipModule,
            MatTreeModule,
            MatIconModule
          
        ],
    providers:
        [
            AlertService,
            AuthenticationService,
        ],
    exports:
        [
            MatSelectModule,
            MatDialogModule,
            BlockUIModule,
            MatSortModule,
            MatTableModule,
            MatButtonModule,
            MatCheckboxModule,
            MatPaginatorModule,
            FormsModule,
            ReactiveFormsModule,
            MatFormFieldModule,
            MatInputModule,
            MatExpansionModule,
            MatProgressBarModule,
            MatTooltipModule,
            MatTreeModule,
            MatIconModule
           
        ],
})

export class CustomMaterialModule { }