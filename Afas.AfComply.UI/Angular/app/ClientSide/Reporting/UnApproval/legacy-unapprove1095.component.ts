import { Component, OnInit, ElementRef, ViewChild, ViewChildren, QueryList, HostListener } from '@angular/core';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { ActivatedRoute } from '@angular/router';
import { FormControl, NgForm } from '@angular/forms';
import { Location } from '@angular/common';
import { MatSort, MatPaginator, MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { BehaviorSubject, Observable } from 'rxjs';
import { Dictionary } from 'app/Base/dictionary';
import { BlockUI, NgBlockUI } from 'ng-block-ui';
import 'rxjs/add/operator/startWith';
import 'rxjs/add/observable/merge';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/debounceTime';
import 'rxjs/add/operator/distinctUntilChanged';
import 'rxjs/add/observable/fromEvent';

import { ExtendedMatTableDataSource } from "app/Models/DataSources/ExtendedDatasource";
import { Approved1095summary } from "app/Models/approved1095summary";
import { Approved1095FinalService } from 'app/Services/approved1095final.service';
import { PrintService } from 'app/Services/print.service';
import { StateAbrev } from 'app/Models/Enums/state_abrev';


@Component({

    selector: 'unapprove-1095',
    templateUrl: './legacy-unapprove1095.component.html',
    styleUrls: ['./legacy-unapprove1095.component.css'
    ],
    providers: [Approved1095FinalService],
    animations: [
        trigger('detailExpand', [
            state('collapsed', style({ height: '0px', minHeight: '0', visibility: 'hidden' })),
            state('expanded', style({ height: '*', visibility: 'visible' })),
            transition('expanded <=> collapsed', animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)')),
        ]),
    ],
})

export class LegacyUnapprove1095sComponent implements OnInit {
    encryptedId: string;
    title = 'UnFinalize 1095';
    displayedColumns = ['Unfinalize', 'Receiving1095', 'FirstName', 'MiddleName', 'LastName', 'SsnHidden', 'Address', 'City', 'State', 'Zip'];
    EmployeeDataSource: ExtendedMatTableDataSource<Approved1095summary>;

    @BlockUI('list-1095') blockUIList: NgBlockUI;

    tablelength: number;

    TotalCount: number;
    Reciving1095Count: number;
    UnPrintedCount: number;
    PercentPrinted: number;

    employees: Approved1095summary[];
    currentPage: Approved1095summary[];
    TaxYears: Dictionary<string>;
    selectedTaxYear: string;

    expandedElement: any;

    isExpansionDetailRow = row => row.hasOwnProperty('isDetails');

    @ViewChild(MatSort) sort: MatSort;

    @ViewChild(MatPaginator) paginator: MatPaginator;

    constructor(private approve1095Service: Approved1095FinalService, private printService: PrintService, private location: Location, route: ActivatedRoute, public dialog: MatDialog) {

        this.encryptedId = route.snapshot.params['encryptedId'];
        this.tablelength = 0;

    }

    ngOnInit(): void {

        this.refreshTaxYears();

    }

    GetStateAbrev(id: number): string {
        return StateAbrev[id];
    }

    applyFilterReceiving(filterValue: string) {
        if (this.EmployeeDataSource)
        {
            this.EmployeeDataSource.filterReceiving = filterValue.trim().toLowerCase();

            if (this.EmployeeDataSource.paginator)
            {
                this.EmployeeDataSource.paginator.firstPage();
            }
        }
    }

    applyFilterClassification(filterValue: string) {
        if (this.EmployeeDataSource) {
            this.EmployeeDataSource.filterClassification = filterValue.trim().toLowerCase();

            if (this.EmployeeDataSource.paginator) {
                this.EmployeeDataSource.paginator.firstPage();
            }
        }
    }

    applyFilterNameSSN(filterValue: string) {
        if (this.EmployeeDataSource) {
            this.EmployeeDataSource.filterNameSSN = filterValue.trim().toLowerCase();

            if (this.EmployeeDataSource.paginator) {
                this.EmployeeDataSource.paginator.firstPage();
            }
        }
    }


    ExportFile(): void {

        const url = `Approved1095FinalApi/GetPart2CsvReport/${this.selectedTaxYear}`;

        window.open(url);

    }

    getProto(row: Approved1095summary): Approved1095summary {
        if (this.isExpansionDetailRow(row)) {
            return Object.getPrototypeOf(row);
        }
        return row;
    }

    toggleRow(row: Approved1095summary): void {
        row = this.getProto(row);
        row.isExpanded = false == row.isExpanded;

    }

    refreshSummary(value: any): void {
        this.employees[this.employees.indexOf(value.old)] = value.new;

    }

    refreshTaxYears(): void {

        this.approve1095Service.getTaxYears().then(years => {

            this.TaxYears = years;
            let lastYear = (new Date()).getFullYear() - 1;

            Object.keys(this.TaxYears).forEach(key => {
                if (Number.parseInt(key) == lastYear) {
                    this.selectedTaxYear = this.TaxYears[key].toString();

                    this.refreshList(this.TaxYears[key]);
                }
                else if (Object.keys(this.TaxYears).length == 1) {
                    this.selectedTaxYear = this.TaxYears[key].toString();
                    this.refreshList(this.TaxYears[key]);
                }
            });
        });
    }

    refreshList(encryptedValue: string): void {
        this.blockUIList.start('Loading...');    

        this.approve1095Service.getSome(encryptedValue).then(all => {
            this.rebuildGrid(all);
        });

    }

    rebuildGrid(all: Approved1095summary[]): void {

        this.employees = all;

        this.RebuildCounters();
        
        console.time('EmployeeDataSource');
        var t0 = performance.now();

        this.EmployeeDataSource = new ExtendedMatTableDataSource(this.employees);

        this.EmployeeDataSource.paginator = this.paginator;
        this.EmployeeDataSource.sort = this.sort;

        var t1 = performance.now();
        console.log("Call to EmployeeDataSource took " + (t1 - t0) + " milliseconds.")
        console.timeEnd('EmployeeDataSource');

        this.blockUIList.stop();   

    }

    RebuildCounters(): void
    {

        this.TotalCount = this.employees.length;
        this.Reciving1095Count = 0;
        this.UnPrintedCount = 0;

        for (var i = 0; i < this.employees.length; i++) {

            this.employees[i].isExpanded = (false == this.employees[i].Printed);

            if (this.employees[i].Receiving1095) {
                this.Reciving1095Count++;

                if (false == this.employees[i].Printed) {
                    this.UnPrintedCount++;
                }
            }
        }

        this.PercentPrinted = (this.UnPrintedCount / this.Reciving1095Count);

    }

    printClick(): void {

        this.blockUIList.start('Loading...');    

        this.printService.printYear( this.selectedTaxYear).then(nothing => { window.location.reload(); });

    }

    reprintClick(): void {

        this.blockUIList.start('Loading...');    

        this.printService.reprintYear( this.selectedTaxYear).then(nothing => { window.location.reload(); });

    }

    pdfPrintClick(): void {

        this.blockUIList.start('Loading...');    

        this.printService.pdfPrintYear( this.selectedTaxYear).then(nothing => { window.location.reload(); });

    }

    UnfinalizePage(): void {

        this.blockUIList.start('Unfinalizing...');    

        var page = this.EmployeeDataSource.display_data;

        var promises: Promise<any>[] = [];
        for (var i = 0; i < page.length; i++) {
            promises.push(this.approve1095Service.Unfinalize1095(page[i]));
        }

        
        Promise.all(promises)
            .catch(() => {
                console.log("One or more items may have failed to be unfinalized.");



            })
            .then(() => {

                this.refreshList(this.selectedTaxYear);

            });

    }

    Unfinalize(row: Approved1095summary): void {

        if (this.employees.indexOf(row) > -1) {
            this.employees.splice(this.employees.indexOf(row), 1);
        }

        this.approve1095Service.Unfinalize1095(row).then(res => { });

        this.rebuildGrid(this.employees);

    }

    UnfinalizeAll(): void {

        this.approve1095Service.UnfinalizeAll1095(this.selectedTaxYear).then(res => {
            this.refreshList(this.selectedTaxYear);
        });

    }

}
