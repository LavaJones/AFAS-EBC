import { Component, Input, Output, EventEmitter, Inject } from '@angular/core';
import { Employee1095summary } from "app/Models/employee1095summary";
import { Employee1095detailsPart2 } from "app/Models/employee1095detailsPart2";
import { Employee1095summaryService } from 'app/Services/employee1095summary.service';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

@Component({
 
    selector: 'resolvePart2Conflict',
    templateUrl: './resolvePart2Conflict.component.html',
})

export class ResolvePart2Conflict
{

    public part2s: Employee1095detailsPart2[];

    constructor(
        public dialogRef: MatDialogRef<ResolvePart2Conflict>,
        @Inject(MAT_DIALOG_DATA) public data: any)
    {
        this.part2s = data.part2s;
    }

    ngOnInit(): void {
        
    }

    onNoClick(): void {
        this.dialogRef.close();
    }

}
