import { Injectable } from '@angular/core';
import { Http, XHRBackend, RequestOptions, Request, RequestOptionsArgs, Response, Headers } from '@angular/http';
import { Observable } from 'rxjs/internal/Observable';
import { from } from 'rxjs';
import { environment } from 'environments/environment';


@Injectable()
export class HttpService extends Http {

    currentToken: string;

    constructor(backend: XHRBackend, options: RequestOptions) {

        super(backend, options);

        this.getVerificationToken();

    }

    async getVerificationToken(options?: RequestOptionsArgs): Promise<string> {
        options = { headers: new Headers() };

        options.headers.append('Cache-control', 'no-cache');
        options.headers.append('Cache-control', 'no-store');
        options.headers.append('Expires', '0');
        options.headers.append('Pragma', 'no-cache');

        return this.get('RenewToken', options)
            .toPromise()
            .then(response => {

                this.currentToken = response.text();

                if ((this.currentToken.indexOf('Logout.aspx') >= 0) || (this.currentToken.indexOf('default.aspx') >= 0)) {
                    alert(environment.Branding.AutoLogoutMessage);
                    window.location.href = window.location.href;
                }

                return response.text();

            });

    }

    post(url: string, body: any, options?: RequestOptionsArgs): Observable<Response>
    {

        return from(this.getVerificationToken().then(() =>
        {

            if (!options)
            {
                options = { headers: new Headers() };
            }

            options.headers.append('__RequestVerificationToken', this.currentToken);
            options.headers.append('Cache-control', 'no-cache');
            options.headers.append('Cache-control', 'no-store');
            options.headers.append('Expires', '0');
            options.headers.append('Pragma', 'no-cache');

            return super.post(url, body, options).toPromise();

        }));

    }

    put(url: string, body: any, options?: RequestOptionsArgs): Observable<Response>
    {

        return from(this.getVerificationToken().then(() => {

            if (!options) {
                options = { headers: new Headers() };
            }

            options.headers.append('__RequestVerificationToken', this.currentToken);
            options.headers.append('Cache-control', 'no-cache');
            options.headers.append('Cache-control', 'no-store');
            options.headers.append('Expires', '0');
            options.headers.append('Pragma', 'no-cache');

            return super.put(url, body, options).toPromise();

        }));

    }

    delete(url: string, options?: RequestOptionsArgs): Observable<Response>
    {

        return from(this.getVerificationToken().then(() => {

            if (!options) {
                options = { headers: new Headers() };
            }

            options.headers.append('__RequestVerificationToken', this.currentToken);
            options.headers.append('Cache-control', 'no-cache');
            options.headers.append('Cache-control', 'no-store');
            options.headers.append('Expires', '0');
            options.headers.append('Pragma', 'no-cache');

            return super.delete(url, options).toPromise();

        }));

    }
}