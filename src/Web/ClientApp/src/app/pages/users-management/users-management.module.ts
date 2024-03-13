import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { UsersManagementRoutingModule } from './users-management-routing.module';
import { UsersListComponent } from './users-list/users-list.component';
import { UsersComponent } from './users.component';
import { CoreModule } from 'src/app/core/core.module';
import { SharedModule } from 'src/app/shared/shared.module';
import { UserFormComponent } from './user-form/user-form.component';
import { DropdownModule } from 'primeng/dropdown';
import { InputTextModule } from 'primeng/inputtext';
import { ButtonModule } from 'primeng/button';
import { PasswordModule } from 'primeng/password';
import { TranslocoModule } from '@ngneat/transloco';
import { ReactiveFormsModule } from '@angular/forms';

const PRIME_UI_MODULES = [DropdownModule, InputTextModule, ButtonModule, PasswordModule];

@NgModule({
  declarations: [UsersComponent, UsersListComponent, UserFormComponent],
  imports: [CommonModule, CoreModule, TranslocoModule, SharedModule,ReactiveFormsModule, UsersManagementRoutingModule, ...PRIME_UI_MODULES],
})
export class UsersManagementModule {}
