import { Injectable, OnInit } from '@angular/core';
import { OctopostHttpService } from './octopost-http.service';
import { CreatedResult } from '../model/created-result.model';
import { CookieService } from 'angular2-cookie/core';
import { CookieOptions } from 'angular2-cookie/services';
import * as moment from 'moment';

@Injectable()
export class VoteService {
  private readonly downVote = -1;
  private readonly upVote = 1;
  private votedPosts: number[] = new Array<number>();

  constructor(private httpService: OctopostHttpService,
              private cookieService: CookieService) {
    const cookie = this.cookieService.getObject('voted');
    this.votedPosts = cookie ? <number[]>cookie : new Array<number>();
  }

  public async upvote(postId: number): Promise<number> {
    return await this.vote(this.upVote, postId);
  }

  public async downvote(postId: number): Promise<number> {
    return await this.vote(this.downVote, postId);
  }

  public hasVoted(postId: number): boolean {
    return this.votedPosts.indexOf(postId) > -1;
  }

  private async vote(value: number, postId: number): Promise<number> {
    const url = `Posts/${postId}/Votes?state=${value}`;
    const result = await this.httpService.post(url, {}, CreatedResult);
    this.votedPosts.push(postId);
    this.cookieService.putObject('voted', this.votedPosts, new CookieOptions({
      expires: moment(new Date()).add(10000, 'd').toDate()
    }));
    return result.createdId;
  }
}
