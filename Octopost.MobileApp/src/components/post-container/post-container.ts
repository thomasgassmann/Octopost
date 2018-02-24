import { Component, Input, OnInit } from '@angular/core';
import { FetchRequest } from '../../model/fetchRequest.model';
import { Post } from '../../model';
import { PostFilterProvider } from '../../providers/index';
import { ToastController } from 'ionic-angular/components/toast/toast-controller';

@Component({
  selector: 'post-container',
  templateUrl: 'post-container.html'
})
export class PostContainerComponent implements OnInit {
  private _fetchRequest: FetchRequest;
  private _isLoading = false;
  private _posts = new Array<Post>();
  private _page = 0;

  constructor(private _filterPostService: PostFilterProvider, private _toastController: ToastController) {
  }

  @Input('fetchRequest')
  public set fetchRequest(value: FetchRequest) {
    this._fetchRequest = value;
  }

  public get fetchRequest(): FetchRequest {
    return this._fetchRequest;
  }

  public get isLoading(): boolean {
    return this._isLoading;
  }

  public get posts(): Array<Post> {
    return this._posts;
  }

  public ngOnInit(): void {
    this.loadMore();
  }

  public trackByFn(index: number, item: any): number {
    return index;
  }

  public async loadMore(): Promise<void> {
    try {
      this._isLoading = true;
      this.fetchRequest.data['page'] = this._page;
      const result = await this.fetchRequest.filterFunc(this._filterPostService, this.fetchRequest.data);
      this._page++;
      this._isLoading = false;
      this._posts.push(...result);
    } catch {
      this._toastController.create({
        message: 'Couldn\'t fetch more posts'
      }).present();
    }
  }
}
