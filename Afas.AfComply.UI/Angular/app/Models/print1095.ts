import { BaseModel } from '../Base/base.model';
import { Approval1095, PrintBatch } from './models';

export class Print1095 extends BaseModel
{
    Approved1095: Approval1095;
    PrintBatch : PrintBatch;
}