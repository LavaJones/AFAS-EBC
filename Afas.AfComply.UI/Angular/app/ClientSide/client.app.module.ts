
// Libraries
import { NgModule } from '@angular/core';
import { HttpModule, Http, RequestOptions, XHRBackend } from '@angular/http';
import { AppRequestOptions } from 'app/Base/base-request-options';
import { HttpService } from 'app/Base/base-http.service';
// Standard Files
import { ClientAppRoutingModule } from './client.app-routing.module';
import { ClientAppComponent } from './client.app.component';
// sub modules
import { ReportingModule } from './Reporting/reporting.app.module';
import { SharedModule } from './shared.module';
import { FileCabinetModule } from './FileCabinet/filecabinet.app.module';

@NgModule({
    imports: [
        HttpModule,
        ClientAppRoutingModule,
        SharedModule,
        ReportingModule,
        FileCabinetModule
    ],
    declarations: [
        ClientAppComponent,
    ],
    providers: [
        { provide: RequestOptions, useClass: AppRequestOptions },
        { provide: Http, useClass: HttpService }],
    bootstrap: [ClientAppComponent]
})

export class ClientAppModule { }
