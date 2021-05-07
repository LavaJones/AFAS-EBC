import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';
import { EmployerIdSelect } from "app/Models/employerIdSelect";
import { EmployerService } from 'app/Services/employer.service';
import { PrintService } from 'app/Services/print.service';

@Component({
 
    selector: 'admin-print-view',
    templateUrl: './print.component.html',
    providers: [EmployerService, PrintService]
})

export class PrintComponent implements OnInit
{
    encryptedId: string;

    title = 'Print';
    employers: EmployerIdSelect[];
    selectedEmployer = null;

    constructor(private employerService: EmployerService, private printService: PrintService, private location: Location)
    {
        
    }

    ngOnInit(): void
    {

        this.refreshEmployers();

    }

    refreshEmployers(): void {

        this.selectedEmployer = null;

        this.employerService.GetAllEmployerIdSelect().then(employers => {

            this.employers = employers;
            
        });

    }

    printClick(): void {

        this.printService.printYear(this.selectedEmployer).then(nothing => { });

    }

    reprintClick(): void {

        this.printService.reprintYear(this.selectedEmployer).then(nothing => { });

    }

    adminPrintClick(): void {

        this.printService.adminPrintYear( this.selectedEmployer).then(nothing => { });

    }

    adminReprintClick(): void {

        this.printService.adminReprintYear( this.selectedEmployer).then(nothing => { });

    }

    pdfPrintClick(): void {

        this.printService.pdfPrintYear( this.selectedEmployer).then(nothing => { });

    }

    print1094Click(): void {

        this.printService.print1094( this.selectedEmployer).then(nothing => { });

    }

}
