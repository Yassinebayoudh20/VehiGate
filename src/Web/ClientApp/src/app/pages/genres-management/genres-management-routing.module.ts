import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { GenresListComponent } from './genres-list/genres-list.component';
import { VehicleTypeFormComponent } from './genres-form/genres-form.component';

const routes: Routes = [
  { path: '', component: GenresListComponent },
  { path: 'upsert/:id', component: VehicleTypeFormComponent },
  { path: 'upsert', component: VehicleTypeFormComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class GenresManagementRoutingModule {}
