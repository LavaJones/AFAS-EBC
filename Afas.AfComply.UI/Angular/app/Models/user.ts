
import { BaseModel } from '../Base/base.model';
import { Transmission1094, Approval1095 } from './models';

export class User extends BaseModel
{
    
    User_First_Name: string;
    User_Last_Name: string;
    User_Full_Name:string;
    User_Email: string;
    User_Phone: number;
    User_UserName: string;
    User_Password: string;
    employer_id: number;
    USER_ACTIVE: boolean;
    poweruser: boolean;
    LAST_MOD_BY: boolean;
    LAST_MOD: boolean;
    User_PWD_RESET: boolean;
    User_Billing: boolean;
    User_IRS_CONTACT: boolean;
    User_Floater: boolean;
    ResourceId: boolean;
}