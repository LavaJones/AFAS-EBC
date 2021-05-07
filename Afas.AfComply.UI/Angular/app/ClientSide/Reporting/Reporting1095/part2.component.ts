import { Component, Input, Output, EventEmitter, Inject } from '@angular/core';
import { Location } from '@angular/common';
import { Employee1095detailsPart2 } from "app/Models/employee1095detailsPart2";
import { ResolvePart2Conflict } from "./resolvePart2Conflict.component";
import { Employee1095detailsPart2Service} from 'app/Services/employee1095detailsPart2.service';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';


@Component({

    selector: 'part2-edit',
    templateUrl: './part2-edit.component.html',
    providers: [Employee1095detailsPart2Service]
})

export class Part2EditComponent {

    public part2: Employee1095detailsPart2 = null;
    @Input() part2s: Array<Employee1095detailsPart2> = [];
    @Output() Line14update = new EventEmitter();
    @Output() Line15update = new EventEmitter();
    @Output() Line16update = new EventEmitter();
    @Output() Receiving1095CUpdate = new EventEmitter();
    @Output() InsuranceType = new EventEmitter();
    @Output() Status = new EventEmitter();

    public line14Options: string[] = ["1A", "1B", "1C", "1D", "1E", "1F", "1G", "1H", "1J", "1K"];
    public line16Options: string[] = ["", "2A", "2B", "2C", "2D", "2E", "2F", "2G", "2H", "2I"];       
    public InsuranceOptions: any[] = [{ key: "1", value: "Full-Insured" }, { key: "2", value: "Self-Insured" }, { key: "", value: "Select" }];
    public StatusOptions: any[] = [{ key: "1", value: "Seasonal" }, { key: "2", value: "Part-time/Variable" }, { key: "3", value: "Termed" }, { key: "4", value: "Cobra Elected" }, { key: "5", value: "Full-time" }, { key: "6", value: "Special Unpaid Leave" }, { key: "7", value: "Initial Import" }, { key: "8", value: "Retiree" }];


    constructor(public dialog: MatDialog, private part2Service: Employee1095detailsPart2Service, private location: Location) { }

    ngOnInit(): void {
        if (this.part2s.length == 1) {
            this.part2 = this.part2s[0];
        }
    }

    Resolve(): void {
        let dialogRef = this.dialog.open(ResolvePart2Conflict, {
            width: '500px',
            data: { part2s: this.part2s }
        });

        dialogRef.afterClosed().subscribe(result => {
            console.log('The dialog was closed');
            this.part2 = result;
            this.part2.UserEdited = true;
        });
    }

    editedBackground(): any {
        if (this.part2.unsavedChanges) {
            return { 'background-color': 'red' };
        }
        return null;
    }

    flag(): any {
        if ((this.part2.Line14 == '1B' || this.part2.Line14 == '1C' || this.part2.Line14 == '1D' || this.part2.Line14 == '1E' || this.part2.Line14 == '1J' || this.part2.Line14 == '1K') && (isNaN(parseFloat(this.part2.Line15)) || (parseFloat(this.part2.Line15) < 0)))
        {           

            return { 'background-color': 'red' };

        }

        return null;
    }



    onChangeLine14(newValue, MonthId): void
    {
        this.Line14update.emit(newValue);

        if (MonthId != 0)
        {
            setTimeout(() =>
            {
                this.part2.Line14 = newValue;
            });
        }
    }

    onChangeLine15(event, MonthId): void
    {
        var newVal = this.part2.Line15; 

        this.Line15update.emit(newVal);

        if (MonthId != 0)
        {
            setTimeout(() =>
            {
                this.part2.Line15 = newVal;
            });
        }
    }

    onChangeLine16(newValue, MonthId): void
    {
        this.Line16update.emit(newValue);

        if (MonthId != 0)
        {
            setTimeout(() =>
            {
                this.part2.Line16 = newValue;
            });
        }
    }

    onChangeReceiving1095C(newValue, MonthId): void
    {
        this.Receiving1095CUpdate.emit(newValue);

        if (MonthId != 0)
        {
            setTimeout(() =>
            {
                this.part2.Receiving1095C = newValue;
            });
        }
    }

    CheckForInsuranceVal(key: string): string {
        switch (key) {
            case "1": {
                return "Full-Insured";

            }
            case "2": {
                return "Self - Insured"
            }
            default: {
                return "";

            }
        }
    }


    CheckForStatusVal(key: number): string {
        switch (key) {
            case 1: {
                return "Seasonal";

            }
            case 2: {
                return "Part-time/Variable";
            }

            case 3: {
                return "Termed";

            }
            case 4: {
                return "Cobra Elected";

            }
            case 5: {
                return "Full-time";

            }
            case 6: {
                return "Special Unpaid Leave";
            }
            case 7: {
                return "Initial Import";
            }
            case 8: {
                return "Retiree";
            };
            default: {
                return "";

            }
        }
    }
 

}