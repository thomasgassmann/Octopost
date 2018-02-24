import { Pipe, PipeTransform } from '@angular/core';

@Pipe({ name: 'postTagName' })
export class PostTagNamePipe implements PipeTransform {
    public transform(value: any, ...args: any[]) {
        if (typeof value !== 'string') {
            throw new Error('Pass a string to this pipe');
        }

        const names = {
            'Arts': 'Arts',
            'Business': 'Business',
            'Computers': 'Computers',
            'Games': 'Games',
            'Health': 'Health',
            'Home': 'Home',
            'Recreation': 'Recreation',
            'Science': 'Science',
            'Society': 'Society',
            'Sports': 'Sports'
          };

        if (names[value] === undefined) {
            throw new Error('Value could not be found');
        }

        return names[value];
    }
}
