import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { RegisterComponent } from './register/register.component';
import { LoginComponent } from './login/login.component';
import { AccountsUploadComponent } from './accounts-upload/accounts-upload.component';
import { ProfileComponent } from './profile/profile.component';
import { AccountsDisplayComponent } from './accounts-display/accounts-display.component';
import { AccountsReportComponent } from './accounts-report/accounts-report.component';

const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'profile', component: ProfileComponent },

  { path: 'upload', component: AccountsUploadComponent },
  { path: 'display', component: AccountsDisplayComponent },
  { path: 'reports', component: AccountsReportComponent },

  { path: '', redirectTo: 'login', pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
