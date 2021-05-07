import { Injectable } from '@angular/core';
import { Employee1095detailsPart3 } from '../Models/employee1095detailsPart3';
import { BaseService } from '../Base/base.service';
import { User } from "../Models/user";
@Injectable()
export class Employee1095detailsPart3Service extends BaseService<Employee1095detailsPart3> {
    protected baseUrl = 'Employee1095detailsPart3';      
    protected IRSContactUserUrl: string = '/Reporting/Employee1095summaryApi/GetAll';  

    
}