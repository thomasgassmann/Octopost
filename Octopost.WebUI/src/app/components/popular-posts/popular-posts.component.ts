import { Component, OnInit, ViewChild } from '@angular/core';
import { Post, PostContainerRequest } from '../../model';
import { FilterPostService } from '../../services';
import { PostContainerComponent } from '../post-container';

@Component({
  selector: 'app-popular-posts',
  templateUrl: './popular-posts.component.html',
  styleUrls: ['./popular-posts.component.css']
})
export class PopularPostsComponent {
  private _isActive = false;
  private _fetchRequest: PostContainerRequest;
  @ViewChild('postContainer') public postContainer: PostContainerComponent;

  constructor(private filterPostService: FilterPostService) {
    this._fetchRequest = this.createRequest();
  }

  public set isActive(value: boolean) {
    this._isActive = value;
  }

  public get isActive(): boolean {
    return this._isActive;
  }

  public get fetchRequest(): PostContainerRequest {
    return this._fetchRequest;
  }

  public createRequest(): PostContainerRequest {
    return new PostContainerRequest(
      (request: PostContainerRequest, page: number, pageSize: number, commentAmount: number) => {
        return request.filterPostService.votes(page, pageSize, commentAmount);
      }, {
      },
      this.filterPostService);
  }

  public async refresh(): Promise<void> {
    await this.postContainer.refresh();
  }
}
