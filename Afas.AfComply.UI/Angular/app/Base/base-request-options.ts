import { Injectable } from '@angular/core';
import { BaseRequestOptions, RequestOptions, RequestOptionsArgs, Http } from '@angular/http';

@Injectable()
export class AppRequestOptions extends BaseRequestOptions {

    constructor() {
        super();
        this.headers.set('Content-Type', 'application/json');
    }

}