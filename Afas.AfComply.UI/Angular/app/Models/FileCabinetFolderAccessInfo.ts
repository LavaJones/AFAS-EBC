import { BaseModel } from '../Base/base.model';


export class FileCabinetFolderAccessInfo extends BaseModel {
    FolderName: string;
    FolderDepth: number;
    children: FileCabinetFolderAccessInfo[];
    ApplicationId: number;
    HasFiles: boolean;
    HasSubFolders: boolean;
    ZipDownloadItemLink: string;
    FolderDeleteItemLink: string;
}