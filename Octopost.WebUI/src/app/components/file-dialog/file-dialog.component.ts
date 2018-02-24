import { Component, OnInit, ViewEncapsulation, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';
import { File } from '../../model';
import { OctopostHttpService } from '../../services/octopost-http.service';

@Component({
  selector: 'app-file-dialog',
  templateUrl: './file-dialog.component.html',
  styleUrls: ['./file-dialog.component.css'],
  encapsulation: ViewEncapsulation.None
})
export class FileDialogComponent {
  private _file: File;
  private _fileId: number;

  constructor(
    public dialogRef: MatDialogRef<FileDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private httpService: OctopostHttpService) {
    this._file = new File();
    this._file.contentType = data.file.contentType;
    this._file.fileName = data.file.fileName;
    this._file.created = data.file.created;
    this._file.id = data.file.id;
    this._file.link = data.file.link;
  }

  public get fileUrl(): string {
    return this._file.url;
  }

  public get file(): File {
    return this._file;
  }

  public get isImage(): boolean {
    return this._file.contentType.startsWith('image/');
  }

  public get isVideo(): boolean {
    return this._file.contentType.startsWith('video/');
  }

  public get isAudio(): boolean {
    return this._file.contentType.startsWith('audio/');
  }

  public get isOther(): boolean {
    return !this.isAudio && !this.isVideo && !this.isImage;
  }

  public close(): void {
    this.dialogRef.close(-1);
  }
}
