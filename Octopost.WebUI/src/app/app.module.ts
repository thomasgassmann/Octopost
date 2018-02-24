import { BrowserModule } from '@angular/platform-browser';
import { NgModule, ErrorHandler } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FormsModule } from '@angular/forms';
import { GlobalErrorHandler } from './global-error-handler';
import { CookieService, CookieOptions } from 'angular2-cookie/core';
import {
  MatButtonModule,
  MatToolbarModule,
  MatTabsModule,
  MatCardModule,
  MatInputModule,
  MatDialogModule,
  MatSnackBarModule,
  MatProgressSpinnerModule,
  MatChipsModule,
  MatProgressBarModule,
  MatSelectModule,
  MatIconModule,
  MatDatepickerModule,
  MatNativeDateModule,
  MatFormFieldModule,
  MatButtonToggleModule,
  MatExpansionModule,
  MatGridListModule,
  MatListModule
} from '@angular/material';

import * as comp from './components';
import * as serv from './services';
import * as pipe from './pipes';

@NgModule({
  declarations: [
    comp.AppComponent,
    comp.CreatePostComponent,
    comp.PopularPostsComponent,
    comp.NewestPostsComponent,
    comp.TaggedPostsComponent,
    comp.PostContainerComponent,
    comp.PostComponent,
    pipe.PrefixNumberPipe,
    pipe.PostTagNamePipe,
    comp.QueryPostsComponent,
    comp.DatePostsComponent,
    comp.LocationPostsComponent,
    comp.CommentsComponent,
    comp.PostFileComponent,
    comp.FileDialogComponent,
    comp.CreateFileComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    BrowserAnimationsModule,
    MatButtonModule,
    MatToolbarModule,
    MatTabsModule,
    MatCardModule,
    MatInputModule,
    FormsModule,
    MatDialogModule,
    MatSnackBarModule,
    MatProgressSpinnerModule,
    MatChipsModule,
    MatProgressBarModule,
    MatSelectModule,
    MatIconModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatFormFieldModule,
    MatButtonToggleModule,
    MatExpansionModule,
    MatGridListModule,
    MatListModule
  ],
  providers: [
    CookieService,
    serv.CreatePostService,
    serv.OctopostHttpService,
    {
      provide: ErrorHandler,
      useClass: GlobalErrorHandler
    },
    serv.SnackbarService,
    serv.VoteService,
    serv.FilterPostService,
    serv.TagService,
    serv.LocationService,
    serv.FilterCommentService,
    serv.CreateCommentService,
    {
      provide: CookieOptions,
      useValue: {}
    },
    serv.FileService
  ],
  entryComponents: [
    comp.CreatePostComponent,
    comp.FileDialogComponent,
    comp.CreateFileComponent
  ],
  bootstrap: [comp.AppComponent]
})
export class AppModule { }
