import { Injectable } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { throwError } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class ErrorHandingService {
  handleError(error: HttpErrorResponse): string {
    const { error: response } = error;

    if (response?.error) return response.error;
    if (response?.errors) {
      const errors = response.errors;
      for (const key in errors) {
        if (
          Object.prototype.hasOwnProperty.call(errors, key) &&
          Array.isArray(errors[key])
        ) {
          const errorMessage = errors[key][0];
          return errorMessage;
        }
      }
    }

    return error.message || 'Unknown error occurred';
  }

  getErrorObservable(error: HttpErrorResponse) {
    const errorMessage = this.handleError(error);
    return throwError(() => new Error(errorMessage));
  }
}
