import { ActivatedRoute } from '@angular/router';
import { Component, OnInit, ElementRef, ViewChild, ViewChildren, QueryList, HostListener } from '@angular/core';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { FormControl, NgForm } from '@angular/forms';
import { Location } from '@angular/common';
import { MatSort, MatPaginator, MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { DataSource } from '@angular/cdk/collections';
import { BehaviorSubject, Observable } from 'rxjs';
import { Dictionary } from 'app/Base/dictionary';
import { BlockUI, NgBlockUI } from 'ng-block-ui';
import { Employer1094summary } from "app/Models/employer1094summary";
import { Employer1094detailsPart3 } from "app/Models/employer1094detailsPart3";
import { Employer1094detailsPart4 } from "app/Models/employer1094detailsPart4";
import { Employer1094summaryService } from 'app/Services/employer1094summary.service';
import { UIMessage } from './ui-message.component';
import { environment } from 'environments/environment';


@Component({
 
    selector: 'View-1094',
    templateUrl: './view1094.component.html',
    styleUrls: ['./view1094.component.css'],
    providers: [Employer1094summaryService],
})


export class View1094Component implements OnInit
{
    @BlockUI('list-1094') blockUIList: NgBlockUI;
    view1094: Employer1094summary;
    employerPart3: Employer1094detailsPart3[];
    employerPart4: Employer1094detailsPart4[];
    Part3All12Yes: boolean = false;
    Part3All12No: boolean = false;
    Part3All12AGI: boolean = false;
    constructor(private employee1094summaryService: Employer1094summaryService, public dialog: MatDialog)
    {
       
    }

    ngOnInit(): void
    {
        this.get1094data();
    }

    get1094data(): void
    {
        this.employee1094summaryService.getSome(environment.Feature.CurrentReportingYear).then(all =>
        {
            this.rebuildGrid(all);
        });

    }

    CheckYesAll12(): void
    {
        var x = this.employerPart3.filter(c => c.MinimumEssentialCoverageOfferIndicator == true).length;
        if (x == 12)
        {
            this.Part3All12Yes=true;
        }
        else
        {
            this.Part3All12Yes = false;
        }

    }

    CheckNoAll12(): void
    {
        var x = this.employerPart3.filter(c => c.MinimumEssentialCoverageOfferIndicator == false).length;
        if (x == 12)
        {
            this.Part3All12No = true;
        }
        else
        {
            
            this.Part3All12No= false;
        }

    }

    CheckAGIAll12(): void {
        var AllTrue = this.employerPart3.filter(c => c.AggregatedGroupIndicator == true).length;
        
        if (AllTrue == 12) {
            this.Part3All12AGI = true;
        }

        
    }

    rebuildGrid(all: Employer1094summary[]): void {
        this.view1094 = all[0];
        this.employerPart3 = this.view1094.Employer1094Part3s;
        this.employerPart4 = this.view1094.Employer1094Part4s;
        this.CheckNoAll12();
        this.CheckYesAll12();
        this.CheckAGIAll12();


    }

    Finalize(): void
    {
        this.blockUIList.start('Loading...');    

        this.employee1094summaryService.finalize1094(this.view1094).then(all => {
            let dialogRef = this.dialog.open(UIMessage, {
                width: '500px',
                data: { message: all }
            });

            dialogRef.afterClosed().subscribe(result => {
                console.log('The dialog was closed');
            });

        });
    }
}