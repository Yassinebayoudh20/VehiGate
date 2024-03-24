import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { GenresManagementRoutingModule } from './genres-management-routing.module';
import { GenresComponent } from './genres.component';
import { GenresListComponent } from './genres-list/genres-list.component';
import { VehicleTypeFormComponent } from './genres-form/genres-form.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { InputTextModule } from 'primeng/inputtext';
import { ButtonModule } from 'primeng/button';
import { TranslocoModule } from '@ngneat/transloco';
import { ReactiveFormsModule } from '@angular/forms';

const PRIME_UI_MODULES = [InputTextModule, ButtonModule];

@NgModule({
  declarations: [GenresComponent, GenresListComponent, VehicleTypeFormComponent],
  imports: [CommonModule, GenresManagementRoutingModule, SharedModule, TranslocoModule, ReactiveFormsModule, ...PRIME_UI_MODULES],
})
export class GenresManagementModule {}
