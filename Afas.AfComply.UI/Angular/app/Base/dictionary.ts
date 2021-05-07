import { Pipe, PipeTransform } from '@angular/core';

export interface Dictionary<TEntity> {
    [key: string]: TEntity
}

@Pipe({ name: 'dictionary' })
export class DictionaryPipe implements PipeTransform {
    transform(value, args: string[]): any {
        let dictionary = [];
        for (let key in value) {
            dictionary.push({ key: key, value: value[key] });
        }
        return dictionary;
    }
}