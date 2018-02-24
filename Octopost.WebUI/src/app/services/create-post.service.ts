import { Injectable } from '@angular/core';
import { OctopostHttpService } from './octopost-http.service';
import { CreatePost } from '../model';
import { CreatedResult } from '../model/created-result.model';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class CreatePostService {
  constructor(private httpService: OctopostHttpService) {
  }

  public async createPost(createPost: CreatePost): Promise<number> {
    const uri = 'Posts';
    const body = {
      text: createPost.text,
      longitude: createPost.longitude,
      latitude: createPost.latitude
    };
    if (createPost.fileId) {
      body['fileId'] = createPost.fileId;
    }
    const result = await this.httpService.post(uri, body, CreatedResult);
    return result.createdId;
  }
}
