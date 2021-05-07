import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule }   from '@angular/forms';     
import { RouterModule }  from '@angular/router';
import { HttpModule, Http, RequestOptions, XHRBackend }    from '@angular/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FlexLayoutModule } from '@angular/flex-layout';

import { CustomMaterialModule } from '../custom-material.module';
import { AppComponent } from './admin.app.component';
import { AdminAppRoutingModule } from './admin.app-routing.module';
import { AppRequestOptions } from '../Base/base-request-options';
import { HttpService } from '../Base/base-http.service';
import { TimeFrameService, TimeFrameComponent, TimeFrameEditComponent } from './TimeFrame/timeFrames';
import { Finalize1094Component, Employer1094summaryService, Employer, EmployerIdSelect } from './Finalize1094/finalize1094';
import { PrintService, PrintComponent } from './Print/print';



@NgModule({
    imports: [
        BrowserModule,
        BrowserAnimationsModule,
        CustomMaterialModule,
        FormsModule,         
        HttpModule,
        AdminAppRoutingModule,
        FlexLayoutModule
    ],
    declarations: [
        AppComponent,
        PrintComponent,
        TimeFrameComponent,
        TimeFrameEditComponent,
        Finalize1094Component
    ],
    providers: [
        PrintService,
        Employer1094summaryService,
            TimeFrameService,
            { provide: RequestOptions, useClass: AppRequestOptions },
            { provide: Http, useClass: HttpService }],
    bootstrap: [AppComponent]
})

export class AdminAppModule { }
