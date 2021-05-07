import { Injectable } from '@angular/core';
import { TimeFrame } from '../Models/timeFrame';
import { BaseService } from '../Base/base.service';

@Injectable()
export class PrintService extends BaseService<TimeFrame> {
    protected baseUrl = 'Print';      
    


    hasPrintedYear(encryptedParams: string): Promise<boolean> {
        const url = `${this.baseUrl}Api/HasPrinted/${encryptedParams}`;

        return this.http.post(url, '{}')
            .toPromise()
            .then(response => {

                return response.json() as boolean;

            })
            .catch(this.handleError);

    } 

    printYear(encryptedParams: string): Promise<any> {
        const url = `${this.baseUrl}Api/Print/${encryptedParams}`;

        return this.http.post(url, '{}')
            .toPromise()
            .catch(this.handleError);

    }
    
    reprintYear(encryptedParams: string): Promise<any> {
        const url = `${this.baseUrl}Api/AllReprint/${encryptedParams}`;

        return this.http.post(url, '{}')
            .toPromise()
            .catch(this.handleError);

    }

    adminPrintYear(encryptedParams: string): Promise<any> {
        const url = `${this.baseUrl}Api/AdminPrint/${encryptedParams}`;

        return this.http.post(url, '{}')
            .toPromise()
            .catch(this.handleError);

    }

    adminReprintYear(encryptedParams: string): Promise<any> {
        const url = `${this.baseUrl}Api/AdminAllReprint/${encryptedParams}`;

        return this.http.post(url, '{}')
            .toPromise()
            .catch(this.handleError);

    }

    pdfPrintYear(encryptedParams: string): Promise<any> {
        const url = `${this.baseUrl}Api/PdfPrint/${encryptedParams}`;

        return this.http.post(url, '{}')
            .toPromise()
            .catch(this.handleError);

    } 
    
    print1094(encryptedParams: string): Promise<any> {
        const url = `${this.baseUrl}Api/Print1094/${encryptedParams}`;

        return this.http.post(url, '{}')
            .toPromise()
            .catch(this.handleError);

    } 
    
}