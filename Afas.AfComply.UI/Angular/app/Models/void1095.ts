import { BaseModel } from '../Base/base.model';
import { Approval1095, Print1095 } from './models';

export class Void1095 extends BaseModel
{
    VoidedOn: Date;
    VoidedBy: string;
    Reason: string;
    Approval: Approval1095;
    Print: Print1095;
}