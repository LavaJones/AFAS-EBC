import { Component, Input, Output, EventEmitter, Inject } from '@angular/core';
import { Employee1095summary } from "app/Models/employee1095summary";
import { Employee1095detailsPart2 } from "app/Models/employee1095detailsPart2";
import { Employee1095summaryService } from 'app/Services/employee1095summary.service';

import { Employer1094summary } from "app/Models/employer1094summary";
import { Employer1094detailsPart3 } from "app/Models/employer1094detailsPart3";
import { Employer1094detailsPart4 } from "app/Models/employer1094detailsPart4";
import { Employer1094summaryService } from 'app/Services/employer1094summary.service';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

@Component({
 
    selector: 'reporting-ui-message',
    templateUrl: './ui-message.component.html',
})

export class UIMessage
{

    public message: string;

    constructor(
        public dialogRef: MatDialogRef<UIMessage>,
        @Inject(MAT_DIALOG_DATA) public data: any)
    {
        this.message = data.message;
    }

    onOkClick(): void {
        this.dialogRef.close();
    }

}
