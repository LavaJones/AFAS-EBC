
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DownloadFileComponent } from './DownloadFile.component';
import { UploadFileComponent } from './UploadFile.component';
import { FileCabinetAppComponent } from './FileCabinetApp.component';
import { DeleteFileComponent } from '../FileCabinet/DeleteFile.component';
import { DeleteFolderComponent } from '../FileCabinet/DeleteFolder.component';

const routes: Routes = [

    {
        path: ' ', component: FileCabinetAppComponent
    },
    {
        path: 'ViewFileCabinet', component: DownloadFileComponent
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})

export class FileCabinetAppRoutingModule { }
