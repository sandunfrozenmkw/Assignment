import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { HttpService } from './http.service';
import { catchError } from 'rxjs/operators';
import { ILoginModel } from '../models/loginModel';

const AUTH_API = 'http://localhost:8080/api/auth/';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  constructor(private http: HttpClient) { }

  // public Login(request: ILoginModel): Observable<any> {
  //   return this.http.postAnonymous<any, ILoginModel>('api/User/UserLogin', request).pipe(
  //     catchError(this.handleError)
  //   );
  // }


  login(request: ILoginModel): Observable<any> {
    return this.http.post(environment.baseApiUrl + 'api/User/UserLogin', 
    request, httpOptions);
  }

  // register(username: string, email: string, password: string): Observable<any> {
  //   return this.http.post(AUTH_API + 'signup', {
  //     username,
  //     email,
  //     password
  //   }, httpOptions);
  // }

  private handleError(error: HttpErrorResponse): Observable<any> {
    console.error(error);
    return Observable.throw(error.message || 'Server error');
  }
}
