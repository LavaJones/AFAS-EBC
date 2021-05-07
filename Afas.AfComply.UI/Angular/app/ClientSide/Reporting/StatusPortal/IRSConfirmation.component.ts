import { Component, Input, Output, EventEmitter } from '@angular/core';
import { Location } from '@angular/common';
import { User } from "app/Models/user";
import { ConfirmationClassificationModel } from "app/Models/ConfirmationClassificationModel";
import { Employee } from "app/Models/employee";
import { IRSConfirmationService } from 'app/Services/IRSConfirmation.service';
import { AcaStatus } from "app/Models/Enums/AcaStatus";
import { Router } from '@angular/router';
import { environment } from 'environments/environment';
import { MatDialog, MatPaginator, MatTableDataSource, MatDialogRef, MatSort, MAT_DIALOG_DATA } from '@angular/material';

@Component({

    selector: 'IRSConfirmation',
    templateUrl: './IRSConfirmation.component.html',
    providers: [IRSConfirmationService]
})

export class IRSConfirmationComponent {

    IRSConfirmationEmployee: Array<Employee> = [];
    IRSConfirmationUser: Array<User> = [];
    IRSConfirmationSafeHarborCodes: Array<ConfirmationClassificationModel> = [];
    DoAlertExists: boolean;
    EmployerName: string;

    Branding = environment.Branding;
    Feature = environment.Feature;

    acaStatuses = AcaStatus;
    statusKeys: string[];

    constructor(private ConfirmationService: IRSConfirmationService, public dialog: MatDialog,
        private router: Router,
        private location: Location) {
        this.statusKeys = Object.keys(this.acaStatuses).filter(Number);
    }

    ngOnInit(): void {
        this.ConfirmationService.GetAllIRSContactUser().then(all => { this.IRSConfirmationUser = all; });
        this.ConfirmationService.GetAllEmployees().then(all => { this.IRSConfirmationEmployee = all; });
        this.ConfirmationService.GetAllIRSSafeHarborCodes().then(all => { this.IRSConfirmationSafeHarborCodes = all; });
        this.ConfirmationService.DoAlertExists().then(all => { this.DoAlertExists = all; });

        this.ConfirmationService.GetEmployerName().then(all => { this.EmployerName = all; });

    }
    onClickSave() {

        this.ConfirmationService.SaveIRSEmployees(this.IRSConfirmationEmployee)
            .then(all => {
                if (all)
                {
                    this.IRSConfirmationEmployee = all;
                }
                else
                {
                    console.log(all);
                    this.IRSConfirmationEmployee = [];
                }
            }).catch(error => { console.log(error); });

    }

    onClickConfirm() {

        this.ConfirmationService.Confirmation().then(confirmed => {
            if (confirmed != null) {
                const dialofRef = this.dialog.open(SuccessMessageComponent, {
                    minWidth: "50vw",
                    maxWidth: "90vw",
                    minHeight: "10vh",
                    maxHeight: "90vh",
                });

            }
            else {
                const dialofRef = this.dialog.open(ErrorMessageComponent, {
                    minWidth: "50vw",
                    maxWidth: "90vw",
                    minHeight: "10vh",
                    maxHeight: "90vh",
                });
            }
        }).catch(error => {
            console.log(error);
            const dialofRef = this.dialog.open(ErrorMessageComponent, {
                minWidth: "50vw",
                maxWidth: "90vw",
                minHeight: "10vh",
                maxHeight: "90vh",
            });
        });

    }
}
@Component({
    selector: 'SuccessMessage.Component',
    templateUrl: 'SuccessMessage.Component.html',
})
export class SuccessMessageComponent { }

@Component({
    selector: 'ErrorMessage.Component',
    templateUrl: 'ErrorMessage.Component.html',
})
export class ErrorMessageComponent { }
