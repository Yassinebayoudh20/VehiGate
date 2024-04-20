import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { VehiclesInspectionManagementRoutingModule } from './vehicles-inspection-management-routing.module';
import { VehicleInspectionsListComponent } from './vehicle-inspections-list/vehicle-inspections-list.component';
import { VehicleInspectionFormComponent } from './vehicle-inspection-form/vehicle-inspection-form.component';
import { ButtonModule } from 'primeng/button';
import { InputTextareaModule } from 'primeng/inputtextarea';
import { SharedModule } from 'src/app/shared/shared.module';
import { TranslocoModule } from '@ngneat/transloco';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RadioButtonModule } from 'primeng/radiobutton';


const PRIME_UI_MODULES = [ButtonModule, InputTextareaModule,RadioButtonModule];

@NgModule({
  declarations: [VehicleInspectionsListComponent, VehicleInspectionFormComponent],
  imports: [CommonModule, VehiclesInspectionManagementRoutingModule, SharedModule,FormsModule, TranslocoModule, ReactiveFormsModule, ...PRIME_UI_MODULES],
})
export class VehiclesInspectionManagementModule {}
