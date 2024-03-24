import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { VehiclesManagementRoutingModule } from './vehicles-management-routing.module';
import { VehiclesComponent } from './vehicles.component';
import { VehiclesListComponent } from './vehicles-list/vehicles-list.component';
import { VehicleFormComponent } from './vehicle-form/vehicle-form.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { InputTextModule } from 'primeng/inputtext';
import { ButtonModule } from 'primeng/button';
import { TranslocoModule } from '@ngneat/transloco';
import { ReactiveFormsModule } from '@angular/forms';
import { CalendarModule } from 'primeng/calendar';
import { ToastModule } from 'primeng/toast';

const PRIME_UI_MODULES = [InputTextModule, ButtonModule, CalendarModule, ToastModule];

@NgModule({
  declarations: [VehiclesComponent, VehiclesListComponent, VehicleFormComponent],
  imports: [CommonModule, VehiclesManagementRoutingModule, SharedModule, TranslocoModule, ReactiveFormsModule, ...PRIME_UI_MODULES],
})
export class VehiclesManagementModule {}
