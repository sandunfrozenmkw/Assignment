import { Component, OnInit } from '@angular/core';
import { UserService } from '../_services/user.service';
import * as XLSX from 'xlsx';
import { AccountsService } from '../_services/accounts.service';
import { IUploadDataModel } from '../models/uploadDataModel';
import { Router } from '@angular/router';


@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  content?: string;
  arrayBuffer: any;
  file: File;
  data!: [][];
  errorMessage = '';
  isLoginFailed = false;
  uploadData: IUploadDataModel[] = [];

  constructor(private accountsService: AccountsService, private router: Router) {

  }

  ngOnInit(): void {

  }

  onFileChange(evt: any) {
    const target: DataTransfer = <DataTransfer>(evt.target);

    if (target.files.length !== 1) throw new Error('Cannot use multiple files');

    const reader: FileReader = new FileReader();

    reader.onload = (e: any) => {
      const bstr: string = e.target.result;
      const wb: XLSX.WorkBook = XLSX.read(bstr, { type: 'binary' });
      const wsname: string = wb.SheetNames[0];
      const ws: XLSX.WorkSheet = wb.Sheets[wsname];
      this.data = (XLSX.utils.sheet_to_json(ws, { header: 1 }));
      let x = this.data.slice(1);

      this.data.forEach(row => {
        let columns: string[] = [];
        row.forEach(cols => {
          if (typeof cols == 'string') {
            columns.push(cols);
          }
          else {
            columns.push(JSON.stringify(cols));
          }
        });
        let roww: IUploadDataModel = new IUploadDataModel();
        roww.DataColumns = columns;
        this.uploadData.push(roww);
      });
    };
    reader.readAsBinaryString(target.files[0]);
  }

  uploadAccountData(evt: any) {
    this.accountsService.uploadData(this.uploadData).subscribe(
      data => {
        alert("Data Successfully Uploaded...!!!");
        this.router.navigate(['admin']);
      },
      err => {
        this.errorMessage = err.error.message;
        this.isLoginFailed = true;
      }
    );
  }

}
