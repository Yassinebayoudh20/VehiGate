import { PaginationParams } from 'src/app/shared/services/pagination-params.service';
import { Injectable } from '@angular/core';
import { CreateDriverCommand, DriversClient, UpdateDriverCommand } from 'src/app/web-api-client';

@Injectable({
  providedIn: 'root',
})
export class DriverService {
  constructor(private driversClient: DriversClient) {}
  getAllDrivers(params: PaginationParams) {
    return this.driversClient.getDrivers(params.pageNumber, params.pageSize, params.searchBy, params.orderBy, params.sortOrder);
  }
  createNewDriver(createCmd: CreateDriverCommand) {
    return this.driversClient.createDriver(createCmd);
  }

  updateDriver(driverId: string, updateCmd: UpdateDriverCommand) {
    return this.driversClient.updateDriver(driverId, updateCmd);
  }

  getDriverDetails(driverId: string) {
    return this.driversClient.getDriverById(driverId);
  }
}
