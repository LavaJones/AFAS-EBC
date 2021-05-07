import { Component, Input, Output, EventEmitter, Inject } from '@angular/core';
import { Approved1095detailsPart2 } from 'app/Models/approved1095detailsPart2';

@Component({
 
    selector: 'part2-show',
    templateUrl: './part2.component.html',
})

export class Part2Component {

    public part2: Approved1095detailsPart2 = null;
    @Input() part2s: Array<Approved1095detailsPart2> = [];
   
    public line14Options: string[] = ["1A","1B","1C", "1D","1E","1F", "1G", "1H", "1J", "1K"];
    public line16Options: string[] = ["", "2A", "2B", "2C", "2D", "2E", "2F", "2G", "2H", "2I"];       
    public InsuranceOptions: any[] = [{ key: "1", value: "Full-Insured" }, { key: "2", value: "Self-Insured" }, { key: "", value: "Select" }];
    public StatusOptions: any[] = [{ key: "1", value: "Seasonal" }, { key: "2", value: "Part-time/Variable" }, { key: "3", value: "Termed" }, { key: "4", value: "Cobra Elected" }, { key: "5", value: "Full-time" }, { key: "6", value: "Special Unpaid Leave" }, { key: "7", value: "Initial Import" }, { key: "8", value: "Retiree" }];
    
    constructor() { }

    ngOnInit(): void
    {
        if (this.part2s.length == 1)
        {
            this.part2 = this.part2s[0];
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
            default : {
                return "";

            }
        }
    }  
}