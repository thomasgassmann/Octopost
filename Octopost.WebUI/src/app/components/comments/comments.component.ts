import { Component, OnInit, ViewEncapsulation, ViewChild, Input } from '@angular/core';
import { Post } from '../../model/post.model';
import { Observable } from 'rxjs/Observable';
import { Subscription } from 'rxjs/Subscription';
import 'rxjs/add/observable/interval';
import { Comment } from '../../model/comment.model';
import { MatExpansionPanel } from '@angular/material';
import { FilterCommentService, CreateCommentService, LocationService, SnackbarService } from '../../services';

@Component({
  selector: 'app-comments',
  templateUrl: './comments.component.html',
  styleUrls: ['./comments.component.css'],
  encapsulation: ViewEncapsulation.None
})
export class CommentsComponent implements OnInit {
  private _post: Post = undefined;
  private readonly _previewCommentAmount = 3;
  private readonly _pageSize: number = 100;
  private _currentCommentPreview: Comment;
  private _currentCommentIndex = 0;
  private _page = 0;
  private _isLoading = false;
  private _comments: Comment[] = new Array<Comment>();
  private _commentText: string = '';
  private _hasLoadedComments = false;
  private _lastLoadedComments: Date = new Date();
  private _firstOpening = true;
  private _isOpen = false;
  private _previewTimer: any = undefined;
  private _commentTimer: any = undefined;
  private _previewTimerSubscription: Subscription = undefined;
  private _commentTimerSubscription: Subscription = undefined;
  @ViewChild('expansionPanel') public expansionPanel: MatExpansionPanel;

  constructor(
    private filterCommentService: FilterCommentService,
    private createCommentService: CreateCommentService,
    private locationService: LocationService,
    private snackbarService: SnackbarService) {
    this._previewTimer = Observable.interval(1500);
    this._commentTimer = Observable.interval(2000);
  }

  @Input('post')
  public set post(value: Post) {
    this._post = value;
    this._currentCommentPreview = this._post.comments[0];
  }

  public ngOnInit(): void {
    this.startPreviewTimer();
    this.expansionPanel.opened.subscribe(x => {
      this.startCommentTimer();
      this.opened();
    });
    this.expansionPanel.closed.subscribe(x => {
      this.closed();
    });
  }

  public get commentText(): string {
    return this._commentText;
  }

  public set commentText(value: string) {
    this._commentText = value;
  }

  public get isLoading(): boolean {
    return this._isLoading;
  }

  public get shownComments(): Array<Comment> {
    return this._comments;
  }

  public get currentCommentPreview(): Comment {
    return this._currentCommentPreview;
  }

  public get showLoadMoreButton(): boolean {
    return this._comments.length % this._pageSize === 0;
  }

  public trackByFn(index: number, item: any): number {
    return index;
  }

  public closed(): void {
    this._commentTimerSubscription.unsubscribe();
    this._isOpen = false;
  }

  public async opened(): Promise<void> {
    this._isOpen = true;
    if (this._firstOpening) {
      this._firstOpening = false;
      await this.reloadComments();
    }
  }

  public async reloadComments(): Promise<void> {
    this._comments = new Array<Comment>();
    this._page = 0;
    await this.loadComments();
    this._hasLoadedComments = true;
    this._lastLoadedComments = new Date();
  }

  public async comment(): Promise<void> {
    this._isLoading = true;
    const location = await this.locationService.getLocation();
    const text = this.commentText;
    this.createCommentService.createComment(this._post.id, text, location.latitude, location.longitude).then(createdId => {
      this._isLoading = false;
      this.commentText = '';
      this.snackbarService.showMessage('Commented!');
    }).catch(x => {
      this._isLoading = false;
    });
  }

  public async loadMore(): Promise<void> {
    if (this.showLoadMoreButton && !this.isLoading) {
      await this.loadComments();
    }
  }

  public async loadComments(): Promise<void> {
    this._isLoading = true;
    const results = await this.filterCommentService.getComments(this._post.id, this._page++, this._pageSize);
    this._lastLoadedComments = new Date();
    this._comments.push(...results);
    this._post.comments.push(...results);
    this._isLoading = false;
  }

  private startPreviewTimer(): void {
    this._previewTimerSubscription = this._previewTimer.subscribe(() => {
      if (this._currentCommentIndex >= Math.min(this._post.comments.length, this._previewCommentAmount)) {
        this._currentCommentIndex = 0;
      }

      if (this._comments.length >= this._previewCommentAmount) {
        this._currentCommentPreview = this._comments[this._currentCommentIndex++];
      } else {
        this._currentCommentPreview = this._post.comments[this._currentCommentIndex++];
      }
    });
  }

  private startCommentTimer(): void {
    this._commentTimerSubscription = this._commentTimer.subscribe(() => {
      if (this._hasLoadedComments && this._lastLoadedComments < new Date() && this._isOpen) {
        this.filterCommentService.getCommentsSince(this._lastLoadedComments, this._post.id).then(x => {
          for (const item of x) {
            if (this._comments.filter(j => j.id === item.id).length <= 0) {
              this._comments.unshift(item);
            }
          }
        });
      }
    });
  }
}
