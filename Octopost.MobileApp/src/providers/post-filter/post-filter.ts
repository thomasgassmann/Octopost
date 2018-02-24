import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Post } from '../../model/post.model';

@Injectable()
export class PostFilterProvider {

  constructor(public http: HttpClient) {
  }

  public async getNewest(page: number, pageSize: number, commentSize: number): Promise<Post[]> {
    const result = await this.http.get(`http://thomasgassmann.internet-box.ch/api/Posts/Newest?page=${page}&pageSize=${pageSize}&comments=${commentSize}`).toPromise();
    return <Post[]>result;
  }
}
