import { DataSource } from '@angular/cdk/collections';
import { MatSort } from '@angular/material';
import { BehaviorSubject } from 'rxjs/internal/BehaviorSubject';
import { Observable } from 'rxjs/internal/Observable';
import 'rxjs/add/operator/startWith';
import 'rxjs/add/observable/merge';
import 'rxjs/add/operator/map';

export abstract class BaseDataSource<Type> extends DataSource<Type>
{
    constructor()
    {

        super();

    }

    displayDataChanges = []; 

    protected dataChange: BehaviorSubject<Type[]> = new BehaviorSubject<Type[]>([]);

    abstract connect(): Observable<Type[]>;

    disconnect() { }
    
}
