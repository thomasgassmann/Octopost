import { Component, OnInit, ViewChild, Injector } from '@angular/core';
import { PostContainerComponent } from '../post-container';
import { FilterPostService } from '../../services/filter-post.service';
import { Post, PostContainerRequest } from '../../model';
import { TagService } from '../../services/tag.service';
import { SnackbarService } from '../../services/snackbar.service';
import { MatChipInputEvent } from '@angular/material';

@Component({
  selector: 'app-tagged-posts',
  templateUrl: './tagged-posts.component.html',
  styleUrls: ['./tagged-posts.component.css']
})
export class TaggedPostsComponent {
  private _selectedValue: string[] = [];
  private _isActive = false;
  private _fetchRequest: PostContainerRequest;
  @ViewChild('postContainer') private postContainer: PostContainerComponent;

  constructor(
    private tagService: TagService,
    private snackbarService: SnackbarService,
    private filterPostService: FilterPostService) {
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

  public set selectedValue(value: string[]) {
    this._selectedValue = value;
    if (value.length === 0) {
      this.postContainer.empty();
      return;
    }

    this._fetchRequest = this.createRequest();
  }

  public get selectedValue(): string[] {
    return this._selectedValue;
  }

  public get labels(): { [id: string]: string } {
    return this.tagService.getAll();
  }

  public get values(): string[] {
    const list = [];
    const all = this.tagService.getAll();
    for (const name in all) {
      if (all.hasOwnProperty(name)) {
        list.push(name);
      }
    }
    return list;
  }

  public remove(value: string): void {
    let index: number;
    if ((index = this.selectedValue.indexOf(value)) > -1) {
      this.selectedValue.splice(index, 1);
      this.selectedValue = this.selectedValue;
    }
  }

  public async refresh(): Promise<void> {
    await this.postContainer.refresh();
  }

  private createRequest(): PostContainerRequest {
    return new PostContainerRequest(
      async (request: PostContainerRequest, page: number, pageSize: number, commentAmount: number) => {
        const tags = <string[]>request.data['selectedTags'];
        const snackbar = <SnackbarService>request.data['snackbarService'];
        if (tags === undefined || tags === null || !tags.length || tags.length === 0) {
          snackbar.showMessage('Please select some tags first');
          return Promise.resolve([]);
        }

        return request.filterPostService.tags(page, pageSize, tags, commentAmount);
      }, {
        'selectedTags': this.selectedValue,
        'snackbarService': this.snackbarService
      },
      this.filterPostService);
  }
}
