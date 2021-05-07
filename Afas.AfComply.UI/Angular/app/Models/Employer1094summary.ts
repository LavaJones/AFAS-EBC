import { BaseModel } from '../Base/base.model';
import { Employer1094detailsPart3 } from './employer1094detailsPart3';
import { Employer1094detailsPart4 } from './employer1094detailsPart4';

export class Employer1094summary extends BaseModel {

   
    EmployerName: string;
    EIN: string;
    Address: string;
    City: string;
    State: string;
    StateId: number;
    ZipCode: number;
    IrsContactName: string;
    IrsContactPhone: string;

    IsDge: boolean;
    
    DgeName: string;
    DgeEIN: string;
    DgeAddress: string;
    DgeCity: string;
    DgeStateId: string;
    DgeState: string;
    DgeZipCode: string;
    DgeContactName: string;
    DgeContactPhone: string;


    TransmissionTotal1095Forms: number;
    IsAuthoritiveTransmission: boolean;
    IsAggregatedAleGroup: boolean;
    Total1095Forms: number;


    Employer1094Part3s: Employer1094detailsPart3[];
    Employer1094Part4s: Employer1094detailsPart4[];

    Finalize1094ItemLink: string;
   
}