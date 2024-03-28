import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { DriversInspectionManagementRoutingModule } from './drivers-inspection-management-routing.module';
import { DriverInspectionsComponent } from './driver-inspections.component';
import { DriverInspectionsListComponent } from './driver-inspections-list/driver-inspections-list.component';
import { DriverInspectionFormComponent } from './driver-inspection-form/driver-inspection-form.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { TranslocoModule } from '@ngneat/transloco';
import { ReactiveFormsModule } from '@angular/forms';
import { ButtonModule } from 'primeng/button';
import { InputTextareaModule } from 'primeng/inputtextarea';

const PRIME_UI_MODULES = [ButtonModule,InputTextareaModule];

@NgModule({
  declarations: [DriverInspectionsComponent, DriverInspectionsListComponent, DriverInspectionFormComponent],
  imports: [CommonModule, DriversInspectionManagementRoutingModule, SharedModule, TranslocoModule, ReactiveFormsModule, ...PRIME_UI_MODULES],
})
export class DriversInspectionManagementModule {}
