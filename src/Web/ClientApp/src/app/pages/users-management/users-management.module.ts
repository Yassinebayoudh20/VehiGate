import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { UsersManagementRoutingModule } from './users-management-routing.module';
import { UsersListComponent } from './users-list/users-list.component';
import { UsersComponent } from './users.component';
import { CoreModule } from 'src/app/core/core.module';


@NgModule({
  declarations: [
    UsersComponent,
    UsersListComponent
  ],
  imports: [
    CommonModule,
    CoreModule,
    UsersManagementRoutingModule
  ]
})
export class UsersManagementModule { }