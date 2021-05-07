import { Component, Input, Output, EventEmitter } from '@angular/core';
import { Location } from '@angular/common';
import { Employee1095detailsPart3 } from "app/Models/employee1095detailsPart3";
import { Employee1095detailsPart3Service } from 'app/Services/employee1095detailsPart3.service';
import { BlockUI, NgBlockUI } from 'ng-block-ui';
import { FormsModule } from '@angular/forms';
import 'rxjs/Rx';
@Component({
 
    selector: 'part3-edit',
    templateUrl: './part3-edit.component.html',
    styleUrls: ['./view1095.component.css'],
    providers: [Employee1095detailsPart3Service],
})

export class Part3EditComponent {
    validationMessage: string;
    @BlockUI('part3-1095') blockUI: NgBlockUI;

    @Input() part3: Employee1095detailsPart3;
    @Input() IsEmployee: boolean;
    @Input() Count: number;
    @Output() Part3Deleted = new EventEmitter();
    @Output() Part3Update = new EventEmitter();
   
    part3Old: Employee1095detailsPart3;
    constructor(private part3Service: Employee1095detailsPart3Service, private location: Location) { this.validationMessage = ""; }

    ngOnInit(): void {

    }

    onClickEdit()
    {
        
        this.part3Old = JSON.parse(JSON.stringify(this.part3));
       
    }

    onClickCancel()
    {
        this.part3 = this.part3Old;
    }

    onClickDelete() {
            this.part3Service.delete(this.part3).then(deleted => {
                this.Part3Deleted.emit();
            });

    }

    editedBackground(): any {
        if (this.part3.unsavedChanges) {
            return { 'background-color': 'red' };
        }
        return null;
    }


    onClickSave() {

        this.part3Service.update(this.part3).then(all => {
            this.Part3Update.emit(this.part3Service);
        });
    }


    parseDate(dateString: string): Date {
        if (dateString) {
            return new Date(dateString);
        } else {
            return null;
        }
    }

    CheckForEmployee(SSN: string): boolean {
        if (SSN && SSN.startsWith("*****"))
        {
            return true;
        }
        else
        {
            return false;
        }
    }


}