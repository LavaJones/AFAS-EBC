import { BaseModel } from '../Base/base.model';
import { Approved1095detailsPart2 } from './approved1095detailsPart2';
import { Approved1095detailsPart3 } from './approved1095detailsPart3';

export class Approved1095summary extends BaseModel
{

    Printed: boolean;
    Receiving1095: boolean;

    FirstName: string;
    MiddleName: string;
    LastName: string;
    SSN : string;
    StreetAddress: string;
    City: string;
    State: number; 
    Zip: number;

    part2s: Approved1095detailsPart2[];
    part3s: Approved1095detailsPart3[];

    Unfinalize1095ItemLink: string;
    
    get FullName() : string
    {
        return this.FirstName + ' ' + this.MiddleName + ' ' + this.LastName;
    }    

    isExpanded: boolean;

}