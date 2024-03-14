import { HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class ErrorHandlingService {
  private errorSubject = new Subject<string>();
  error$ = this.errorSubject.asObservable();

  handleError(error: any): void {
    let errorMessage = 'An error occurred';

    // Check if the error is an instance of HttpErrorResponse and has an error message
    if (error instanceof HttpErrorResponse && error.error && error.error.message) {
      errorMessage = error.error.message;
    } else if (typeof error === 'string') {
      // If the error is a string, use it as the error message
      errorMessage = error;
    } else if (error && error.detail) {
      // Check if the error has a 'detail' property, common in ProblemDetails
      errorMessage = error.detail;
    }

    this.errorSubject.next(errorMessage);
  }

  handleFluentValidationErrors(errors: any): void {
    const errorMessages: string[] = [];

    for (const prop in errors) {
      if (errors.hasOwnProperty(prop)) {
        const errorMessagesForProp = errors[prop];
        if (Array.isArray(errorMessagesForProp)) {
          errorMessages.push(...errorMessagesForProp.map((errorMessage: string) => `${prop}: ${errorMessage}`));
        } else if (typeof errorMessagesForProp === 'string') {
          errorMessages.push(`${prop}: ${errorMessagesForProp}`);
        }
      }
    }

    this.errorSubject.next(errorMessages.join('\n'));
  }
}
