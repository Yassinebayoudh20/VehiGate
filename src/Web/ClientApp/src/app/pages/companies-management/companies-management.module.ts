import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CompaniesManagementRoutingModule } from './companies-management-routing.module';
import { CompaniesComponent } from './companies.component';
import { CompaniesListComponent } from './companies-list/companies-list.component';
import { CompanyFormComponent } from './company-form/company-form.component';
import { SharedModule } from 'src/app/shared/shared.module';


@NgModule({
  declarations: [
    CompaniesComponent,
    CompaniesListComponent,
    CompanyFormComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    CompaniesManagementRoutingModule
  ]
})
export class CompaniesManagementModule { }
