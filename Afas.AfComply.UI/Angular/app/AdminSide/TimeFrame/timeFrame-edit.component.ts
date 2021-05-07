import { Component, Input, Output, EventEmitter } from '@angular/core';
import { Location } from '@angular/common';
import { TimeFrame } from "app/Models/timeFrame";
import { TimeFrameService } from 'app/Services/timeFrame.service';

@Component({
 
    selector: 'timeFrame-edit',
    templateUrl: './timeFrame-edit.component.html',
    providers: [TimeFrameService]
})

export class TimeFrameEditComponent {

    @Input() timeFrame: TimeFrame;

    @Output() timeFramesUpdated = new EventEmitter();

    constructor(private timeFrameService: TimeFrameService, private location: Location) { }

    ngOnInit(): void {

        this.refreshTimeFrame();

    }

    refreshTimeFrame(): void {

        if (this.timeFrame && this.timeFrame.ThisUrlParameter && this.timeFrame.ThisUrlParameter != '') {
            this.timeFrameService.getThis(this.timeFrame).then(timeFrame => {
                this.timeFrame = timeFrame;
            });
        }

        this.timeFramesUpdated.emit();
    }

    addItemClick(): void {

        this.timeFrameService.addNew(this.timeFrame).then(
            (added) => {
                this.timeFrame = added;
                this.location.replaceState("/TimeFrame/" + this.timeFrame.ThisUrlParameter);

                this.refreshTimeFrame();
            });

    }

    updateItemClick(): void {

        this.timeFrameService.update(this.timeFrame).then(
            (updated) => {
                this.timeFrame = updated;
                this.refreshTimeFrame();
        });

    }

    deleteItemClick(): void {

        this.timeFrameService.delete(this.timeFrame).then(this.refreshTimeFrame);

    }
}
