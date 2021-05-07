import { Component, Input, Output, EventEmitter } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';
import { Employee1095summary } from "app/Models/employee1095summary";
import { Employee1095detailsPart2 } from "app/Models/employee1095detailsPart2";
import { Employee1095summaryService } from 'app/Services/employee1095summary.service';
import { Employee1095detailsPart3 } from 'app/Models/employee1095detailsPart3';
import { BlockUI, NgBlockUI } from 'ng-block-ui';
import { MatDialog } from '@angular/material'
import { DialogContent } from './dialogContent.component';

@Component({
 
    selector: 'edit1095Details',
    templateUrl: './edit1095Details.component.html',
    styleUrls: ['./view1095.component.css'],
    providers: [Employee1095summaryService]
})

export class Edit1095DetailsComponent
{
    WarningMessage: string = "";
    @BlockUI('edit-1095') blockUI: NgBlockUI;

    @Input() employee: Employee1095summary;
    @Output() summaryUpdated = new EventEmitter();
    public listOfLists: Array<Employee1095detailsPart2[]> = [];
    validationMessage: string;
    newRecord: boolean = false;
    index: number = 0;
    newEmployeeRecord: Employee1095detailsPart3 = new Employee1095detailsPart3();
    public stateOptions: any[] = [{ key: "1", value: "MN" }];
    CancelEditEmployeeMonthlyDetails: Array<Employee1095detailsPart2[]> = [];
    constructor(private employee1095summaryService: Employee1095summaryService, public dialog:MatDialog)
    {

    }
    openDialog() {
        const dialogRef = this.dialog.open(DialogContent, { height: '350px' });
    }
    ngOnInit(): void
    {

        for (var i = 0; i <= 12; i++)
        {
            this.listOfLists.push(this.employee.EmployeeMonthlyDetails.filter((item) => item.MonthId == i));
        }
        this.CancelEditEmployeeMonthlyDetails = JSON.parse(JSON.stringify(this.listOfLists));
    }

    Reload(): void
    {
        this.employee1095summaryService
            .getThis(this.employee)
            .then(
            (result) => {
                this.summaryUpdated.emit({ 'old': this.employee, 'new': result });
                this.employee = result;

                this.listOfLists = [];

                this.employee.EmployeeMonthlyDetails.forEach((item) =>
                {
                    if (item.Line14 == null && item.Line15 == null && item.Line16 == null)
                    {
                        item.Receiving1095C = null;
                    }
                });

                for (var i = 0; i <= 12; i++)
                {
                    this.listOfLists.push(this.employee.EmployeeMonthlyDetails.filter((item) => item.MonthId == i));
                }

            });
        this.listOfLists = this.CancelEditEmployeeMonthlyDetails;

    }

    SaveEdits(): void
    {
       


        if (this.employee.unsavedChanges) {
            let all12edits = this.employee.EmployeeMonthlyDetails.filter((item) => item.MonthId == 0);

            let part2edits = this.employee.EmployeeMonthlyDetails.filter((item) => item.MonthId > 0);
            if (part2edits && part2edits.length > 0)
            {

                if (all12edits && all12edits.length > 0)
                {
                    let all12edit = all12edits[0];
                    if (all12edit.Line14 != null && all12edit.Line14 != "")
                    {
                        for (let i = 0; i < part2edits.length; i++)
                        {
                            part2edits[i].Line14 = all12edit.Line14;
                        }
                    }
                    if (all12edit.Line15 != null && all12edit.Line15 != "")
                    {
                        for (let i = 0; i < part2edits.length; i++)
                        {
                            part2edits[i].Line15 = all12edit.Line15;
                        }
                    }
                    if(all12edit.Line16 != null && all12edit.Line16 != "")
                    {
                        for (let i = 0; i < part2edits.length; i++)
                        {
                            part2edits[i].Line16 = all12edit.Line16;
                        }
                    }
                    if (all12edit.Receiving1095C != null && all12edit.Receiving1095C)
                    {
                        for (let i = 0; i < part2edits.length; i++) {
                            part2edits[i].Receiving1095C = all12edit.Receiving1095C;
                        }
                    }
                }
            
                this.employee1095summaryService.
                    updateMultiplePart2(this.employee, part2edits)
                    .then(
                    (result) =>
                    {
                        this.employee.EmployeeMonthlyDetails = result;

                        this.listOfLists = [];

                        for (var i = 0; i <= 12; i++) {
                            this.listOfLists.push(this.employee.EmployeeMonthlyDetails.filter((item) => item.MonthId == i));
                        }

                        this.employee1095summaryService
                            .updatePart1(this.employee)
                            .then(
                            (result) => {

                                this.summaryUpdated.emit({ 'old': this.employee, 'new': result });
                                this.employee = result;

                                this.listOfLists = [];

                                for (var i = 0; i <= 12; i++) {
                                    this.listOfLists.push(this.employee.EmployeeMonthlyDetails.filter((item) => item.MonthId == i));
                                }

                            });

                    });
            }
        }
    }

    editedBackground(): any
    {
        if (this.employee.unsavedChanges)
        {
            return { 'background-color': 'white' };
        }
        return { 'background-color': 'white' };
    }
    editedButton(): any
    {
        if (this.employee.unsavedChanges)
        {
            return { 'background-color': '#eb0029', 'border': '3px solid #f44336' };
        }
        return { 'background-color': 'green', 'border': '3px solid #007C3D' };
    }


