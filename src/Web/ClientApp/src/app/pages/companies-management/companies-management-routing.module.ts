import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CompaniesListComponent } from './companies-list/companies-list.component';
import { CompanyFormComponent } from './company-form/company-form.component';

const routes: Routes = [
  { path: '', component: CompaniesListComponent },
  { path: 'upsert/:id', component: CompanyFormComponent },
  { path: 'upsert', component: CompanyFormComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CompaniesManagementRoutingModule { }
