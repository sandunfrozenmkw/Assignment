import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { IUploadDataModel } from '../models/uploadDataModel';
import { ILoginModel } from '../models/loginModel';

const API_URL = 'http://localhost:8080/api/test/';
const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};


@Injectable({
  providedIn: 'root'
})
export class AccountsService {
  constructor(private http: HttpClient) { }

  getGetAccountBalances(): Observable<any> {
    return this.http.get(environment.baseApiUrl + 'api/User/GetAccountBalances', { responseType: 'text' });
  }

  uploadData(request: IUploadDataModel[]): Observable<any> {
    return this.http.post(environment.baseApiUrl + 'api/User/UploadData', request, httpOptions);
  }

}
