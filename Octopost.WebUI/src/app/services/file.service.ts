import { Injectable } from '@angular/core';
import { OctopostHttpService } from './octopost-http.service';
import { CreatedResult } from '../model/index';
import { HttpClient } from '@angular/common/http';
import { File } from '../model/file.model';

@Injectable()
export class FileService {

  constructor(private httpService: OctopostHttpService, private httpClient: HttpClient) { }

  public async addFile(fileToUpload: any): Promise<CreatedResult> {
    const input = new FormData();
    input.append('file', fileToUpload);
    const response = await this.httpService.post('Files', input, CreatedResult);
    return response;
  }

  public async addLinkedFile(link: string): Promise<CreatedResult> {
    const url = 'Files/Linked';
    const reply = await this.httpService.post(url, {
      link: link
    }, CreatedResult);
    return reply;
  }

  public async getFile(fileId: number): Promise<File> {
    const url = `Files/${fileId}/Info`;
    return this.httpService.get(url, File);
  }
}
