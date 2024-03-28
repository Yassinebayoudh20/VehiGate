import { Injectable } from '@angular/core';
import { PaginationParams } from 'src/app/shared/services/pagination-params.service';
import { CreateDriverInspectionCommand, DriverInspectionsClient, UpdateDriverInspectionCommand } from 'src/app/web-api-client';

@Injectable({
  providedIn: 'root',
})
export class DriverInspectionService {
  constructor(private dic: DriverInspectionsClient) {}

  getAllDriversInspections(params: PaginationParams) {
    return this.dic.getDriverInspections(params.pageNumber, params.pageSize, params.searchBy, params.orderBy, params.sortOrder);
  }

  createNewDriverInspection(cmd: CreateDriverInspectionCommand) {
    return this.dic.createDriverInspection(cmd);
  }

  updateDriverInspection(driverInspectionId: string,cmd: UpdateDriverInspectionCommand) {
    return this.dic.updateDriverInspection(driverInspectionId,cmd);
  }

  getDriverInspectionDetails(driverInspectionId: string) {
    return this.dic.getDriverInspectionById(driverInspectionId);
  }
}
