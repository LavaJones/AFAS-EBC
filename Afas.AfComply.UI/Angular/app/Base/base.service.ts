import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import 'rxjs/add/operator/toPromise';
import { BaseModel } from './base.model';

@Injectable()
export class BaseService<TEntity extends BaseModel> {
    protected baseUrl = '';            

    constructor(protected http: Http) { }

    getAll(): Promise<TEntity[]> {

        return this.http.get(this.baseUrl + 'Api')
            .toPromise()
            .then(response => {

                return response.json() as TEntity[];

            })
            .catch(this.handleError);

    }

    getSome(encryptedParams: string): Promise<TEntity[]> {

        const url = `${this.baseUrl}Api/Multiple/${encryptedParams}`;

        return this.http.get(url)
            .toPromise()
            .then(response => {

                return response.json() as TEntity[];

            })
            .catch(this.handleError);

    }

    getThis(entity: TEntity): Promise<TEntity> {

        const url = entity.GetSingleItemLink;

        return this.http.get(url)
            .toPromise()
            .then(response => {

                return response.json() as TEntity;

            })
            .catch(this.handleError);

    }

    getById(encryptedId: string): Promise<TEntity> {

        const url = `${this.baseUrl}Api/${encryptedId}`;

        return this.http.get(url)
            .toPromise()
            .then(response => {

                return response.json() as TEntity;

            })
            .catch(this.handleError);

    }

    addNew(entity: TEntity): Promise<TEntity> {

        let json = JSON.stringify(entity);

        return this.http.post(this.baseUrl + 'Api/Add', json)
            .toPromise()
            .then(response => {

                return response.json() as TEntity;

            })
            .catch(this.handleError);

    }

    update(toUpdate: TEntity): Promise<TEntity> {

        const url = toUpdate.UpdateItemLink;

        let json = JSON.stringify(toUpdate);

        return this.http.put(url, json)
            .toPromise()
            .then(response => {

                return response.json() as TEntity;

            })
            .catch(this.handleError);

    }

    delete(toDelete: TEntity): Promise<any> {

        const url = toDelete.DeleteItemLink;

        return this.http.delete(url)
            .toPromise()
            .catch(this.handleError);

    }

    protected handleError(error: any): Promise<any> {
        var host = "http://" + window.location.host;
        console.error('An error occurred', error);     
        if (error && error.message && error.message.search("Customer Login")) {

        }

        return Promise.reject("");

    }

}