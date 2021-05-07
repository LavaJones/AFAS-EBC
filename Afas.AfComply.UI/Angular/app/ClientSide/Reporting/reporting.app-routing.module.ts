
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { IRSVerificationComponent } from './StatusPortal/IRSVerification.component';
import { IRSConfirmationComponent } from './StatusPortal/IRSConfirmation.component';
import { View1095sComponent, View1094Component, LegacyView1095sComponent } from './Reporting1095/reporting1095';
import { Unapprove1095sComponent, LegacyUnapprove1095sComponent } from './UnApproval/unapprove1095';

const routes: Routes = [
    {
        path: '', redirectTo: '/Verification', pathMatch: 'full'
    }
    ,
    {
        path: 'Verification',
        component: IRSVerificationComponent
    }
    ,
    {
        path: 'Confirmation',
        component: IRSConfirmationComponent
    },
    {
        path: 'View1095',
        component: View1095sComponent
    },
    {
        path: 'LegacyView1095',
        component: LegacyView1095sComponent
    },
    {
        path: 'View1095/:encryptedId',
        component: View1095sComponent
    },
    {
        path: 'View1094',
        component: View1094Component
    },
    {
        path: 'Unfinalize1095',
        component: Unapprove1095sComponent
    },
    {
        path: 'LegacyUnfinalize1095',
        component: LegacyUnapprove1095sComponent
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})

export class ReportingAppRoutingModule { }
