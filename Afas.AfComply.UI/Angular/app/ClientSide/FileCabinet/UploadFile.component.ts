import { Component, OnInit, Input, Output, EventEmitter, Inject } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { BlockUI, NgBlockUI } from 'ng-block-ui';
import { FileCabinetInfo } from "app/Models/FileCabinetInfo";
import { FileCabinetService } from 'app/Services/fileCabinetservice';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({

    selector: 'UploadFile',
    templateUrl: './UploadFile.component.html',
    styleUrls: ['UploadFile.component.css'],
    providers: [FileCabinetService],
})
export class UploadFileComponent implements OnInit {
    FolderUploadUrl: string;
    fileName: string;
    fileDescription: string;
    files: any;
    ErrorMessage: string;
    ValidationMessage: string;
    constructor(private FileCabinetService: FileCabinetService, public dialogRef: MatDialogRef<UploadFileComponent>,
        @Inject(MAT_DIALOG_DATA) public data: string, private snackBar: MatSnackBar) { }
    ngOnInit() {

    }
    onUploadFile(event) {
        this.files = event.target.files;
    }


    onUpload(): void {
        if (this.fileName == null) {
            this.ValidationMessage = "Filename is  required";
        }

        if (this.files.length > 0 && (false == this.isValidFiles(this.files))) {
            this.ErrorMessage = "Files are not Valid, Please Upload a Valid File";
            return;
        }
        if (this.fileName != null) {
            this.FileCabinetService.uploadFile(this.files[0], this.fileName, this.fileDescription, this.data)
                .then(Uploaded => {

                    if (Uploaded != null) {
                        this.snackBar.open("File Uploaded Sucessfully", '',
                            {
                                duration: 4000,
                            });
                    }
                    this.dialogRef.close();
                })
                .catch(ex => {

                    console.log("Caught an issue while uploading the file");

                    console.log(ex);

                    this.snackBar.open("Error occured while Uploading  the file", "Dismiss",
                        {
                            panelClass: "Error-Message",
                            duration: 10000,
                            verticalPosition: 'top',      
                            horizontalPosition: 'center'           
                        });
                });

        }

    }
    private isValidFiles(files): boolean {
        if (files.length != 1) {
            console.log("wronge number of files: " + files.length);
            return false;
        }
        var ext = files[0].name.toUpperCase().split('.').pop() || files[0].name;
        if (files[0].size <= 0) {
            console.log(" file is empty: " + files[0].size);
            return false;
        }
    }

    onNoClick(): void {
        this.dialogRef.close();
    }


}