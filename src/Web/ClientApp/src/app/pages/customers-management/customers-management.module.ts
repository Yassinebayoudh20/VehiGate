import { CustomersComponent } from './customers.component';
import { CustomerFormComponent } from './customer-form/customer-form.component';
import { ReactiveFormsModule } from '@angular/forms';
import { SharedModule } from 'src/app/shared/shared.module';
import { TranslocoModule } from '@ngneat/transloco';
import { CoreModule } from 'src/app/core/core.module';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CustomersManagementRoutingModule } from './customers-management-routing.module';
import { CustomersListComponent } from './customers-list/customers-list.component';

const PRIME_UI_MODULES = [InputTextModule, ButtonModule];

@NgModule({
  declarations: [CustomerFormComponent, CustomersListComponent, CustomersComponent],
  imports: [CommonModule, CustomersManagementRoutingModule, CoreModule, TranslocoModule, SharedModule, ReactiveFormsModule, ...PRIME_UI_MODULES],
})
export class CustomersManagementModule {}
