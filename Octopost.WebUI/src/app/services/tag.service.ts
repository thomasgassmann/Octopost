import { Injectable } from '@angular/core';
import { OctopostHttpService } from './octopost-http.service';

@Injectable()
export class TagService {
  private map = {
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

  public getAll(): { [id: string]: string } {
    return this.map;
  }

  public getDescription(value: string): string {
    return this.map[value];
  }

  public getValue(description: string): string {
    let result = description;
    Object.keys(this.map).forEach(key => {
      if (this.map[key] === description) {
        result = key;
      }
    });
    return result;
  }
}
