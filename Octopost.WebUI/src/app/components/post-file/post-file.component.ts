import { Component, OnInit, ViewEncapsulation, Input } from '@angular/core';
import { Post } from '../../model/post.model';
import { File } from '../../model/file.model';
import { FileDialogComponent } from '../file-dialog';
import { MatDialog } from '@angular/material';

@Component({
  selector: 'app-post-file',
  templateUrl: './post-file.component.html',
  styleUrls: ['./post-file.component.css'],
  encapsulation: ViewEncapsulation.None
})
export class PostFileComponent {
  private _post: Post = undefined;

  constructor(private dialog: MatDialog) { }

  @Input('post')
  public set post(value: Post) {
    this._post = value;
  }

  public get file(): File {
    return this._post.file;
  }

  public clicked(): void {
    const dialogRef = this.dialog.open(FileDialogComponent, {
      width: '70%',
      data: {
        file: this.file
      }
    });
  }
}
