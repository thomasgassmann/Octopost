import { Component, OnInit, ViewEncapsulation, ViewChild } from '@angular/core';
import { PostContainerRequest } from '../../model';
import { PostContainerComponent } from '../post-container';
import { SnackbarService, FilterPostService } from '../../services';
import * as moment from 'moment';

@Component({
  selector: 'app-date-posts',
  templateUrl: './date-posts.component.html',
  styleUrls: ['./date-posts.component.css'],
  encapsulation: ViewEncapsulation.None
})
export class DatePostsComponent {
  private _to: Date;
  private _from: Date;
  private _isActive = false;
  private _fetchRequest: PostContainerRequest;
  @ViewChild('postContainer') private postContainer: PostContainerComponent;

  constructor(
    private snackbarService: SnackbarService,
    private filterPostService: FilterPostService) {
    this._fetchRequest = this.createRequest();
  }

  public set to(value: Date) {
    this._to = value;
    this._fetchRequest = this.createRequest();
  }

  public get to(): Date {
    return this._to;
  }

  public set from(value: Date) {
    this._from = value;
    this._fetchRequest = this.createRequest();
  }

  public get from(): Date {
    return this._from;
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
        const from = <Date>request.data['from'];
        const to = <Date>request.data['to'];
        const snackbar = <SnackbarService>request.data['snackbarService'];
        if (!moment(from).isValid() || !moment(to).isValid()) {
          snackbar.showMessage('Please select to valid dates');
          return Promise.resolve([]);
        }

        if (from > to) {
          snackbar.showMessage('From date cannot be bigger than to date');
          return Promise.resolve([]);
        }

        return request.filterPostService.byDate(from, to, page, pageSize, commentAmount);
      }, {
        'from': this.from,
        'to': this.to,
        'snackbarService': this.snackbarService
      },
      this.filterPostService);
  }
}
