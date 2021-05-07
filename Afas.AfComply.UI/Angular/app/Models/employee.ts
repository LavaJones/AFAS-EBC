import { BaseModel } from '../Base/base.model';

export class Employee //extends BaseModel
{
    EMPLOYEE_ID: number
    EMPLOYEE_TYPE_ID: number;
    EMPLOYEE_HR_STATUS_ID: number;
    EMPLOYEE_EMPLOYER_ID: number;
    EMPLOYEE_FIRST_NAME: string;
    EMPLOYEE_MIDDLE_NAME: string;
    EMPLOYEE_LAST_NAME: string;
    EMPLOYEE_FULL_NAME: string;
    EMPLOYEE_FULL_NAME_SSN: string
    EMPLOYEE_FULL_NAME_ExtID: string
    EMPLOYEE_ADDRESS: string
    EMPLOYEE_CITY: string;
    EMPLOYEE_STATE_ID: number;
    EMPLOYEE_ZIP: number;
    EMPLOYEE_HIRE_DATE: Date;
    EMPLOYEE_C_DATE: Date;
    Employee_SSN_Visible: string;
    Employee_SSN_Hidden: string
    EMPLOYEE_TERM_DATE: Date;
    EMPLOYEE_DOB: Date;
    EMPLOYEE_EXT_ID: string;
    EMPLOYEE_PERCENT_MPP: number;
    EMPLOYEE_PERCENT_HWP: number;
    EMPLOYEE_PERCENT_QT: number;
    EMPLOYEE_IMP_END: Date;
    EMPLOYEE_PLAN_YEAR_ID: number
    EMPLOYEE_PLAN_YEAR_ID_LIMBO: number;
    EMPLOYEE_PLAN_YEAR_ID_MEAS: number;
    EMPLOYEE_AVG_HOURS_WORKED: number;
    EMPLOYEE_HOURS_WORKED: number;
    EMPLOYEE_PY_AVG_STABILITY_HOURS: number;
    EMPLOYEE_PY_AVG_ADMIN_HOURS: number;
    EMPLOYEE_PY_AVG_MEAS_HOURS: number;
    EMPLOYEE_PY_AVG_INIT_HOURS: number;
    EMPLOYEE_CLASS_ID: number;
    EMPLOYEE_ACT_STATUS_ID: number;
}