import { BaseModel } from '../Base/base.model';
import { Approval1094, Print1094 } from './models';

export class Void1094 extends BaseModel
{
    VoidedOn: Date;
    VoidedBy: string;
    Reason: string;
    Approval: Approval1094;
    Print: Print1094;
}