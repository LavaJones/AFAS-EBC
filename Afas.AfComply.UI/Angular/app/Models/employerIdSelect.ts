export class EmployerIdSelect {

    EncryptedId: string;
    EmployerName: string;
    EIN: string;
    Address: string;
    City: string;
    State: string;
    Zip: string;

    constructor(values: Object = {}) {
        Object.assign(this, values);
    }
}