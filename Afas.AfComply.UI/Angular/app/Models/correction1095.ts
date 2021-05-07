import { BaseModel } from '../Base/base.model';
import { Void1095, Approval1095 } from './models';

export class Correction1095 extends BaseModel
{
    Voided1095: Void1095;
    Approved1095: Approval1095;
}