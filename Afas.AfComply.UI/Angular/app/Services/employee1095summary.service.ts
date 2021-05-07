import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import { Employee1095summary } from '../Models/employee1095summary';
import { BaseService } from '../Base/base.service';
import { Dictionary } from '../Base/dictionary';
import { Employee1095detailsPart2 } from '../Models/employee1095detailsPart2';
import { Employee1095detailsPart3 } from '../Models/employee1095detailsPart3';

@Injectable()
export class Employee1095summaryService extends BaseService<Employee1095summary> {
    protected baseUrl = 'Employee1095summary';      

    getTaxYears(): Promise<Dictionary<string>> {

        return this.http.get(this.baseUrl + 'Api/GetTaxYears/')
            .toPromise()
            .then(response => {

                return response.json() as Dictionary<string>;

            })
            .catch(this.handleError);

    }

    SaveNewInsuranceCoverage(test: Employee1095detailsPart3, para2: Employee1095summary): Promise<Employee1095detailsPart3> {

        let json = JSON.stringify(test);
        return this.http.post(para2.AddPart3ICItemLink, json)
            .toPromise()
            .then(response => {

                return response.json() as Employee1095detailsPart3;

            })
            .catch(this.handleError);

    }

    updatePart1(entity: Employee1095summary): Promise<Employee1095summary> {

        const url = entity.UpdatePart1Link;

        let json = JSON.stringify(entity);

        return this.http.put(url, json)
            .toPromise()
            .then(response => {

                return response.json() as Employee1095summary;

            })
            .catch(this.handleError);

    }

    updateMultiplePart2(entity: Employee1095summary, allToUpdate: Employee1095detailsPart2[]): Promise<Employee1095detailsPart2[]> {

        const url = entity.UpdateMultiplePart2Link;

        let json = JSON.stringify(allToUpdate);

        return this.http.put(url, json)
            .toPromise()
            .then(response => {

                return response.json() as Employee1095detailsPart2[];

            })
            .catch(this.handleError);

    }

    getPart2ForEmployee(entity: Employee1095summary): Promise<Employee1095detailsPart2[]> {

        const url = entity.LoadPart2ItemLink;

        entity.IsLoadingPart2 = true;

        return this.http.get(url)
            .toPromise()
            .then(response => {

                var result = response.json() as Employee1095detailsPart2[];

                entity.EmployeeMonthlyDetails = result;
                entity.IsPart2Loaded = true;
                entity.IsLoadingPart2 = false;

                return result;

            })
            .catch(this.handleError);

    }

    getPart3ForEmployee(entity: Employee1095summary): Promise<Employee1095detailsPart3[]> {

        const url = entity.LoadPart3ItemLink;

        entity.IsLoadingPart3 = true;

        return this.http.get(url)
            .toPromise()
            .then(response => {

                var result = response.json() as Employee1095detailsPart3[];

                entity.CoveredIndividuals = result;
                entity.IsPart3Loaded = true;
                entity.IsLoadingPart3 = false;

                return result;

            })
            .catch(this.handleError);

    }

    finalize1095(entity: Employee1095summary): Promise<any>
     {

        const url = entity.Finalize1095ItemLink;
        return this.http.post(url, "")
            .toPromise()
            .then(response => {

                return response.json() as string;

            })
            .catch(this.handleError);

    }

    Review1095(entity: Employee1095summary): Promise<any>
    {

        const url = entity.Review1095ItemLink;

        let json = JSON.stringify(entity.Reviewed);

        return this.http.post(url, json)
            .toPromise()
            .catch(this.handleError);

    }

    uploadEditFile(file: any, encryptedParams: string): Promise<any>
    {
        const url = `${this.baseUrl}Api/UploadFileEdits/${encryptedParams}`;

        let formData: FormData = new FormData();
        formData.append("file[]", file, file.name);

        return this.http.post(url, formData)
            .toPromise()
            .catch(this.handleError);

    }

}