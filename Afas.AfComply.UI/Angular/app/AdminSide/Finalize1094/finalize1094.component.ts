import { ActivatedRoute } from '@angular/router';
import { Component, OnInit, ElementRef, ViewChild, ViewChildren, QueryList, HostListener } from '@angular/core';
import { FormControl, NgForm } from '@angular/forms';
import { Location } from '@angular/common';
import { MatSort, MatPaginator, MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { DataSource } from '@angular/cdk/collections';
import { BehaviorSubject } from 'rxjs/internal/BehaviorSubject';
import { Observable } from 'rxjs/internal/Observable';
import { Dictionary } from 'app/Base/dictionary';
import { BlockUI, NgBlockUI } from 'ng-block-ui';
import { Employer1094summary } from "app/Models/employer1094summary";
import { Employer1094detailsPart3 } from "app/Models/employer1094detailsPart3";
import { Employer1094detailsPart4 } from "app/Models/employer1094detailsPart4";
import { Employer1094summaryService } from 'app/Services/employer1094summary.service';
import { Employer } from "app/Models/employer";
import { EmployerService } from 'app/Services/employer.service';
import { EmployerIdSelect } from "app/Models/employerIdSelect";

@Component({
 
    selector: 'admin-Finalize1094',
    templateUrl: './finalize1094.component.html',
    providers: [Employer1094summaryService, EmployerService]
})

export class Finalize1094Component implements OnInit {
    EmployerId: Number;

    employers: EmployerIdSelect[];
    selectedEmployer = null;
    Employer: EmployerIdSelect=null

    constructor(private employee1094summaryService: Employer1094summaryService, private employerService: EmployerService, public dialog: MatDialog) {

    }
    onEmployerSelect(newValue): void {
        this.selectedEmployer = newValue;
        this.Employer = this.employers.find(e => e.EncryptedId === newValue);;
    }

    ngOnInit(): void {

        this.refreshEmployers();

    }

    refreshEmployers(): void {

        this.selectedEmployer = null;

        this.employerService.GetAll1095FinzlizedEmployerIdSelect().then(employers => {

            this.employers = employers;

        });

    }


    Finalize(): void {


        this.employee1094summaryService.Adminfinalize1094(this.selectedEmployer).then(nothing => { });

        this.employee1094summaryService.Confirm(this.selectedEmployer).then(nothing => { });
        window.location.reload();
    }


}