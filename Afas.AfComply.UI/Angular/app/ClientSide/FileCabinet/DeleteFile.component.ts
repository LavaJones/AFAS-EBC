import { Component, OnInit, Input, Output, EventEmitter, ViewChild, Inject } from '@angular/core';
import { FileCabinetInfo } from 'app/Models/FileCabinetInfo';
import { FileCabinetService } from 'app/Services/fileCabinetservice';
import { MatDialog, MatPaginator, MatTableDataSource, MatDialogRef, MatSort, MAT_DIALOG_DATA } from '@angular/material';
import { MatSnackBar } from '@angular/material/snack-bar';


@Component({

    selector: 'DeleteFile',
    templateUrl: './DeleteFile.component.html',
    styleUrls: ['DeleteFile.component.css'],
    providers: [FileCabinetService],

})
export class DeleteFileComponent implements OnInit {


    constructor(private FileCabinetService: FileCabinetService, public dialogRef: MatDialogRef<DeleteFileComponent>,
        @Inject(MAT_DIALOG_DATA) public data: any, private snackBar: MatSnackBar) {
    }

    ngOnInit() {

    }
    DeleteFile(data: FileCabinetInfo) {
        this.FileCabinetService.DeleteFile(this.data)
            .then(deleted => {
                if (null != deleted && undefined != deleted) {

                    this.snackBar.open("File Deleted Sucessfully", '',
                        {
                            duration: 4000,
                        });
                }
                this.dialogRef.close();

            })

            .catch(ex => {

                console.log("Caught an issue with updating FEIN");

                console.log(ex);

                this.snackBar.open("Error occured while deleting the file", "Dismiss",
                    {
                        panelClass: "Error-Message",
                        duration: 10000,
                        verticalPosition: 'top',      
                        horizontalPosition: 'center'           
                    });
                this.dialogRef.close();
            });
    }
    onNoClick(): void {
        this.dialogRef.close();
    }
}