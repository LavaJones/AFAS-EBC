import { Component, Input, Output, EventEmitter } from '@angular/core';
import { Approved1095detailsPart3 } from 'app/Models/approved1095detailsPart3';
import { FormsModule } from '@angular/forms';
import 'rxjs/Rx';

@Component({
 
    selector: 'part3-show',
    templateUrl: './part3.component.html',
    styleUrls: ['./details1095.component.css'],
})

export class Part3Component
{
    @Input() part3: Approved1095detailsPart3;
      
    constructor() { }

    ngOnInit(): void { }        
}