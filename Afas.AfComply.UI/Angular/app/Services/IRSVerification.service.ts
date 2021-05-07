import { Injectable } from '@angular/core';
import { IRSVerification } from '../Models/IRSVerification';
import { BaseService } from '../Base/base.service';

@Injectable()
export class IRSVerificationService extends BaseService<IRSVerification> 
{
    protected baseUrl = 'Verification';      

}