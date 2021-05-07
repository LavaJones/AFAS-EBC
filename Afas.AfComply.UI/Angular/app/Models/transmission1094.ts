import { BaseModel } from '../Base/base.model';
import { Approval1094, Transmission1095 } from './models';

export class Transmission1094 extends BaseModel
{
    TransmissionTime: Date;
    TransmissionType: string;
    UniqueRecordId: string;
    TransmissionStatus: string;
    Approval: Approval1094;
    IrsReciptId: string;
    All1095Tranmissions: Transmission1095[];
}