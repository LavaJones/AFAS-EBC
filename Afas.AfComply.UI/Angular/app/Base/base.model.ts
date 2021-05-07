export class BaseModel {

    ThisUrlParameter: string;
    GetSingleItemLink: string;
    UpdateItemLink: string;
    DeleteItemLink: string;

    constructor(values: Object = {}) {
        Object.assign(this, values);
    }
}