import { Component, EventEmitter, Input, Output } from '@angular/core';
import { LazyLoadEvent, MessageService } from 'primeng/api';
import { TableLazyLoadEvent } from 'primeng/table';
import { UsersService } from 'src/app/pages/users-management/services/users.service';
import { TableColumn } from './models/table-column';
import { getColumnValue, getColumns } from './utils/crud.utils';
import { CrudService } from './crud.service';
import { ToasterService } from '../../services/toaster.service';
import { ToasterResponse } from '../models/toaster-response';

@Component({
  selector: 'app-crud',
  templateUrl: './crud.component.html',
  styleUrls: ['./crud.component.css'],
  providers : [MessageService,ToasterService]
})
export class CrudComponent {
  @Input() entities!: any[];

  @Input() canAdd: boolean = true;

  @Input() canEdit: boolean = true;

  @Input() canDelete: boolean = true;

  @Input() canView: boolean = true;

  @Output() onAdd: EventEmitter<any> = new EventEmitter();

  @Output() onEdit: EventEmitter<string> = new EventEmitter();

  @Output() onDelete: EventEmitter<string> = new EventEmitter();

  @Output() onView: EventEmitter<string> = new EventEmitter();

  totalRecords!: number;

  loading: boolean = false;

  columns: TableColumn[] = [];

  globalFilterFields: string[] = [];

  constructor(private userService: UsersService, private crudService: CrudService, private toasterService: ToasterService) {}

  ngOnInit() {
    this.loading = true;
    this.crudService.executeToaster.subscribe((response: ToasterResponse) => {
      if (response.isSuccess) {
        this.toasterService.showSuccess(response.message);
      } else {
        this.toasterService.showError(response.message);
      }
    });
  }

  loadEntities(event: TableLazyLoadEvent) {
    this.loading = true;
    setTimeout(() => {
      this.userService.getUsersLazy(event).subscribe((data) => {
        this.entities = data.users;
        this.columns = getColumns(this.entities);
        console.log(this.columns);
        this.globalFilterFields = this.columns.map((column) => column.field);
        this.totalRecords = data.totalRecords;
        this.loading = false;
      });
    }, 1000);
  }

  getValue(entity: any, colName: string) {
    return getColumnValue(entity, colName);
  }

  addNewEntity() {
    this.onAdd.emit();
  }

  deleteEntity(entityId: string) {
    this.onDelete.emit(entityId);
  }

  editEntity(entityId: string) {
    this.onEdit.emit(entityId);
  }

  viewEntity(entityId: string) {
    this.onView.emit(entityId);
  }
}
