import { Injectable } from "@angular/core";
import { Observable, throwError } from "rxjs";

@Injectable({
    providedIn: 'root'
})

export class ErrorHandlerService {
    constructor() { }

    handlerError(error: any): Observable<never> {
        let errorMessage = 'An unknown error occurred';
        if (error.error instanceof ErrorEvent) {
            errorMessage = `Client-side error: ${error.error.message}`;
        } else {
            errorMessage = error.error.message;
            if (error.error.detail) {
                errorMessage += ` - ${error.error.detail}`;
            } else if (typeof error.error === 'string') {
                errorMessage = error.error;
            }
        }
        console.error(errorMessage);
        return throwError(() => new Error(errorMessage));
    }

}