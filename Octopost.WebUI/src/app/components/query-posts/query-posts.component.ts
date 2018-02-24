import { Component, OnInit, ViewEncapsulation, ViewChild } from '@angular/core';
import { PostContainerRequest } from '../../model';
import { PostContainerComponent } from '../post-container';
import { FilterPostService } from '../../services';

@Component({
  selector: 'app-query-posts',
  templateUrl: './query-posts.component.html',
  styleUrls: ['./query-posts.component.css'],
  encapsulation: ViewEncapsulation.None
})
export class QueryPostsComponent {
  private _query = '';
  private _isActive = false;
  private _fetchRequest: PostContainerRequest;
  @ViewChild('postContainer') private postContainer: PostContainerComponent;

  constructor(
    private filterPostService: FilterPostService) {
    this._fetchRequest = this.createRequest();
  }

  public get query(): string {
    return this._query;
  }

  public set query(value: string) {
    this._query = value;
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

  public async refresh(): Promise<void> {
    await this.postContainer.refresh();
  }

  private createRequest(): PostContainerRequest {
    return new PostContainerRequest(
      async (request: PostContainerRequest, page: number, pageSize: number, commentAmount: number) => {
        const query = <string>request.data['query'];
        if (query.length < 2) {
          return Promise.resolve([]);
        }

        return request.filterPostService.byQuery(query, page, pageSize, commentAmount);
      }, {
        'query': this._query
      },
      this.filterPostService);
  }
}
