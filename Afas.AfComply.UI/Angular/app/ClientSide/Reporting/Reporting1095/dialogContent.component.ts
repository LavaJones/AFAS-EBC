import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { AlertService, AuthenticationService } from 'app/Base/base';

@Component({
 
    templateUrl: 'dialogContent.html'
})

export class DialogContent implements OnInit {
    model: any = {};
    loading = false;
    returnUrl: string;

    constructor(
        private route: ActivatedRoute,
        private router: Router,
        private authenticationService: AuthenticationService,
        private alertService: AlertService) { }

    ngOnInit() {
        this.authenticationService.logout();
        this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
    }

    login() {
        this.loading = true; 
        if (this.model.password == 'ihopeThiswork') 
        {
            this.model.close();
        } else {
            this.loading = false;
        }
    }

}

