import { BaseModel } from '../Base/base.model';


export class FileCabinetInfo extends BaseModel {
    Filename: string;
    FileDescription: string;
    FileType: string;
    ApplicationId: number;
    DownloadItemLink: string;
    DeleteItemLink: string;
 }