import { Component, OnInit, ViewEncapsulation, ViewChild } from '@angular/core';
import { PostContainerRequest } from '../../model';
import { PostContainerComponent } from '../post-container';
import { LocationService, FilterPostService } from '../../services';
import { AfterViewInit } from '@angular/core/src/metadata/lifecycle_hooks';

@Component({
  selector: 'app-location-posts',
  templateUrl: './location-posts.component.html',
  styleUrls: ['./location-posts.component.css'],
  encapsulation: ViewEncapsulation.None
})
export class LocationPostsComponent {
  private _myLocation: { lng: number, lat: number };
  private _isActive = false;
  private _fetchRequest: PostContainerRequest;
  @ViewChild('postContainer') private postContainer: PostContainerComponent;

  constructor(private locationService: LocationService, private filterPostService: FilterPostService) {
  }

  public refreshLocation(): void {
    this.locationService.getLocation().then(x => {
      this._myLocation = { lng: x.longitude, lat: x.latitude };
      this._fetchRequest = this.createRequest();
    });
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
        return request.filterPostService.byLocation(page, pageSize, this._myLocation.lat, this._myLocation.lng, commentAmount);
      }, {
      },
      this.filterPostService);
  }
}
