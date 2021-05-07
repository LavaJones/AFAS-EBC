import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';


import { ReportingAppRoutingModule } from './reporting.app-routing.module';

import { SharedModule } from 'app/ClientSide/shared.module';

import { IRSVerificationComponent } from './StatusPortal/IRSVerification.component';
import { IRSConfirmationComponent } from './StatusPortal/IRSConfirmation.component';
import { SuccessMessageComponent, ErrorMessageComponent } from './StatusPortal/IRSConfirmation.component';

import {
    Employee1095detailsPart2Service,
    Part2EditComponent,
    ResolvePart2Conflict,
    Employee1095detailsPart3Service,
    Part3EditComponent,
    Employee1095summaryService,
    Edit1095DetailsComponent,
    View1095sComponent,
    LegacyView1095sComponent,
    View1094Component,
    Employer1094summaryService,
} from './Reporting1095/reporting1095';

import {
    Part2Component,
    Part3Component,
    Details1095Component,
    Unapprove1095sComponent,
    LegacyUnapprove1095sComponent
} from './UnApproval/unapprove1095';

import { PrintService } from 'app/Services/print.service';

@NgModule({
    imports: [
        CommonModule,
        SharedModule,
        ReportingAppRoutingModule
    ],
    declarations: [
        Edit1095DetailsComponent,
        Part2EditComponent,
        ResolvePart2Conflict,
        Part3EditComponent,
        View1095sComponent,
        LegacyView1095sComponent,
        Part2Component,
        Part3Component,
        Details1095Component,
        Unapprove1095sComponent,
        LegacyUnapprove1095sComponent,
        IRSVerificationComponent,
        IRSConfirmationComponent,
        SuccessMessageComponent,
        ErrorMessageComponent,
        View1094Component,
    ]
    , providers: [
        Employee1095summaryService,
        Employee1095detailsPart2Service,
        Employee1095detailsPart3Service,
        Employer1094summaryService,
        PrintService
    ],
    entryComponents: [
        SuccessMessageComponent,
        ErrorMessageComponent
    ]
})

export class ReportingModule { }
