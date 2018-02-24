import { ErrorHandler, Injectable, Injector} from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { MatSnackBar } from '@angular/material';
import { AppComponent } from './components/index';
import { BadRequest } from './model';
import { SnackbarService } from './services/snackbar.service';

@Injectable()
export class GlobalErrorHandler implements ErrorHandler {

  constructor(private snackBarService: SnackbarService) {
  }

  public handleError(error: any): void {
    const response = error.rejection;
    const errorResponse = <HttpErrorResponse>response;
    if (errorResponse) {
      const responseText = <string>errorResponse.error;
      try {
        const responseObj = <BadRequest>JSON.parse(responseText);
        this.snackBarService.showMessage(responseObj.message);
      } catch (ex) {
      }
    } else {
      console.dir(error);
      this.snackBarService.showMessage('Catastrophic failure!!!');
    }
  }
}
