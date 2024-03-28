import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { VehiclesInspectionManagementRoutingModule } from './vehicles-inspection-management-routing.module';
import { VehicleInspectionsListComponent } from './vehicle-inspections-list/vehicle-inspections-list.component';
import { VehicleInspectionFormComponent } from './vehicle-inspection-form/vehicle-inspection-form.component';


@NgModule({
  declarations: [
    VehicleInspectionsListComponent,
    VehicleInspectionFormComponent
  ],
  imports: [
    CommonModule,
    VehiclesInspectionManagementRoutingModule
  ]
})
export class VehiclesInspectionManagementModule { }
