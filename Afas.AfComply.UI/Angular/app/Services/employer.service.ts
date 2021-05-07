import { Injectable } from '@angular/core';
import { TimeFrame } from '../Models/timeFrame';
import { EmployerIdSelect } from '../Models/employerIdSelect';
import { BaseService } from '../Base/base.service'; 

@Injectable()
export class EmployerService extends BaseService<TimeFrame> {
    protected baseUrl = 'Employer';  
    
 

    GetAllEmployerIdSelect(): Promise<EmployerIdSelect[]> {

        const url = `${this.baseUrl}Api/EmployerList`;

        return this.http.get(url, '{}').toPromise()
            .then(response => {
                return response.json() as EmployerIdSelect[];
            })
            .catch(this.handleError);

    }
    GetAll1095FinzlizedEmployerIdSelect(): Promise<EmployerIdSelect[]> {

        const url = `${this.baseUrl}Api/GetAll1095FinalizedEmployers`;

        return this.http.get(url, '{}').toPromise()
            .then(response => {
                return response.json() as EmployerIdSelect[];
            })
            .catch(this.handleError);

    }
}