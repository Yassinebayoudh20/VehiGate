import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CrudComponent } from './components/crud/crud.component';

import { TableModule } from 'primeng/table';
import { MultiSelectModule } from 'primeng/multiselect';
import { ToolbarModule } from 'primeng/toolbar';
import { FileUploadModule } from 'primeng/fileupload';
import { ToastModule } from 'primeng/toast';

import { FormsModule } from '@angular/forms';
import { TranslocoModule } from '@ngneat/transloco';
import { InputTextModule } from 'primeng/inputtext';


const PRIME_UI_MODULES = [TableModule, MultiSelectModule, ToolbarModule, FileUploadModule, ToastModule,InputTextModule];

@NgModule({
  declarations: [CrudComponent],
  imports: [CommonModule, FormsModule,TranslocoModule, ...PRIME_UI_MODULES],
  exports: [CrudComponent],
})
export class SharedModule {}
