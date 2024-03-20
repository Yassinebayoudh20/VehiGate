import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AppLayoutComponent } from '../layout/app.layout.component';

const routes: Routes = [
  {
    path: '',
    component: AppLayoutComponent,
    children: [
      {
        path: 'users',
        loadChildren: () => import('./users-management/users-management.module').then((m) => m.UsersManagementModule),
      },
      {
        path: 'drivers',
        loadChildren: () => import('./drivers-management/drivers-management.module').then((m) => m.DriversManagementModule),
      },
      // {
      //   path: 'vehicles',
      //   loadChildren: () => import('./vehicles-management/vehicles-management.module').then((m) => m.VehiclesManagementModule),
      // },
      // {
      //   path: 'sites',
      //   loadChildren: () => import('./sites-management/sites-management.module').then((m) => m.SitesManagementModule),
      // },
      // {
      //   path: 'genres',
      //   loadChildren: () => import('./genres-management/genres-management.module').then((m) => m.GenresManagementModule),
      // },
      {
        path: 'customers',
        loadChildren: () => import('./customers-management/customers-management.module').then((m) => m.CustomersManagementModule),
      },
      // {
      //   path: 'drivers-inspection',
      //   loadChildren: () => import('./drivers-inspection-management/drivers-inspection-management.module').then((m) => m.DriversInspectionManagementModule),
      // },
      // {
      //   path: 'vehicles-inspection',
      //   loadChildren: () => import('./vehicles-inspection-management/vehicles-inspection-management.module').then((m) => m.VehiclesInspectionManagementModule),
      // },
      // {
      //   path: 'checking',
      //   loadChildren: () => import('./checkings-management/checkings-management.module').then((m) => m.CheckingsManagementModule),
      // },
      {
        path: 'companies',
        loadChildren: () => import('./companies-management/companies-management.module').then((m) => m.CompaniesManagementModule),
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class PagesRoutingModule {}
