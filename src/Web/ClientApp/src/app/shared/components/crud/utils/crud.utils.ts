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
  const excludedProperties = ['id', 'createdAt', 'updatedAt']; //! Extract to constants
  return excludedProperties.includes(property);
}
