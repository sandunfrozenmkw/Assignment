import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { IUploadDataModel } from '../models/uploadDataModel';
import { ILoginModel } from '../models/loginModel';
import { IAccountDataModel } from '../models/accountDataModel';
import { IReportDataModel } from '../models/reportDataModel';

const API_URL = 'http://localhost:8080/api/test/';
const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};


@Injectable({
  providedIn: 'root'
})
export class AccountsService {
  constructor(private http: HttpClient) { }

  uploadData(request: IUploadDataModel[]): Observable<any> {
    return this.http.post(environment.baseApiUrl + 'api/Account/UploadData', request, httpOptions);
  }

  getGetAccountBalances(): Observable<IAccountDataModel[]> {
    return this.http.get<IAccountDataModel[]>(environment.baseApiUrl + 'api/Account/GetAccountBalances');
  }

  getReport():Observable<IReportDataModel[]>{
    return this.http.get<IReportDataModel[]>(environment.baseApiUrl + 'api/Account/GetReport');
  }
}
