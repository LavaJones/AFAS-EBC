import { Component, OnInit, ElementRef, ViewChild, ViewChildren, QueryList, HostListener } from '@angular/core';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { ActivatedRoute } from '@angular/router';
import { FormControl, NgForm } from '@angular/forms';
import { Location } from '@angular/common';
import { MatSort, MatPaginator, MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { DataSource } from '@angular/cdk/collections';
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
import { Employee1095summary } from "app/Models/employee1095summary";
import { Employee1095summaryService } from 'app/Services/employee1095summary.service';
import { UIMessage } from './ui-message.component';
import { PrintService } from 'app/Services/print.service';
import { StateAbrev } from 'app/Models/Enums/state_abrev';

import { environment } from 'environments/environment';

@Component({

    selector: 'view-1095',
    templateUrl: './view1095.component.html',
    styleUrls: ['./view1095.component.css'
    ],
    providers: [Employee1095summaryService],
    animations: [
        trigger('detailExpand', [
            state('collapsed', style({ height: '0px', minHeight: '0', visibility: 'hidden' })),
            state('expanded', style({ height: '*', visibility: 'visible' })),
            transition('expanded <=> collapsed', animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)')),
        ]),
    ],
})

export class View1095sComponent implements OnInit {
    encryptedId: string;
    title = 'Employer 1095';
    displayedColumns = ['Reviewed', 'Receiving1095', 'FirstName', 'MiddleName', 'LastName', 'SsnHidden', 'Address', 'City', 'State', 'Zip', 'HireDate', 'TermDate'];
    EmployeeDataSource: ExtendedMatTableDataSource<Employee1095summary>;

    @BlockUI('list-1095') blockUIList: NgBlockUI;

    tablelength: number;

    TotalCount: number;
    Reciving1095Count: number;
    ReviewedCount: number;
    ReviewedPercent: number;

    employees: Employee1095summary[];
    currentPage: Employee1095summary[];
    TaxYears: Dictionary<string>;
    selectedTaxYear: string;
    receiving1095Filter: string = "True";
    expandedElement: any;

    Feature = environment.Feature;
    Branding = environment.Branding;
   

    isExpansionDetailRow = row => row.hasOwnProperty('isDetails');

    @ViewChild(MatSort) sort: MatSort;
    @ViewChild(MatPaginator) paginator: MatPaginator;

    hasPrinted: boolean;
    canPrint(): boolean {
        if (this.TaxYears != undefined && this.selectedTaxYear == this.TaxYears[this.Feature.CurrentReportingYear].toString()) {
            return this.Reciving1095Count <= 0 && false == this.hasPrinted;

        }
        else {

            return false;
        }
    }

    canPdfPrint(): boolean {

        if (this.TaxYears != undefined && this.selectedTaxYear == this.TaxYears[this.Feature.CurrentReportingYear].toString()) {
            return this.Reciving1095Count <= 0 && this.hasPrinted;
        }
        else {

            return false;
        }
    }


    constructor(private employee1095summaryService: Employee1095summaryService, private printService: PrintService, private location: Location, route: ActivatedRoute, public dialog: MatDialog) {

        this.encryptedId = route.snapshot.params['encryptedId'];
        this.tablelength = 0;

    }

    ngOnInit(): void {

        this.refreshTaxYears();

    }

    applyFilterReceiving(filterValue: any) {

        if (this.EmployeeDataSource) {
            this.EmployeeDataSource.filterReceiving = filterValue.trim().toLowerCase();

            if (this.EmployeeDataSource.paginator) {
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

    getProto(row: Employee1095summary): Employee1095summary {
        if (this.isExpansionDetailRow(row)) {
            return Object.getPrototypeOf(row);
        }
        return row;
    }

    toggleRow(row: Employee1095summary): void {
        row = this.getProto(row);
        row.isExpanded = false == row.isExpanded;

    }

    refreshSummary(value: any): void {
    }

    GetStateAbrev(id: number): string {
        return StateAbrev[id];
    }

    ExportFile(): void {

        const url = `Employee1095summaryApi/GetFileExport/${this.selectedTaxYear}`;

        window.open(url);

    }

    refreshTaxYears(): void {

        this.employee1095summaryService.getTaxYears().then(years => {

            this.TaxYears = years;
            let temp: Dictionary<string>;
            
            let lastYear = this.Feature.CurrentReportingYear.toString();   

            Object.keys(this.TaxYears).forEach(key => {
                if (Number.parseInt(key) == Number.parseInt(lastYear)) {
                    this.selectedTaxYear = this.TaxYears[key].toString();

                    this.refreshList(this.TaxYears[key]);
                }
                else {
                    delete this.TaxYears[key];
                }
            });
        });

    }

    refreshList(encryptedValue: string): void {

        this.blockUIList.start('Loading...');    

        this.printService.hasPrintedYear(this.selectedTaxYear).then(has => {
            this.hasPrinted = has;
        });

        this.employee1095summaryService.getSome(encryptedValue)            
            .then(
                all => {
                this.rebuildGrid(all);
            })
            .catch(() =>
            {
                console.log("Caught issue with employee1095summaryService.getSome");

                this.blockUIList.stop();   

                alert("Failed to Load Forms, please Refresh.");

            })
            ;

    }

    rebuildGrid(all: Employee1095summary[]): void {

        all.forEach((employee) => {
            employee.EmployeeMonthlyDetails.forEach((item) => {
                if (item.Line14 == null && item.Line15 == null && item.Line16 == null)
                {
                    item.Receiving1095C = null;
                }
            });
        });

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

        this.applyFilterReceiving(this.receiving1095Filter);

        this.blockUIList.stop();   

    }

    printClick(): void {
        if (this.canPrint()) {
            this.blockUIList.start('Loading...');    

            this.printService.printYear(this.selectedTaxYear).then(nothing => { window.location.reload(); });
        }
    }

    pdfPrintClick(): void {
        if (this.canPdfPrint()) {
            this.blockUIList.start('Loading...');    

            this.printService.pdfPrintYear(this.selectedTaxYear).then(nothing => { window.location.reload(); });
        }
        alert("Your forms have been sent to print, and your PDFs are being generated. Please check again in 2-3 Business days.");
        return;

    }
    
    ReviewedPage(): void {

        this.blockUIList.start('Saving...');    

        var page = this.EmployeeDataSource.display_data;

        var promises: Promise<any>[] = [];
        for (var i = 0; i < page.length; i++)
        {

            if (false == page[i].Reviewed)
            {
                page[i].Reviewed = true;


                promises.push(this.employee1095summaryService.Review1095(page[i]));


            }
        }
        

        
        Promise.all(promises)
            .catch(() => {
                console.log("One or more items may have failed to be reviewed.");

                this.blockUIList.stop();   
                

                alert("Failed to Review Page.");

            })
            .then(() => {

                this.blockUIList.stop();   

                this.refreshList(this.selectedTaxYear);


            });

    }

    UnReviewedPage(): void {

        this.blockUIList.start('Saving...');    
        var page = this.EmployeeDataSource.display_data;
        var promises: Promise<any>[] = [];
        for (var i = 0; i < page.length; i++) {
            if (true == page[i].Reviewed) {
                page[i].Reviewed = false;
                promises.push(this.employee1095summaryService.Review1095(page[i]));
            }
        }
        Promise.all(promises)
            .catch(() => {
                console.log("One or more items may have failed to be Unreviewed.");



            })
            .then(() => {

                this.refreshList(this.selectedTaxYear);

            });




    }

    RebuildCounters(): void {


        this.TotalCount = this.employees.length;
        this.Reciving1095Count = 0;
        this.ReviewedCount = 0;

        for (var i = 0; i < this.employees.length; i++) {

            this.employees[i].isExpanded = (false == this.employees[i].Reviewed);

            if (this.employees[i].Receiving1095) {
                this.Reciving1095Count++;

                if (this.employees[i].Reviewed) {
                    this.ReviewedCount++;
                }
            }
        }

        this.ReviewedPercent = (this.ReviewedCount / this.Reciving1095Count);

    }


    Reviewed(row: Employee1095summary): void {

        row.Reviewed = true;
        this.employee1095summaryService.Review1095(row).then(all => { });


        this.RebuildCounters();

    }

    UnReviewed(row: Employee1095summary): void {

        row.Reviewed = false;
        this.employee1095summaryService.Review1095(row).then(all => { });

        this.RebuildCounters();

    }

    Finalize1095(): void {
        this.blockUIList.start('Loading...');    

        this.employee1095summaryService.finalize1095(this.employees[0])
            .then(all =>
            {
                this.refreshList(this.selectedTaxYear);

                let dialogRef = this.dialog.open(UIMessage, {
                    width: '500px',
                    data: { message: all }
                });

                dialogRef.afterClosed().subscribe(result => {
                    console.log('The dialog was closed');
                });

            }).catch(() => {
                console.log("Caught issue with employee1095summaryService.finalize1095");

                this.blockUIList.stop();   

                alert("Failed to Finalize Employees.");

            });;

    }

    dragAreaClass: string = 'dragarea';

    saveFiles(files) {
        if (files.length > 0 && (false == this.isValidFiles(files))) {
            alert("Invalid File, Please Upload a Valid CSV file");
            return;
        }

        this.blockUIList.start('Loading...');     

        this.employee1095summaryService.uploadEditFile(files[0], this.selectedTaxYear)
            .then(all =>
            {
                console.log("Sucessfully Uploaded edits file.", all);

                this.refreshList(this.selectedTaxYear);
            })
            .catch(() => {
                console.log("Caught issue with employee1095summaryService.uploadEditFile");

                this.blockUIList.stop();   

                alert("Failed to Upload File.");

            });

    }

    private isValidFiles(files): boolean {
        if (files.length != 1) {
            console.log("wronge number of files: " + files.length);
            return false;
        }
        var ext = files[0].name.toUpperCase().split('.').pop() || files[0].name;

        if (ext.toUpperCase().trim() != 'csv'.toUpperCase()) {
            console.log("file is not a csv: " + ext.trim());
            return false;
        }

        if (files[0].size <= 0) {
            console.log(" file is empty: " + files[0].size);
            return false;
        }

        return true;
    }

    onFileChange(event) {
        console.log('file changed');
        let files = event.target.files;
        this.saveFiles(files);
    }

    @HostListener('dragover', ['$event']) onDragOver(event) {
        this.dragAreaClass = "droparea";
        event.preventDefault();
    }

    @HostListener('dragenter', ['$event']) onDragEnter(event) {
        this.dragAreaClass = "droparea";
        event.preventDefault();
    }

    @HostListener('dragend', ['$event']) onDragEnd(event) {
        this.dragAreaClass = "dragarea";
        event.preventDefault();
    }

    @HostListener('dragleave', ['$event']) onDragLeave(event) {
        this.dragAreaClass = "dragarea";
        event.preventDefault();
    }

    @HostListener('drop', ['$event']) onDrop(event) {
        this.dragAreaClass = "dragarea";
        event.preventDefault();
        event.stopPropagation();
        var files = event.dataTransfer.files;
        this.saveFiles(files);
    }

}
