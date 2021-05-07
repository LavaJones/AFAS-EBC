import { BaseModel } from '../Base/base.model';
import { AcaStatus } from './Enums/AcaStatus';
export class Employee1095detailsPart2 extends BaseModel
{
    MonthId: number;
    Line14: string;
    Line15: string;
    Line16: string;
    InsuranceType: string;
    AcaStatus: AcaStatus;
    Offered: boolean;
    Enrolled: boolean;
    MonthlyHours: number;
    Receiving1095C: boolean;
    UserEdited: boolean;
    unsavedChanges: boolean = false;

}