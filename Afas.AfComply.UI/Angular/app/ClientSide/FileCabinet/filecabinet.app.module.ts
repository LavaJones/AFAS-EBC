import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { MatTableModule, MatSortModule, MatTreeModule, MatIconModule, MatButtonModule, MatFormFieldModule, MatInputModule, MatPaginatorModule } from '@angular/material';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { MatDialogModule } from '@angular/material';
import { MatSnackBarModule } from '@angular/material/snack-bar';

import { SharedModule } from 'app/ClientSide/shared.module';

import { FileCabinetAppRoutingModule } from './filecabinet.app-routing.module';
import { UploadFileComponent } from './UploadFile.component';
import { DownloadFileComponent } from './DownloadFile.component';
import { FileCabinetAppComponent } from './FileCabinetApp.component';
import { DeleteFileComponent } from '../FileCabinet/DeleteFile.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { DeleteFolderComponent } from '../FileCabinet/DeleteFolder.component';


@NgModule({
    imports: [
        CommonModule,
        MatTreeModule,
        MatIconModule,
        MatButtonModule,
        MatPaginatorModule,
        MatDialogModule,
        MatTableModule,
        MatSortModule,
        SharedModule,
        HttpModule,
        MatFormFieldModule,
        MatInputModule,
        ReactiveFormsModule,
        BrowserAnimationsModule,
        MatSnackBarModule,
        FileCabinetAppRoutingModule
    ],
    declarations: [

        UploadFileComponent,
        DownloadFileComponent,
        DeleteFileComponent,
        DeleteFolderComponent,
        FileCabinetAppComponent



    ]
    , providers: [

    ],
    entryComponents: [
        DeleteFileComponent,
        DeleteFolderComponent,
        UploadFileComponent
    ]
})

export class FileCabinetModule { }
