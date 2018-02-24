import { Component, OnInit, Input, HostListener, ChangeDetectorRef } from '@angular/core';
import { Post, PostContainerRequest } from '../../model';
import { VoteService, FilterPostService } from '../../services';
import * as moment from 'moment';

@Component({
  selector: 'app-post-container',
  templateUrl: './post-container.component.html',
  styleUrls: ['./post-container.component.css']
})
export class PostContainerComponent implements OnInit {
  private page = 0;
  private readonly pageSize = 10;
  private readonly commentAmount = 3;
  private _endOfList = false;
  private request: PostContainerRequest;
  private posts: Post[] = new Array<Post>();
  private endOfListLoading = false;
  private lastFilter: Date;
  private _isActive = false;

  @Input() public set isActive(value: boolean) {
    this._isActive = value;
  }

  public get nothingToShow(): boolean {
    return this.shownPosts.length === 0 && this._endOfList;
  }

  public async ngOnInit() {
    this.page = 0;
    await this.fetch();
  }

  public async refresh(): Promise<void> {
    this.page = 0;
    this._endOfList = false;
    this.endOfListLoading = false;
    this.posts = new Array<Post>();
    await this.fetch();
  }

  public get shownPosts(): Post[] {
    return this.posts;
  }

  public get endOfList(): boolean {
    return this._endOfList;
  }

  @Input('fetchRequest')
  public set fetchRequest(value: PostContainerRequest) {
    this.request = value;
    this._endOfList = false;
    this.posts = new Array<Post>();
    this.page = 0;
    this.fetch();
  }

  public get fetchRequest(): PostContainerRequest {
    return this.request;
  }

  public async fetch(): Promise<void> {
    if (this._endOfList) {
      return;
    }

    const request = this.request;
    const fetchedPosts = await request.fetchFunction(request, this.page++, this.pageSize, this.commentAmount);
    if (fetchedPosts.length < this.pageSize) {
      this._endOfList = true;
    }
    if (fetchedPosts.length === 0) {
      return;
    }
    for (const fetchedPost of fetchedPosts) {
      if (this.posts.filter(x => x.id === fetchedPost.id).length === 0) {
        this.posts.push(fetchedPost);
      }
    }
  }

  public empty(): void {
    this.posts = [];
  }

  public trackByFn(index: number, item: any): number {
    return index;
  }

  @HostListener('window:scroll', [])
  public async onScroll(): Promise<void> {
    if (!this._isActive) {
      return;
    }

    const last = moment(this.lastFilter);
    const now = moment(new Date());
    const difference = moment.duration(3, 'seconds').asMilliseconds();
    const offset = 50;
    if ((window.innerHeight + window.scrollY + offset) >= document.body.offsetHeight && !this.endOfListLoading) {
      this.endOfListLoading = true;
      await this.fetch();
      this.endOfListLoading = false;
      this.lastFilter = new Date();
    }
  }
}
