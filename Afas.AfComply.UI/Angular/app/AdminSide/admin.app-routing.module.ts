import { NgModule } from '@angular/core';
import { RouterModule, Routes, PreloadAllModules } from '@angular/router';

import { TimeFrameService, TimeFrameComponent, TimeFrameEditComponent } from './TimeFrame/timeFrames';

import { Finalize1094Component, Employer1094summaryService, Employer, EmployerIdSelect } from './Finalize1094/finalize1094';
import { PrintService, PrintComponent } from './Print/print';

const routes: Routes = [
    {
        path: 'Print',
        component: PrintComponent
    },
    {
        path: 'TimeFrame',
        component: TimeFrameComponent
    },
    {
        path: 'TimeFrame/New',
        component: TimeFrameComponent
    },
    {
        path: 'TimeFrame/:encryptedId',
        component: TimeFrameComponent
    },
    {
        path: 'Finalize1094',
        component: Finalize1094Component
    }
];

@NgModule({
    imports: [RouterModule.forRoot(routes, { useHash: false, preloadingStrategy: PreloadAllModules })],
    exports: [RouterModule]
})
export class AdminAppRoutingModule { }