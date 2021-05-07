import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import { BaseService } from '../Base/base.service';
import { Dictionary } from '../Base/dictionary';
import { FileCabinetInfo } from '../Models/FileCabinetInfo';
import { FileCabinetFolderAccessInfo } from '../Models/FileCabinetFolderAccessInfo'
import { FileCabinetAccess } from '../Models/FileCabinetAccess';
import { Body } from '@angular/http/src/body';



@Injectable()
export class FileCabinetService extends BaseService<FileCabinetAccess>
{
    protected baseUrl = 'FileCabinetInfo'; //// URL to web api

    uploadFile(file: any, fileName: string, fileDescription: string, SelectedItem: string): Promise<any> {
        const url = `${this.baseUrl}Api/UploadFile/`;

        let formData: FormData = new FormData();
        console.log("file are", file);
        formData.append("file[]", file, file.name);
        formData.append("fileName", fileName);
        formData.append("fileDescription", fileDescription);
        formData.append("FolderEncryptedValues", SelectedItem);

        return this.http.post(url, formData)
            .toPromise().then(
                response => {
                    return response as Body;
                })

            .catch(this.handleError);

    }
    GetFilesForFolder(EncryptedParameters: string): Promise<FileCabinetInfo[]> {

        const url = `${this.baseUrl}Api/GetFilesForFolder/${EncryptedParameters}`;

        return this.http.get(url, '{}')
            .toPromise()
            .then(response => {

                return response.json() as FileCabinetInfo[];

            })
            .catch(this.handleError);
    }

    GetByFolders(): Promise<FileCabinetFolderAccessInfo[]> {
        const url = `${this.baseUrl}Api/GetFolders/`;

        console.log("tree Component of FileCabinet");
        return this.http.get(url, '{}')
            .toPromise()
            .then(response => {
                return response.json() as FileCabinetFolderAccessInfo[];
            })
            .catch(this.handleError);
    }
    DeleteFile(entity: FileCabinetInfo): Promise<FileCabinetInfo> {
        return this.http.post(entity.DeleteItemLink, "")
            .toPromise().then(
                response => {
                    return response.json() as FileCabinetInfo;
                })

            .catch(this.handleError);
    }
    DeleteFolder(SelectedItem: FileCabinetFolderAccessInfo): Promise<any> {
        return this.http.post(SelectedItem.FolderDeleteItemLink, "")
            .toPromise().then(
                response => {
                    return response as Body;
                })

            .catch(this.handleError);
    }

}