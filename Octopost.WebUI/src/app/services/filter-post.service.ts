import { Injectable } from '@angular/core';
import { OctopostHttpService } from './octopost-http.service';
import { Post } from '../model';
import * as moment from 'moment';

@Injectable()
export class FilterPostService {

  constructor(private httpService: OctopostHttpService) { }

  public async newest(page: number, pageSize: number, commentAmount: number): Promise<Post[]> {
    const url = `Posts/Newest?page=${page}&pageSize=${pageSize}&comments=${commentAmount}`;
    const result = await this.httpService.getArray(url, Post);
    return result;
  }

  public async votes(page: number, pageSize: number, commentAmount: number): Promise<Post[]> {
    const url = `Posts/Votes?page=${page}&pageSize=${pageSize}&comments=${commentAmount}`;
    const result = await this.httpService.getArray(url, Post);
    return result;
  }

  public async byQuery(query: string, page: number, pageSize: number, commentAmount: number): Promise<Post[]> {
    const url = `Posts/Query?query=${query}&page=${page}&pageSize=${pageSize}&comments=${commentAmount}`;
    const result = await this.httpService.getArray(url, Post);
    return result;
  }

  public async byDate(from: Date, to: Date, page: number, pageSize: number, commentAmount: number): Promise<Post[]> {
    const fromStr = moment(from).format('YYYY-MM-DD');
    const toStr = moment(to).format('YYYY-MM-DD');
    const url = `Posts/Date?from=${fromStr}&to=${toStr}&page=${page}&pageSize=${pageSize}&comments=${commentAmount}`;
    const result = await this.httpService.getArray(url, Post);
    return result;
  }

  public async tags(page: number, pageSize: number, tags: string[], commentAmount: number): Promise<Post[]> {
    const concatenated = tags.join(',');
    const url = `Posts/Tags?page=${page}&pageSize=${pageSize}&tags=${concatenated}&comments=${commentAmount}`;
    const result = await this.httpService.getArray(url, Post);
    return result;
  }

  public async byLocation(page: number, pageSize: number, latitude: number, longitude: number, commentAmount: number): Promise<Post[]> {
    const url = `Posts/Location?page=${page}&pageSize=${pageSize}&lng=${longitude}&lat=${latitude}&comments=${commentAmount}`;
    const result = await this.httpService.getArray(url, Post);
    return result;
  }
}
