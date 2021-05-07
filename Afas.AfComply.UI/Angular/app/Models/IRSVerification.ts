import { BaseModel } from '../Base/base.model';

export class IRSVerification extends BaseModel
{
    Step: string;
    Status: boolean;
    StatusString: string;
    Instructions: string;
}