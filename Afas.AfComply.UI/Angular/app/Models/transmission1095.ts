import { BaseModel } from '../Base/base.model';
import { Transmission1094, Approval1095 } from './models';

export class Transmission1095 extends BaseModel
{
    TransmissionTime: Date;
    TransmissionType: string;
    UniqueRecordId: string;
    TransmissionStatus: string;
    Transmission1094: Transmission1094;
    Approval: Approval1095;
}