import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { VehicleInspectionsListComponent } from './vehicle-inspections-list/vehicle-inspections-list.component';
import { VehicleInspectionFormComponent } from './vehicle-inspection-form/vehicle-inspection-form.component';

const routes: Routes = [
  { path: '', component: VehicleInspectionsListComponent },
  { path: 'upsert', component: VehicleInspectionFormComponent },
  { path: 'upsert/:id', component: VehicleInspectionFormComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class VehiclesInspectionManagementRoutingModule {}
