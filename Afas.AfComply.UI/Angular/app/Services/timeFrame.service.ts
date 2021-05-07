import { Injectable } from '@angular/core';
import { TimeFrame } from '../Models/timeFrame';
import { BaseService } from '../Base/base.service';

@Injectable()
export class TimeFrameService extends BaseService<TimeFrame> {
    protected baseUrl = 'TimeFrame';      
    
}