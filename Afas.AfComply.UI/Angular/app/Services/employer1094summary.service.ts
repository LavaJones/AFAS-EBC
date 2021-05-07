import { Injectable } from '@angular/core';
import { Employer1094summary } from '../Models/employer1094summary';
import { BaseService } from '../Base/base.service';
import { Employer } from "../Models/employer";
import { Employer1094detailsPart3 } from '../Models/employer1094detailsPart3';
import { Employer1094detailsPart4 } from '../Models/employer1094detailsPart4';

@Injectable()
export class Employer1094summaryService extends BaseService<Employer1094summary> {
    protected baseUrl = 'Employer1094Summary';      

    finalize1094(entity: Employer1094summary): Promise<any> {

        const url = entity.Finalize1094ItemLink;
        return this.http.post(url, "")
            .toPromise()
            .then(response => {

                return response.json() as string;

            })
            .catch(this.handleError);

    }



    Adminfinalize1094(encryptedParams: string): Promise<any> {
        const url = `${this.baseUrl}Api/AdminFinalize1094/${encryptedParams}`;
        return this.http.post(url, "")
            .toPromise()
            .then(response => {

                return response.json() as string;

            })
            .catch(this.handleError);

    }


    Confirm(encryptedParams: string): Promise<any> {
        const url = `${this.baseUrl}Api/Confirm/${encryptedParams}`;
        return this.http.post(url, "")
            .toPromise()
            .then(response => {

                return response.json() as string;

            })
            .catch(this.handleError);

    }
    

}