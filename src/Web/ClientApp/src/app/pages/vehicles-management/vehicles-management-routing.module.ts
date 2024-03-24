import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { VehiclesListComponent } from './vehicles-list/vehicles-list.component';
import { VehicleFormComponent } from './vehicle-form/vehicle-form.component';

const routes: Routes = [
  { path: '', component: VehiclesListComponent },
  { path: 'upsert/:id', component: VehicleFormComponent },
  { path: 'upsert', component: VehicleFormComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class VehiclesManagementRoutingModule {}
