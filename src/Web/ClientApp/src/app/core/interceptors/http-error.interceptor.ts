import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpHandler, HttpRequest, HttpEvent, HttpErrorResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { ErrorHandlingService } from '../services/error-handling.service';

@Injectable()
export class HttpErrorInterceptor implements HttpInterceptor {
  constructor(private errorHandlingService: ErrorHandlingService) {}

  public intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(req).pipe(
      catchError((err) => {
        if (err instanceof HttpErrorResponse && err.error) {
          // Check if the error response is a Blob and its type is 'application/json'
          if (err.error instanceof Blob && err.error.type === 'application/json') {
            return new Promise<any>((resolve, reject) => {
              let reader = new FileReader();
              reader.onload = (e: Event) => {
                try {
                  const errorMessage = JSON.parse((<any>e.target).result);
                  if (errorMessage.error && this.isValidationProblemDetails(errorMessage.error)) {
                    this.handleValidationProblemDetails(errorMessage.error);
                  } else {
                    this.handleProblemDetails(errorMessage);
                  }
                  reject(
                    new HttpErrorResponse({
                      error: errorMessage,
                      headers: err.headers,
                      status: err.status,
                      statusText: err.statusText,
                      url: err.url || undefined,
                    })
                  );
                } catch (e) {
                  reject(err);
                }
              };
              reader.onerror = (e) => {
                reject(err);
              };
              reader.readAsText(err.error);
            });
          } else {
            // Handle other types of error responses
            this.errorHandlingService.handleError(err.error);
          }
        }
        throw err;
      })
    );
  }

  private handleProblemDetails(problemDetails: any): void {
    // Handle ProblemDetails
    const title = problemDetails.title || 'Error';
    const detail = problemDetails.detail || 'An error occurred';
    this.errorHandlingService.handleError(`${title}: ${detail}`);
  }

  private handleValidationProblemDetails(validationProblemDetails: any): void {
    // Handle ValidationProblemDetails
    const errors = validationProblemDetails.errors || {};
    const errorMessages = Object.keys(errors)
      .map((key) => `${key}: ${errors[key]}`)
      .join('\n');
    this.errorHandlingService.handleFluentValidationErrors(errorMessages);
  }

  private isValidationProblemDetails(error: any): boolean {
    // Check if the error object has properties specific to ValidationProblemDetails
    return error.hasOwnProperty('errors') && typeof error.errors === 'object';
  }
}
