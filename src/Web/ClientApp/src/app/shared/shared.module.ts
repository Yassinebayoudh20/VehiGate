import { ContactInfoComponent } from 'src/app/shared/components/contact-info/contact-info.component';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CrudComponent } from './components/crud/crud.component';

import { TableModule } from 'primeng/table';
import { MultiSelectModule } from 'primeng/multiselect';
import { ToolbarModule } from 'primeng/toolbar';
import { FileUploadModule } from 'primeng/fileupload';
import { ToastModule } from 'primeng/toast';

import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TranslocoModule } from '@ngneat/transloco';
import { InputTextModule } from 'primeng/inputtext';
import { FieldsetModule } from 'primeng/fieldset';
import { DropdownVScrollComponent } from './components/dropdown-v-scroll/dropdown-v-scroll.component';
import { DropdownModule } from 'primeng/dropdown';
import { LoaderComponent } from './components/loader/loader.component';
import { AutoCompleteComponent } from './components/auto-complete/auto-complete.component';
import { AutoCompleteModule } from 'primeng/autocomplete';
import { ChecklistComponent } from './components/checklist/checklist.component';
import { RadioButtonModule } from 'primeng/radiobutton';
import { DataEntryFormComponent } from './components/checklist/data-entry-form/data-entry-form.component';
import { CalendarModule } from 'primeng/calendar';
import { TagModule } from 'primeng/tag';
import { EntityInfoComponent } from './components/checklist/entity-info/entity-info.component';
import { ConfirmDialogModule } from 'primeng/confirmdialog';

const PRIME_UI_MODULES = [TableModule,CalendarModule,TagModule, RadioButtonModule, MultiSelectModule, ToolbarModule, FileUploadModule, DropdownModule, ToastModule, InputTextModule, FieldsetModule, AutoCompleteModule, ConfirmDialogModule];

@NgModule({
  declarations: [CrudComponent, ContactInfoComponent, DropdownVScrollComponent, LoaderComponent, AutoCompleteComponent, ChecklistComponent, DataEntryFormComponent, EntityInfoComponent],
  imports: [CommonModule, FormsModule, TranslocoModule, ReactiveFormsModule, ...PRIME_UI_MODULES],
  exports: [CrudComponent,EntityInfoComponent, ContactInfoComponent, DropdownVScrollComponent, AutoCompleteComponent, ChecklistComponent,DataEntryFormComponent],
})
export class SharedModule {}
