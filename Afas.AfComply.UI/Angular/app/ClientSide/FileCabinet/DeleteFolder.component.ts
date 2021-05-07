import { Component, OnInit, Input, Output, EventEmitter, ViewChild, Inject } from '@angular/core';
import { FileCabinetService } from 'app/Services/fileCabinetservice';
import { MatDialog, MatPaginator, MatTableDataSource, MatDialogRef, MatSort, MAT_DIALOG_DATA } from '@angular/material';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({

    selector: 'DeleteFolder',
    templateUrl: './DeleteFolder.component.html',
    styleUrls: ['DeleteFolder.component.css'],
    providers: [FileCabinetService],

})

export class DeleteFolderComponent implements OnInit {

    constructor(private FileCabinetService: FileCabinetService, public dialogRef: MatDialogRef<DeleteFolderComponent>,
        @Inject(MAT_DIALOG_DATA) public data: any, private snackBar: MatSnackBar) {
    }

    ngOnInit() {

    }
    DeleteFolder() {
        this.FileCabinetService.DeleteFolder(this.data)
            .then(FolderDeleted => {
                if (FolderDeleted != null) {
                    this.snackBar.open("Files deleted Sucessfully", ' ',
                        {
                            duration: 4000,
                        })
                }
                this.dialogRef.close();
            })
            .catch(ex => {

                console.log("Caught an issue while deleting files in the folder");

                console.log(ex);

                this.snackBar.open("Error occured while deleting  the file", "Dismiss",
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