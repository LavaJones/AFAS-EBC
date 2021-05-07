import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';
import { TimeFrame } from "app/Models/timeFrame";
import { TimeFrameService } from 'app/Services/timeFrame.service';

@Component({
 
    selector: 'timeFrame-view',
    templateUrl: './timeFrame.component.html',
    providers: [TimeFrameService]
})

export class TimeFrameComponent implements OnInit
{
    encryptedId: string;

    title = 'TimeFrame';
    timeFrames: TimeFrame[];
    selectedTimeFrame = new TimeFrame(
        {
            Year: new Date().getFullYear(),
            Month: new Date().getMonth()
        });

    constructor(private timeFrameService: TimeFrameService, private location: Location, route: ActivatedRoute)
    {
        this.encryptedId = route.snapshot.params['encryptedId'];
        
    }

    ngOnInit(): void
    {

        this.refreshTimeFrames();

    }

    refreshTimeFrames(): void {
        this.selectedTimeFrame = new TimeFrame({
            Year: new Date().getFullYear(),
            Month: new Date().getMonth()
        });

        this.timeFrameService.getAll().then(timeFrames => {
            this.timeFrames = timeFrames;

            if (this.encryptedId) {
                this.timeFrameService.getById(this.encryptedId).then(timeFrame => {
                    this.selectedTimeFrame = timeFrame;
                });
            }
            else
            {
                this.location.replaceState("/TimeFrame/New");

                this.selectedTimeFrame = new TimeFrame({
                    Year: new Date().getFullYear(),
                    Month: new Date().getMonth()
                });
            }
        });

    }

    itemClick(frame: TimeFrame): void
    {

        this.selectedTimeFrame = frame;

        this.location.replaceState("/TimeFrame/" + frame.ThisUrlParameter);
        
    }

    newItemClick(): void
    {
        this.location.replaceState("/TimeFrame/New");
        
        this.selectedTimeFrame = new TimeFrame({
            Year: new Date().getFullYear(),
            Month: new Date().getMonth()
        });

    }
}
