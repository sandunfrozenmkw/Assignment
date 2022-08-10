import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { ILoginModel } from '../models/loginModel';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  constructor(private http: HttpClient) { }

  login(request: ILoginModel): Observable<any> {
    return this.http.post(environment.baseApiUrl + 'api/User/UserLogin',
      request, httpOptions);
  }

  private handleError(error: HttpErrorResponse): Observable<any> {
    console.error(error);
    return Observable.throw(error.message || 'Server error');
  }
}
