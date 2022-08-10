import { Inject, Injectable, LOCALE_ID } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class HttpService {

  private baseUri = environment.baseApiUrl;
  constructor(private http: HttpClient, @Inject(LOCALE_ID) private language: string) {
  }

  public get<T>(uri: string): Observable<T> {
    return this.http.get<T>(`${this.baseUri}${uri}`, { withCredentials: true }).pipe(
      catchError(this.handleError)
    );
  }

  public getAnonymous<T>(uri: string): Observable<T> {
    var reqHeader = new HttpHeaders({ 'No-Auth': 'True' });
    return this.http.get<T>(`${this.baseUri}${uri}`, { headers: reqHeader }).pipe(
      catchError(this.handleError)
    );
  }

  public post<T, E>(uri: string, body: E): Observable<T> {
    return this.http.post<T>(`${this.baseUri}${uri}`, body, { withCredentials: true }).pipe(
      catchError(this.handleError)
    );
  }
  public postAnonymous<T, E>(uri: string, body: E): Observable<T> {
    var reqHeader = new HttpHeaders({ 'No-Auth': 'True' });
    return this.http.post<T>(`${this.baseUri}${uri}`, body, { headers: reqHeader }).pipe(
      catchError(this.handleError)
    );
  }

  private handleError(error: HttpErrorResponse) {
    if (error.error instanceof ErrorEvent) {
      console.error('An error occurred:', error.error.message);
    } else {
      console.error(
        `Backend returned code ${error.status}, ` +
        `body was: ${error.error}`);
    }
    return throwError(error);
  };
}



