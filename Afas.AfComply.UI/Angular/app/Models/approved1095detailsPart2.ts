import { BaseModel } from '../Base/base.model';
import { AcaStatus } from './Enums/AcaStatus';
export class Approved1095detailsPart2 extends BaseModel
{

    MonthId: number;
    Line14: string;
    Line15: string;
    Line16: string;
    Offered: boolean;
    Receiving1095C: boolean;

}