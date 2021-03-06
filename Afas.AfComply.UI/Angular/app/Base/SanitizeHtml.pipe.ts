import { Pipe, PipeTransform } from '@angular/core';
import { SafeHtml, DomSanitizer } from '@angular/platform-browser';

@Pipe({
    name: 'sanitizeHtml'
})
export class SanitizeHtml implements PipeTransform {

    constructor(private _sanitizer: DomSanitizer) { }

    transform(html: string): SafeHtml {
        return this._sanitizer.bypassSecurityTrustHtml(html);
    }
} 