    AddNew() {
        this.newRecord = true;
        this.newEmployeeRecord = new Employee1095detailsPart3();
        this.newEmployeeRecord.Enrolled = new Array<boolean>(12);
        for (var month = 0; month < this.newEmployeeRecord.Enrolled.length; month++) {
            this.newEmployeeRecord.Enrolled[month] = false;
        }
        if (this.employee.CoveredIndividuals.length == 0) {
            this.newEmployeeRecord.FirstName = this.employee.FirstName;
            this.newEmployeeRecord.MiddleName = this.employee.MiddleName;
            this.newEmployeeRecord.LastName = this.employee.LastName;
            this.newEmployeeRecord.Ssn = this.employee.SsnHidden;
            this.newEmployeeRecord.Dob = this.employee.HireDate;

        }
    }

    CancelAddNew() {
        this.newRecord = false;
        this.WarningMessage = "";
        this.validationMessage = "";

    }

    updateChecked(option, event) {
        this.newEmployeeRecord.Enrolled[option] = event.target.checked;          
    }
    updateCheckedOptions(option, event) {
        this.newEmployeeRecord.Enrolled[option] = event.target.checked;
    }

    saveRecord() {
        this.WarningMessage = ""
        if (this.ValidateField() === "Valid") {
            this.employee1095summaryService.SaveNewInsuranceCoverage(this.newEmployeeRecord, this.employee).then(all =>
                {
                    this.newEmployeeRecord = all;
                    this.employee.CoveredIndividuals.push(this.newEmployeeRecord);
                });
            this.newRecord = false;
        }
        else {
            this.validationMessage = this.WarningMessage;
        }
    }
    ValidateField(): string {
      
        if (this.newEmployeeRecord.FirstName === undefined)
        {
            this.WarningMessage = this.WarningMessage + " Please enter valid first Name.";
        }

        if (this.newEmployeeRecord.LastName === undefined)
        {
            this.WarningMessage = this.WarningMessage + " Please enter valid Last Name.";
        }

        if ((this.newEmployeeRecord.Ssn === undefined || this.newEmployeeRecord.Ssn.length != 9) && (this.newEmployeeRecord.Dob === undefined || this.newEmployeeRecord.Dob.toString().length==0))
        {
            this.WarningMessage = this.WarningMessage + " Please enter valid SSN or DOB.";
        }
        if (this.WarningMessage.length === 0)
        {
            this.WarningMessage = "Valid"
        }
        return this.WarningMessage;
    }
   

    parseDate(dateString: string): Date {
        if (dateString) {
            return new Date(dateString);
        } else {
            return null;
        }
    }

    trackByIndex(index: number, obj: any): any {
        return index;
    }

    Part3Deleted(part3Record: Employee1095detailsPart3)
    {
        var index = this.employee.CoveredIndividuals.indexOf(part3Record);

        if (index != -1)
        {
            this.employee.CoveredIndividuals.splice(index, 1);
        }
    }

    Part3Update(part3Record: Employee1095detailsPart3)
    {
        var index = this.employee.CoveredIndividuals.indexOf(part3Record);

        if (index != -1)
        {
            this.employee.CoveredIndividuals[index] = part3Record;
        }
    }

    UpdatePart2Line14(Line14: string)
    {

        this.employee.unsavedChanges = true;

        let all12 = this.employee.EmployeeMonthlyDetails.filter((item) => item.MonthId == 0);

        if (all12 != undefined && all12 != null && all12.length > 0 && all12[0] != undefined && all12[0] != null) {
            if (all12[0].Line14 != undefined && all12[0].Line14 != null) {

                for (var item of this.employee.EmployeeMonthlyDetails) {
                    item.Line14 = all12[0].Line14;
                }

                setTimeout(() => {
                    all12[0].Line14 = null;
                });

            }

        }

    }

    UpdatePart2Line15(Line15: string) {

        this.employee.unsavedChanges = true;

        let all12 = this.employee.EmployeeMonthlyDetails.filter((item) => item.MonthId == 0);

        if (all12 != undefined && all12 != null && all12.length > 0 && all12[0] != undefined && all12[0] != null) {
            if (all12[0].Line15 != undefined && all12[0].Line15 != null) {

                for (var item of this.employee.EmployeeMonthlyDetails) {
                    item.Line15 = all12[0].Line15;
                }

                setTimeout(() => {
                    all12[0].Line15 = null;
                });

            }

        }

    }

    UpdatePart2Line16(Line16: string) {

        this.employee.unsavedChanges = true;

        let all12 = this.employee.EmployeeMonthlyDetails.filter((item) => item.MonthId == 0);

        if (all12 != undefined && all12 != null && all12.length > 0 && all12[0] != undefined && all12[0] != null) {
            if (all12[0].Line16 != undefined && all12[0].Line16 != null) {

                for (var item of this.employee.EmployeeMonthlyDetails) {
                    item.Line16 = all12[0].Line16;
                }

                setTimeout(() => {
                    all12[0].Line16 = null;
                });

            }

        }

    }

    UpdatePart2Receiving1095C(Receiving1095C: boolean) {

        this.employee.unsavedChanges = true;

        let all12 = this.employee.EmployeeMonthlyDetails.filter((item) => item.MonthId == 0);

        if (all12 != undefined && all12 != null && all12.length > 0 && all12[0] != undefined && all12[0] != null)
        {
            if (all12[0].Receiving1095C != undefined && all12[0].Receiving1095C != null)
            {

                for (var item of this.employee.EmployeeMonthlyDetails) {
                    item.Receiving1095C = all12[0].Receiving1095C;
                }

                setTimeout(() => {
                    all12[0].Receiving1095C = null;
                }, 500);

            }

        }

    }
    
}
