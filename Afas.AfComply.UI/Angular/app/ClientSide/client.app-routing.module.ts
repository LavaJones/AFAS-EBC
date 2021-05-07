import { NgModule } from '@angular/core';
import { RouterModule, Routes, PreloadAllModules } from '@angular/router';
import { ReportingModule } from './Reporting/reporting.app.module';
import { IRSVerificationComponent } from './Reporting/StatusPortal/IRSVerification.component';
import { FileCabinetModule } from './FileCabinet/filecabinet.app.module';
const routes: Routes = [
    {
        path: 'Reporting', loadChildren: './Reporting/reporting.app.module#ReportingModule'
    }
    ,
    {
        path: 'FileCabinet', loadChildren: './FileCabinet/filecabinet.app.module#FileCabinetModule'
    }
];

@NgModule({
    imports: [RouterModule.forRoot(routes, { useHash: false })],
    exports: [RouterModule]
})

export class ClientAppRoutingModule { }
