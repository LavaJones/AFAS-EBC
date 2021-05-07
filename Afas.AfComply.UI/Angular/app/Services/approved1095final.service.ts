import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import { Approved1095summary } from '../Models/approved1095summary';
import { BaseService } from '../Base/base.service';
import { Dictionary } from '../Base/dictionary';
import { Approved1095detailsPart2 } from '../Models/approved1095detailsPart2';
import { Approved1095detailsPart3 } from '../Models/approved1095detailsPart3';

@Injectable()
export class Approved1095FinalService extends BaseService<Approved1095summary> {
    protected baseUrl = 'Approved1095Final';      

    getTaxYears(): Promise<Dictionary<string>> {

        return this.http.get(this.baseUrl + 'Api/GetTaxYears/')
            .toPromise()
            .then(response => {

                return response.json() as Dictionary<string>;

            })
            .catch(this.handleError);

    }
    
    UnfinalizeAll1095(encryptedParams: string): Promise<any>
    {
        const url = `${this.baseUrl}Api/UnFinalizeAll1095/${encryptedParams}`;

        return this.http.post(url, "")
            .toPromise()
            .catch(this.handleError);

    }

    Unfinalize1095(entity: Approved1095summary): Promise<any>
    {

        const url = entity.Unfinalize1095ItemLink;

        return this.http.post(url, "")
            .toPromise()
            .catch(this.handleError);

    }
    
}