import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DriverFormComponent } from './driver-form/driver-form.component';
import { DriversListComponent } from './drivers-list/drivers-list.component';

const routes: Routes = [
  { path: '', component:DriversListComponent },
  { path: 'upsert/:id', component: DriverFormComponent },
  { path: 'upsert', component: DriverFormComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class DriversManagementRoutingModule { }
