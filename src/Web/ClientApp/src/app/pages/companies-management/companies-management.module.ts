import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CompaniesManagementRoutingModule } from './companies-management-routing.module';
import { CompaniesComponent } from './companies.component';
import { CompaniesListComponent } from './companies-list/companies-list.component';
import { CompanyFormComponent } from './company-form/company-form.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { InputTextModule } from 'primeng/inputtext';
import { ButtonModule } from 'primeng/button';
import { TranslocoModule } from '@ngneat/transloco';
import { ReactiveFormsModule } from '@angular/forms';

const PRIME_UI_MODULES = [InputTextModule, ButtonModule];

@NgModule({
  declarations: [CompaniesComponent, CompaniesListComponent, CompanyFormComponent],
  imports: [CommonModule, SharedModule, CompaniesManagementRoutingModule,ReactiveFormsModule,TranslocoModule, ...PRIME_UI_MODULES],
})
export class CompaniesManagementModule {}
