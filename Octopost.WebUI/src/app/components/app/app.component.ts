import { Component, ViewChild, OnInit } from '@angular/core';
import { MatDialog, MatTabGroup, MatToolbar } from '@angular/material';
import { CreatePostComponent } from '../create-post';
import { QueryPostsComponent } from '../query-posts';
import { DatePostsComponent } from '../date-posts';
import { TaggedPostsComponent } from '../tagged-posts';
import { PopularPostsComponent } from '../popular-posts';
import { NewestPostsComponent } from '../newest-posts';
import { LocationPostsComponent } from '../location-posts';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  @ViewChild('popular') public popular: PopularPostsComponent;
  @ViewChild('newest') public newest: NewestPostsComponent;
  @ViewChild('byTag') public byTag: TaggedPostsComponent;
  @ViewChild('byDate') public byDate: DatePostsComponent;
  @ViewChild('byQuery') public byQuery: QueryPostsComponent;
  @ViewChild('byLocation') public byLocation: LocationPostsComponent;
  @ViewChild('tabGroup') public tabGroup: MatTabGroup;
  public isRefreshing = false;

  constructor(private dialog: MatDialog) {
  }

  public ngOnInit(): void {
    this.popular.isActive = true;
    this.tabGroup.selectedIndexChange.subscribe(async item => {
      this.setIsActive();
      await this.refresh();
    });
  }

  public showDialog(): void {
    const dialogRef = this.dialog.open(CreatePostComponent, {
      width: '70%'
    });
    dialogRef.afterClosed().subscribe(async result => {
      if (result > 0) {
        await this.refresh();
      }
    });
  }

  public setIsActive(): void {
    this.byTag.isActive = false;
    this.newest.isActive = false;
    this.popular.isActive = false;
    this.byDate.isActive = false;
    this.byQuery.isActive = false;
    this.byLocation.isActive = false;
    switch (this.tabGroup.selectedIndex) {
      case 0:
        this.newest.isActive = true;
        break;
      case 1:
        this.popular.isActive = true;
        break;
      case 2:
        this.byLocation.isActive = true;
        this.byLocation.refreshLocation();
        break;
      case 3:
        this.byTag.isActive = true;
        break;
      case 4:
        this.byDate.isActive = true;
        break;
      case 5:
        this.byQuery.isActive = true;
        break;
    }
  }

  public async refresh(): Promise<void> {
    this.isRefreshing = true;
    switch (this.tabGroup.selectedIndex) {
      case 0:
        await this.newest.refresh();
        break;
      case 1:
        await this.popular.refresh();
        break;
      case 2:
        await this.byLocation.refresh();
        break;
      case 3:
        await this.byTag.refresh();
        break;
      case 4:
        await this.byDate.refresh();
        break;
      case 5:
        await this.byQuery.refresh();
        break;
    }

    this.isRefreshing = false;
  }
}
