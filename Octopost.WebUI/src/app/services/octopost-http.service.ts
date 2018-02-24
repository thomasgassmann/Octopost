import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import 'rxjs/add/operator/toPromise';
import 'rxjs/add/operator/map';
import { SnackbarService } from './snackbar.service';
import * as moment from 'moment';
import { Comment, File, BadRequest } from '../model';

declare type ConstructorType = { new(): any };

@Injectable()
export class OctopostHttpService {
  private readonly baseUri: string = '/octopost/api/';
  private readonly propertyAdapters: { propertyName: string, constructor: ConstructorType, isArray: boolean }[] = [];

  constructor(private httpClient: HttpClient, private snackbarService: SnackbarService) {
    this.registerAdapters();
  }

  public get BaseUri(): string {
    return this.baseUri;
  }

  public async get<T>(uri: string, type: { new(): T }): Promise<T> {
    const combinedUri = this.baseUri + uri;
    const result = await this.httpClient.get(combinedUri).toPromise();
    return <T>result;
  }

  public async getArray<T>(uri: string, type: { new(): T }): Promise<T[]> {
    const combinedUri = this.baseUri + uri;
    const resultObject = await this.handleRequest(
      () => this.httpClient.get(combinedUri).toPromise(),
      type,
      true);
    return <T[]>resultObject;
  }

  public async post<T>(uri: string, body: any, type: { new(): T }): Promise<T> {
    const combinedUri = this.baseUri + uri;
    const resultObject = await this.handleRequest(
      () => this.httpClient.post(combinedUri, body).toPromise(),
      type,
      false);
    return <T>resultObject;
  }

  public async handleRequest<T>(request: () => Promise<Object>, typeConstructor: { new(): T }, isArray: boolean): Promise<T | T[]> {
    try {
      const response = await request();
      if (isArray) {
        const array = <any[]>response;
        const resultArray = [];
        for (const i of array) {
          const parsedResult = this.copyProperties<T>(i, typeConstructor);
          resultArray.push(parsedResult);
        }

        return resultArray;
      } else {
        const result = this.copyProperties<T>(response, typeConstructor);
        return result;
      }
    } catch (error) {
      switch (error.status) {
        case 500:
          this.snackbarService.showMessage(error.message, 7000);
          break;
        case 400:
          const response = <BadRequest>error.error;
          this.snackbarService.showMessage(response.message);
          break;
        case 401:
          this.snackbarService.showMessage('You\'re not authorized to access this resource');
          break;
        case 403:
          this.snackbarService.showMessage('Forbidden');
          break;
      }
    }
  }

  public copyProperties<T>(source: any, target: { new(): T }): T {
    if (source === null || source === undefined) {
      return null;
    }

    const result = new target();
    for (const key of Object.keys(result)) {
      if (source.hasOwnProperty(key)) {
        if (this.adapterExists(key)) {
          const constructor = this.getConstructor(key);
          const isArray = this.getIsArray(key);
          if (isArray) {
            result[key] = [];
            for (const item of source[key]) {
              const created = this.copyProperties(item, constructor);
              result[key].push(created);
            }
          } else {
            const created = this.copyProperties(source[key], constructor);
            result[key] = created;
          }
          continue;
        }

        const date = moment(source[key]);
        if (date.isValid() && typeof source[key] === 'string' && source[key].indexOf(' ') < 0 && key !== 'text') {
          result[key] = date.toDate();
        } else {
          result[key] = source[key];
        }
      }
    }

    return result;
  }

  private adapterExists(propertyName: string): boolean {
    return this.propertyAdapters.map(x => x.propertyName).indexOf(propertyName) > -1;
  }

  private getConstructor(propertyName: string): ConstructorType {
    return this.propertyAdapters.find(x => x.propertyName === propertyName).constructor;
  }

  private getIsArray(propertyName: string): boolean {
    return this.propertyAdapters.find(x => x.propertyName === propertyName).isArray;
  }

  private registerAdapters(): void {
    this.propertyAdapters.push({
      propertyName: 'comments',
      constructor: Comment,
      isArray: true
    });
    this.propertyAdapters.push({
      propertyName: 'file',
      constructor: File,
      isArray: false
    });
  }
}
