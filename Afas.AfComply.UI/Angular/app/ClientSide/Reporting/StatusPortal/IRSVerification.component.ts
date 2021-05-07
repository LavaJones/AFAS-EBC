import { Component, Input, Output, EventEmitter } from '@angular/core';
import { Location } from '@angular/common';
import { IRSVerification } from "app/Models/IRSVerification";
import { IRSVerificationService } from 'app/Services/IRSVerification.service';
import { environment } from 'environments/environment';


@Component({

    selector: 'IRSVerification',
    templateUrl: './IRSVerification.component.html',
    providers: [IRSVerificationService]
})

export class IRSVerificationComponent {

    irsVerification: Array<IRSVerification> = [];
    test: IRSVerification;

    Branding = environment.Branding;
    Feature = environment.Feature;

    constructor(private VerificationService: IRSVerificationService, private location: Location) { }

    ngOnInit(): void {
        IRSVerificationService
        this.VerificationService.getAll().then(all => {
            this.irsVerification = all;
        });

    }
}