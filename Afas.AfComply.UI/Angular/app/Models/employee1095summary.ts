import { BaseModel } from '../Base/base.model';
import { Employee1095detailsPart2 } from './employee1095detailsPart2';
import { Employee1095detailsPart3 } from './employee1095detailsPart3';

export class Employee1095summary extends BaseModel
{

    Reviewed: boolean;
    Receiving1095: boolean;
    FirstName: string;
    MiddleName: string;
    LastName: string;
    SsnHidden : string;
    Address: string;
    City: string;
    State: number; 
    Zip: number;
    ZipPlus4: number;
    HireDate: Date;
    TermDate: Date;
    TaxYear: number;
    EmployeeClass: string; 
    EmployeeMonthlyDetails: Employee1095detailsPart2[];
    CoveredIndividuals: Employee1095detailsPart3[];

    IsPart2Loaded: boolean;
    IsPart3Loaded: boolean;
    IsLoadingPart2: boolean = false;
    IsLoadingPart3: boolean = false;
    unsavedChanges: boolean = false;

    UpdatePart1Link: string;
    UpdateMultiplePart2Link: string;
    LoadPart2ItemLink: string;
    LoadPart3ItemLink: string;
    AddPart3ICItemLink: string;
    Finalize1095ItemLink: string;
    Review1095ItemLink: string;

    get FullName() : string
    {
        return this.FirstName + ' ' + this.MiddleName + ' ' + this.LastName;
    }    

    isExpanded: boolean;
}