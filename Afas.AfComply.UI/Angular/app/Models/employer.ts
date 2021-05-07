import { BaseModel } from '../Base/base.model';

export class Employer extends BaseModel
{
    EmployerId: number
    Name: string;
    Address: string
    City: string;
    StateId: number;
    State: string;
    Zip: number;
    EIN: string;
   
   
}          