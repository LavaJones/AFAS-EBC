import { BaseModel } from '../Base/base.model';
import { Approval1094, PrintBatch } from './models';

export class Print1094 extends BaseModel
{
    Approved1094: Approval1094;
    PrintBatch : PrintBatch;
}