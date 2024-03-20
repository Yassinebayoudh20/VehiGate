import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { SitesListComponent } from './sites-list/sites-list.component';
import { SiteFormComponent } from './site-form/site-form.component';

const routes: Routes = [
  { path: '', component: SitesListComponent },
  { path: 'upsert/:id', component: SiteFormComponent },
  { path: 'upsert', component: SiteFormComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class SitesManagementRoutingModule {}
