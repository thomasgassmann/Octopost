import { Component, OnInit, ViewEncapsulation, ViewChild, ElementRef } from '@angular/core';
import { MatDialogRef, MatTabGroup } from '@angular/material';
import { FileService } from '../../services/file.service';
import { SnackbarService } from '../../services/snackbar.service';

@Component({
  selector: 'app-create-file',
  templateUrl: './create-file.component.html',
  styleUrls: ['./create-file.component.css'],
  encapsulation: ViewEncapsulation.None
})
export class CreateFileComponent implements OnInit {
  private _isLoading = false;
  @ViewChild('fileInput') private fileInput: ElementRef;
  @ViewChild('tabGroup') private tabGroup: MatTabGroup;
  public UrlInput = '';

  constructor(public dialogRef: MatDialogRef<CreateFileComponent>,
    private fileService: FileService,
    private snackbarService: SnackbarService) { }

  public get isLoading(): boolean {
    return this._isLoading;
  }

  public get otherFileName(): string {
    if (this.fileInput.nativeElement.files && this.fileInput.nativeElement.files[0]) {
      return this.fileInput.nativeElement.files[0].name;
    } else {
      return 'No file selected';
    }
  }

  public ngOnInit(): void {
    this.tabGroup.selectedIndexChange.subscribe(x => {
      const index = this.tabGroup.selectedIndex;
    });
    this.tabGroup.selectedIndex = 3;
  }

  public async save(): Promise<void> {
    this._isLoading = true;
    await this.createFile().then(x => {
      this._isLoading = false;
      this.dialogRef.close(x);
    }).catch(x => {
      this._isLoading = false;
      this.snackbarService.showMessage(x);
    });
  }

  public cancel(): void {
    this.dialogRef.close(-1);
  }

  public selectFile(): void {
    this.fileInput.nativeElement.click();
  }

  private async createFile(): Promise<number> {
    switch (this.tabGroup.selectedIndex) {
      case 0:
        return Promise.resolve(-1);
      case 1:
        return Promise.resolve(-1);
      case 2:
        return Promise.resolve(-1);
      case 3:
        if (this.fileInput.nativeElement.files && this.fileInput.nativeElement.files[0]) {
          const result = await this.fileService.addFile(this.fileInput.nativeElement.files[0]);
          if (result !== undefined) {
            return result.createdId;
          } else {
            const message = 'Something went wrong. The file may be too large';
            this.snackbarService.showMessage(message);
            throw new Error(message);
          }
        } else {
          throw new Error('No file selected');
        }
      case 4:
        const result = await this.fileService.addLinkedFile(this.UrlInput);
        if (result !== undefined) {
          return result.createdId;
        } else {
          const message = 'Please provide a valid URL';
          this.snackbarService.showMessage(message);
          throw new Error(message);
        }
      default:
        this.snackbarService.showMessage('What the fuck?');
        throw new Error('What the fuck');
    }
  }
}
