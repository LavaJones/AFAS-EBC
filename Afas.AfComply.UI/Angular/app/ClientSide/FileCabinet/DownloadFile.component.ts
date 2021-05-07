import { Component, OnInit, Input, Output, EventEmitter, ViewChild } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { BlockUI, NgBlockUI } from 'ng-block-ui';
import { FileCabinetInfo } from 'app/Models/FileCabinetInfo';
import { FileCabinetService } from 'app/Services/fileCabinetservice';
import { FileCabinetAccess } from 'app/Models/FileCabinetAccess';
import { MatTreeNestedDataSource } from '@angular/material/tree';
import { NestedTreeControl } from '@angular/cdk/tree';
import { FileCabinetFolderAccessInfo } from 'app/Models/FileCabinetFolderAccessInfo';
import { MatDialog, MatPaginator, MatTableDataSource, MatDialogRef, MatSort, MAT_DIALOG_DATA } from '@angular/material';
import { UploadFileComponent } from 'app/ClientSide/FileCabinet/UploadFile.component';
import { DeleteFileComponent } from '../FileCabinet/DeleteFile.component';
import { DeleteFolderComponent } from '../FileCabinet/DeleteFolder.component';


interface Folders {
    FolderName: string;
    HasSubFolders?: boolean;
    children: Folders[];
};
export interface FileCabinetColumns {
    Filename: string;
    FileType: string;
    FileDescription: string;

}
@Component({

    selector: 'DownloadFile',
    templateUrl: './DownloadFile.component.html',
    styleUrls: ['DownloadFile.component.css'],
    providers: [FileCabinetService],

})
export class DownloadFileComponent implements OnInit {
    Files: FileCabinetInfo[];
    panelOpenState = false;
    SelectedItem: FileCabinetFolderAccessInfo;
    TREE_DATA = null;
    Element_data = null;
    Folders: FileCabinetFolderAccessInfo[];
    data: FileCabinetFolderAccessInfo[];
    treeControl = new NestedTreeControl<Folders>(node => node.children);
    dataSource = new MatTreeNestedDataSource<Folders>();
    displayedColumns: string[] = ['Filename', 'Download', 'Delete'];
    FileCabinetdatasource: MatTableDataSource<FileCabinetColumns>;
    @ViewChild(MatPaginator) paginator: MatPaginator;
    @ViewChild(MatSort) sort: MatSort;
    hideDelay = new FormControl(3000);
    showDelay = new FormControl(1000);
    position = new FormControl('left');


    constructor(private FileCabinetService: FileCabinetService, public dialog: MatDialog) {
    }
    ngOnInit(): void {
        this.FileCabinetService.GetByFolders().then(fromServer => {
            this.Folders = fromServer;

            this.TREE_DATA = this.Folders;
            this.dataSource.data = this.TREE_DATA;

            this.SelectedItem = this.Folders[0];

            this.loadFiles(this.SelectedItem);
        })

    }
    loadFiles(SelectedItem: any): void {
        this.FileCabinetService.GetFilesForFolder(this.SelectedItem.ThisUrlParameter).then(fromServer => {
            this.Files = fromServer;
            this.FileCabinetdatasource = new MatTableDataSource(this.Files);
            this.FileCabinetdatasource.sort = this.sort;
            this.FileCabinetdatasource.paginator = this.paginator;

        });
    }
    applyFileNameFilter(filterValue: string) {
        this.FileCabinetdatasource = new MatTableDataSource(this.Files);
        this.FileCabinetdatasource.filter = filterValue.trim().toLowerCase();
    }

    onFolderClicked(folder: FileCabinetFolderAccessInfo): void {
        console.log(folder);
        console.log("Folder selected to upload");

        this.SelectedItem = folder
        this.loadFiles(this.SelectedItem);



    }
    hasChild = (_: number, node: Folders) => {
        return node.HasSubFolders && node.children.length > 0;
    }

    disableButton(): boolean {
        if (this.Files.length >= 1) {
            return false;
        }
        else {
            return true;
        }
    }
    openDialog(): void {
        const dialogRef = this.dialog.open(UploadFileComponent, {
            width: '500px',
            data: this.SelectedItem.ThisUrlParameter
        });

        dialogRef.afterClosed()
            .subscribe(result =>
            {
                this.loadFiles(this.SelectedItem)
            });
    }

    openDialogDelete(SelectedFile: any = FileCabinetInfo): void {
        const dialogRef = this.dialog.open(DeleteFileComponent, {
            height: '300px',
            width: '500px',
            data: SelectedFile

        });
        dialogRef.afterClosed()
            .subscribe(result =>
            {
                this.loadFiles(this.SelectedItem)
            });

    }
    FolderDelete(): void {
        const dialogRef = this.dialog.open(DeleteFolderComponent, {
            height: '300px',
            width: '500px',
            data: this.SelectedItem
        });
        dialogRef.afterClosed()
            .subscribe(result =>
            {
                this.loadFiles(this.SelectedItem)
            });
    }

    DownloadFile(selectedFile: FileCabinetInfo): void {

        window.open(selectedFile.DownloadItemLink);
    }


    ZipDownload(): void {

        window.open(this.SelectedItem.ZipDownloadItemLink);

    }


}










