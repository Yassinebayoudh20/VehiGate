import { Observable, Subject, debounceTime, tap } from 'rxjs';
import { Component, ElementRef, EventEmitter, Input, OnChanges, Output, SimpleChanges, ViewChild } from '@angular/core';
import { LazyLoadEvent, MessageService } from 'primeng/api';
import { Table, TableLazyLoadEvent } from 'primeng/table';
import { UsersService } from 'src/app/pages/users-management/services/users.service';
import { TableColumn } from './models/table-column';
import { getColumnValue, getColumns, getPageNumber } from './utils/crud.utils';
import { CrudService } from './crud.service';
import { ToasterService } from '../../services/toaster.service';
import { ToasterResponse } from '../models/toaster-response';
import { PagedResult } from '../models/paged-result';
import { PaginationParamsService } from '../../services/pagination-params.service';
import { DEFAULT_PAGE_SIZE } from 'src/app/core/constants';

@Component({
  selector: 'app-crud',
  templateUrl: './crud.component.html',
  styleUrls: ['./crud.component.css'],
  providers: [MessageService, ToasterService],
})
export class CrudComponent implements OnChanges {
  @Input() entities$!: Observable<any>;

  @Input() canAdd: boolean = true;

  @Input() canEdit: boolean = true;

  @Input() canDelete: boolean = true;

  @Input() canView: boolean = true;

  @Output() onAdd: EventEmitter<any> = new EventEmitter();

  @Output() onEdit: EventEmitter<string> = new EventEmitter();

  @Output() onDelete: EventEmitter<string> = new EventEmitter();

  @Output() onView: EventEmitter<string> = new EventEmitter();

  @ViewChild('dataTable') dataTable: Table;

  @ViewChild('globalSearchInput') globalSearchInput: ElementRef;

  searchTermChanged: Subject<string> = new Subject<string>();

  searchTerm: string = '';

  totalRecords!: number;

  loading: boolean = false;

  columns: TableColumn[] = [];

  globalFilterFields: string[] = [];

  data: any;

  constructor(private crudService: CrudService, private toasterService: ToasterService, private paramsService: PaginationParamsService) {}

  ngOnInit() {
    this.loading = true;

    this.searchTermChanged.pipe(debounceTime(300)).subscribe((searchTerm) => {
      this.dataTable.filterGlobal(searchTerm, 'contains');
    });

    this.crudService.executeToaster.subscribe((response: ToasterResponse) => {
      if (response.isSuccess) {
        this.toasterService.showSuccess(response.message);
      } else {
        this.toasterService.showError(response.message);
      }
    });
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes.entities$) {
      this.refreshData();
    }
  }

  refreshData() {
    this.entities$.pipe(tap((result: PagedResult) => {})).subscribe({
      next: (result: PagedResult) => {
        this.data = result.data;
        this.columns = getColumns(result.data);
        this.globalFilterFields = this.columns.map((column) => column.field);
        this.totalRecords = result.totalCount;
        this.loading = false;
      },
    });
  }

  loadEntities(event: TableLazyLoadEvent) {
    this.loading = true;
    this.paramsService.updateParams({ pageNumber: getPageNumber(event), pageSize: DEFAULT_PAGE_SIZE, searchBy: event.globalFilter as string, orderBy: event.sortField as string , sortOrder : event.sortOrder });
  }

  onSearchInputChange(value: string) {
    this.searchTerm = value;
    this.searchTermChanged.next(value);
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

  clear(table: Table) {
    table.clear();
    this.globalSearchInput.nativeElement.value = null;
  }
}
