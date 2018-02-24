import { Component, OnInit, Input, OnDestroy, ViewChild } from '@angular/core';
import { Post } from '../../model';
import {
  VoteService,
  SnackbarService,
  LocationService,
  FilterCommentService,
  CreateCommentService,
  OctopostHttpService
} from '../../services';
import { GlobalErrorHandler } from '../../global-error-handler';

@Component({
  selector: 'app-post',
  templateUrl: './post.component.html',
  styleUrls: ['./post.component.css']
})
export class PostComponent {
  private _internalPost: Post;
  private _hasVoted = false;
  private _address: string;
  private _index: number;

  constructor(
    private httpService: OctopostHttpService,
    private voteService: VoteService,
    private snackbarService: SnackbarService) {
  }

  @Input()
  public set post(value: Post) {
    this._internalPost = value;
  }

  @Input()
  public set index(value: number) {
    this._index = value;
  }

  public get isAudio(): boolean {
    if (this._internalPost && this._internalPost.file && this._internalPost.file.contentType) {
      return this._internalPost.file.contentType.startsWith('audio');
    }
    return false;
  }

  public get fileUrl(): string {
    if (this._internalPost && this._internalPost.file) {
      return this._internalPost.file.url;
    }
    return '';
  }

  public get post(): Post {
    return this._internalPost;
  }

  public get voted(): boolean {
    return this.voteService.hasVoted(this.post.id);
  }

  public async downvote(): Promise<void> {
    await this.vote((service, id) => service.downvote(id), 'Downvoted post!', -1);
  }

  public async upvote(): Promise<void> {
    await this.vote((service, id) => service.upvote(id), 'Upvoted post!', 1);
  }

  private async vote(voteAction: (service: VoteService, id: number) => Promise<number>, text: string, addValue: number): Promise<void> {
    if (!this._hasVoted) {
      this._hasVoted = true;
      this.post.voteCount += addValue;
      await voteAction(this.voteService, this.post.id);
      this.snackbarService.showMessage(text);
    }
  }
}
