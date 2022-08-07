import { Component, OnInit } from '@angular/core';
import { IAccountDataModel } from '../models/accountDataModel';
import { AccountsService } from '../_services/accounts.service';

@Component({
  selector: 'app-accounts-display',
  templateUrl: './accounts-display.component.html',
  styleUrls: ['./accounts-display.component.css']
})
export class AccountsDisplayComponent implements OnInit {
  errorMessage = '';
  isLoadingFailed = false;
  accountData : IAccountDataModel[] =[];
  year: number = new Date().getFullYear();
  monthNames = ["January", "February", "March", "April", "May", "June",
  "July", "August", "September", "October", "November", "December"
  ];
  monthName = "";


  constructor(private accountsService: AccountsService) { }

  ngOnInit(): void {
    const d = new Date();
    this.monthName = this.monthNames[d.getMonth()];
    this.getAccountData();
  }

  getAccountData() {
    this.accountsService.getGetAccountBalances().subscribe(
      data => {
        this.accountData = data;
        console.log(data);
      },
      err => {
        this.errorMessage = "Error Occoured while retriveing data...";
        this.isLoadingFailed = true;
      }
    );
  }
}
