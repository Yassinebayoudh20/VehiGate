import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { PagesRoutingModule } from './pages-routing.module';
import { UsersManagementModule } from './users-management/users-management.module';

@NgModule({
  declarations: [],
  imports: [CommonModule, PagesRoutingModule],
})
export class PagesModule {}
