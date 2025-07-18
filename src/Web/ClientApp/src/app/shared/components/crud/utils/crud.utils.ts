import { TableLazyLoadEvent } from 'primeng/table';
import { TableColumn } from '../models/table-column';

export function getColumns(data: any[]): TableColumn[] {
  const columns: TableColumn[] = [];

  if (data.length === 0) {
    return columns;
  }

  const sampleEntity = data[0];
  for (const property in sampleEntity) {
    if (sampleEntity.hasOwnProperty(property) && !isExcludedProperty(property)) {
      columns.push({ field: property, header: formatHeader(property), sortable: true, filterable: true });
    }
  }

  return columns;
}

export function formatHeader(key: string): string {
  return key.replace(/_/g, ' ').replace(/\b\w/g, (firstChar) => firstChar.toUpperCase());
}

export function getColumnValue(entity: any, column: string): any {
  return entity[column];
}

function isExcludedProperty(property: string): boolean {
  const excludedProperties = ['createdAt', 'updatedAt','isAuthorized','items'];
  return excludedProperties.includes(property) || property.toLowerCase().includes('id');
}

export function getPageNumber(event: TableLazyLoadEvent): number {
  return event.first! / event.rows! + 1;
}
