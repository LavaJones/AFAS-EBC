import { Injectable } from '@angular/core';
import { Employee1095detailsPart2 } from '../Models/employee1095detailsPart2';
import { BaseService } from '../Base/base.service';

@Injectable()
export class Employee1095detailsPart2Service extends BaseService<Employee1095detailsPart2> {
    protected baseUrl = 'Employee1095detailsPart2';      
    
}