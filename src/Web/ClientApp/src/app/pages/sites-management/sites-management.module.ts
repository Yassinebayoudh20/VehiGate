import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SitesManagementRoutingModule } from './sites-management-routing.module';
import { SitesComponent } from './sites.component';
import { SitesListComponent } from './sites-list/sites-list.component';
import { SiteFormComponent } from './site-form/site-form.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { TranslocoModule } from '@ngneat/transloco';
import { InputTextModule } from 'primeng/inputtext';
import { ButtonModule } from 'primeng/button';
import { ReactiveFormsModule } from '@angular/forms';

const PRIME_UI_MODULES = [InputTextModule, ButtonModule];

@NgModule({
  declarations: [SitesComponent, SitesListComponent, SiteFormComponent],
  imports: [CommonModule, SharedModule, TranslocoModule,ReactiveFormsModule, SitesManagementRoutingModule, ...PRIME_UI_MODULES],
})
export class SitesManagementModule {}
