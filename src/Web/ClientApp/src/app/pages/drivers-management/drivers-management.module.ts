import { DropdownModule } from 'primeng/dropdown';
import { ReactiveFormsModule } from '@angular/forms';
import { SharedModule } from 'src/app/shared/shared.module';
import { TranslocoModule } from '@ngneat/transloco';
import { CoreModule } from 'src/app/core/core.module';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { DriversManagementRoutingModule } from './drivers-management-routing.module';
import { DriversListComponent } from './drivers-list/drivers-list.component';
import { DriverFormComponent } from './driver-form/driver-form.component';
import { DriversComponent } from './drivers.component';

const PRIME_UI_MODULES = [InputTextModule, ButtonModule, DropdownModule];

@NgModule({
  declarations: [DriversComponent, DriversListComponent, DriverFormComponent],
  imports: [CommonModule, DriversManagementRoutingModule, CoreModule, TranslocoModule, SharedModule, ReactiveFormsModule, ...PRIME_UI_MODULES],
})
export class DriversManagementModule {}
