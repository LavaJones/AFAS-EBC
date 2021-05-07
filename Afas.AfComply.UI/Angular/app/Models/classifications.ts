import { BaseModel } from '../Base/base.model';

export class classifications extends BaseModel
{

    CLASS_ID: number
    CLASS_EMPLOYER_ID: number
    CLASS_DESC: string;
    CLASS_AFFORDABILITY_CODE: string;
    CLASS_MOD_ON: Date
    CLASS_MOD_BY: string;
    CLASS_HISTORY: string;
    CLASS_WAITING_PERIOD_ID: number
    CLASS_CREATED_ON: Date
    CLASS_CREATED_BY: Date
    CLASS_ENTITY_STATUS: number
    CLASS_DEFAULT_OOC: string;
    
}