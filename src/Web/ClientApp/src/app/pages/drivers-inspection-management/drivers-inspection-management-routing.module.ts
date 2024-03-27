import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DriverInspectionsListComponent } from './driver-inspections-list/driver-inspections-list.component';
import { DriverInspectionFormComponent } from './driver-inspection-form/driver-inspection-form.component';

const routes: Routes = [
  { path: '', component: DriverInspectionsListComponent },
  { path: 'upsert/:id', component: DriverInspectionFormComponent },
  { path: 'upsert', component: DriverInspectionFormComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class DriversInspectionManagementRoutingModule {}
