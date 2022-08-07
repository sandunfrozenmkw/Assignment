import { Component, OnInit, ViewChild, HostListener } from '@angular/core';
import { ChartDataSets, ChartOptions, ChartType } from 'chart.js';
import { Color, BaseChartDirective, Label } from 'ng2-charts';
import { IReportDataModel } from '../models/reportDataModel';
import { AccountsService } from '../_services/accounts.service';

@Component({
  selector: 'app-accounts-report',
  templateUrl: './accounts-report.component.html',
  styleUrls: ['./accounts-report.component.css'],
})
export class AccountsReportComponent {

  chartDataReady = false;
  public lineChartData: ChartDataSets[] = [];

  public lineChartLabels: Label[] = [
    'January 2020',
    'February 2020',
    'March 2020',
    'April 2020',
    'June 2020',
    'July 2020',
    'August 2020'
  ];

  public lineChartOptions: ChartOptions = {
    responsive: true,
    maintainAspectRatio: true,
    scales: {
      yAxes: [
        {
          scaleLabel: {
            display: true,
            labelString: 'Account Balance',
          },
          ticks: {
            // maxTicksLimit: 4,
            fontStyle: 'normal',
            fontSize: 13,
            beginAtZero: false,
            callback: (value) => {
              return `$${value.toLocaleString()}`;
            },
          },
          gridLines: {
            drawOnChartArea: false,
            // color: '#676A6C',
          },
        },
      ],
      xAxes: [
        {
          ticks: {
            fontStyle: 'normal',
            fontSize: 13,
            autoSkip: false,
            maxRotation: window.innerWidth < 1100 ? 90 : 0,
            minRotation: window.innerWidth < 1100 ? 90 : 0,
          },
          gridLines: {
            drawOnChartArea: false,
            // color: '#676A6C',
            lineWidth: 1.5,
          },
        },
      ],
    },
    hover: {
      mode: 'nearest',
      intersect: true,
    },
  };

  public lineChartColors: Color[] = [
    {
      backgroundColor: 'white',
      borderColor: 'rgba(148,159,177,1)',
      pointBackgroundColor: 'rgba(148,159,177,1)',
      pointBorderColor: '#fff',
      pointHoverBackgroundColor: '#fff',
      pointHoverBorderColor: 'rgba(148,159,177,0.8)',
    },
  ];

  public lineChartLegend = true;
  public lineChartType: ChartType = 'line';
  errorMessage = '';
  isLoadingFailed = false;
  reportData: IReportDataModel[] = [];

  @ViewChild(BaseChartDirective, { static: true }) chart: BaseChartDirective;


  @HostListener('window:resize', ['$event'])
  onResize() {
  }

  constructor(private accountsService: AccountsService) { }

  ngOnInit(): void {
    this.accountsService.getReport().subscribe(
      data => {

        data.forEach(accountData => {
          var accountValues = accountData.amount.map(i => Number(i));
          var account = accountData.account;

          var chartData = <ChartDataSets>{
            data: accountValues,
            label: account
          }

          this.lineChartData.push(chartData);
        });
        this.chartDataReady = true;
      },
      err => {
        this.errorMessage = "Error Occoured while retriveing data...";
        this.isLoadingFailed = true;
      }
    );

  }


}
