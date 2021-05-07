import { Injectable } from '@angular/core';
import { User } from "../Models/user";
import { ConfirmationClassificationModel } from "../Models/ConfirmationClassificationModel";
import { Employee } from "../Models/employee";
import { BaseService } from '../Base/base.service';
import { Observable } from 'rxjs';
import { Http, Response } from '@angular/http';

@Injectable()
export class IRSConfirmationService   
{
    constructor(private http: Http) { }
    protected baseUrl: string = '/Reporting/ConfirmationPage/Confirm';      

    protected IRSContactUserUrl: string = '/Reporting/ConfirmationPage/GetAllIRSContactUser';
    protected IRSSafeHarborCodesUrl: string = '/Reporting/ConfirmationPage/GetAllIRSSafeHarborCodes';
    protected EmployeesUrl: string = '/Reporting/ConfirmationPage/GetAllEmployees';
    protected AlertUrl: string = '/Reporting/ConfirmationPage/DoAlertExists';

    protected EmployerName: string = '/Reporting/ConfirmationPage/GetEmployerName';

    Confirmation(): Promise<any> {

        return this.http.post(this.baseUrl, '{}').toPromise().then(
            response => {
                return response;
            })
            .catch(this.handleError);

    }

    GetAllIRSContactUser(): Promise<User[]> {

        return this.http.get(this.IRSContactUserUrl, '{}').toPromise().then(response => { return response.json() as User[]; }).catch(this.handleError);

    }

    GetAllIRSSafeHarborCodes(): Promise<ConfirmationClassificationModel[]> {

        return this.http.get(this.IRSSafeHarborCodesUrl, '{}').toPromise().then(response => { return response.json() as ConfirmationClassificationModel[]; }).catch(this.handleError);

    }

    GetAllEmployees(): Promise<Employee[]> {

        return this.http.get(this.EmployeesUrl, '{}').toPromise().then(response => { return response.json() as Employee[]; }).catch(this.handleError);

    }

    GetEmployerName(): Promise<string> {

        return this.http.get(this.EmployerName, '{}').toPromise().then(response => { return response.json() as string; }).catch(this.handleError);

    }

    protected handleError(error: any): Promise<any> {

        console.error('An error occurred', error);     

        return Promise.reject(error.message || error);

    }


    SaveIRSEmployees(toUpdate: Employee[]): Promise<Employee[]> {

        const url = "/Reporting/ConfirmationPage/SaveEmployees";

        let json = JSON.stringify(toUpdate);
        return this.http.post(url, json)
            .toPromise()
            .then(response => {

                return response.json() as Employee[];

            })
            .catch(this.handleError);

    }

    DoAlertExists(): Promise<boolean> {

        return this.http.get(this.AlertUrl, '{}').toPromise().then(response => { return response.json() as boolean; }).catch(this.handleError);

    }

}