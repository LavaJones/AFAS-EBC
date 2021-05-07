import { BaseModel } from '../Base/base.model';

export class Employee1095detailsPart3 extends BaseModel
{
    FirstName: string;
    MiddleName: string;
    LastName: string;
    SsnHidden: string;
    Ssn: string;
    Dob: Date;
    Enrolled: boolean[];
    unsavedChanges: boolean = false;

}