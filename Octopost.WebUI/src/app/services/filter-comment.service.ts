import { Injectable } from '@angular/core';
import { OctopostHttpService } from './octopost-http.service';
import { Comment } from '../model/comment.model';
import * as moment from 'moment';

@Injectable()
export class FilterCommentService {

  constructor(private httpService: OctopostHttpService) { }

  public async getComments(postId: number, page: number, pageSize: number): Promise<Comment[]> {
    const url = `Posts/${postId}/Comments?page=${page}&pageSize=${pageSize}`;
    const result = await this.httpService.getArray(url, Comment);
    return result;
  }

  public async getCommentsSince(date: Date, postId: number): Promise<Comment[]> {
    const formattedDate = moment(date).subtract(1, 'h').format('YYYY-MM-DDTHH:mm:ss.SSS');
    const url = `Posts/${postId}/Comments/Date?since=${formattedDate}`;
    const result = await this.httpService.getArray(url, Comment);
    return result;
  }
}
