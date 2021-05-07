import { Component, Input, Output, EventEmitter } from '@angular/core';
import { Approved1095summary } from "app/Models/approved1095summary";
import { Approved1095detailsPart2 } from "app/Models/approved1095detailsPart2";
import { Approved1095detailsPart3 } from 'app/Models/approved1095detailsPart3';
import { Approved1095FinalService } from 'app/Services/approved1095final.service';

@Component({
    selector: 'details1095',
    templateUrl: './details1095.component.html',
    styleUrls: ['./details1095.component.css']
})

export class Details1095Component
{
    @Input() employee: Approved1095summary;
    public listOfLists: Array<Approved1095detailsPart2[]> = [];

    constructor(private approve1095Service: Approved1095FinalService)
    {

    }

    ngOnInit(): void
    {

        for (var i = 0; i <= 12; i++)
        {
            this.listOfLists.push(this.employee.part2s.filter((item) => item.MonthId == i));
        }

    }

    Unapprove(): void{

        this.approve1095Service.Unfinalize1095(this.employee).then(res => { });

    }

}
