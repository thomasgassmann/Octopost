import { Component, OnInit, ViewChild } from '@angular/core';
import { PostContainerComponent } from '../post-container';
import { FilterPostService } from '../../services/filter-post.service';
import { Post, PostContainerRequest } from '../../model';

@Component({
  selector: 'app-newest-posts',
  templateUrl: './newest-posts.component.html',
  styleUrls: ['./newest-posts.component.css']
})
export class NewestPostsComponent {
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
        return request.filterPostService.newest(page, pageSize, commentAmount);
      }, {
      },
      this.filterPostService);
  }

  public async refresh(): Promise<void> {
    await this.postContainer.refresh();
  }
}
