import { CustomersListComponent } from './customers-list/customers-list.component';
import { CustomerFormComponent } from './customer-form/customer-form.component';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  { path: '', component: CustomersListComponent },
  { path: 'upsert/:id', component: CustomerFormComponent },
  { path: 'upsert', component: CustomerFormComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CustomersManagementRoutingModule { }
