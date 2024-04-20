import { Injectable } from '@angular/core';
import { PaginationParams } from 'src/app/shared/services/pagination-params.service';
import { CreateVehicleInspectionCommand, VehicleInspectionsClient, UpdateVehicleInspectionCommand } from 'src/app/web-api-client';

@Injectable({
  providedIn: 'root',
})
export class VehicleInspectionService {
  constructor(private dic: VehicleInspectionsClient) {}

  getAllVehiclesInspections(params: PaginationParams) {
    return this.dic.getVehicleInspections(params.pageNumber, params.pageSize, params.searchBy, params.orderBy, params.sortOrder);
  }

  createNewVehicleInspection(cmd: CreateVehicleInspectionCommand) {
    return this.dic.createVehicleInspection(cmd);
  }

  updateVehicleInspection(vehicleInspectionId: string,cmd: UpdateVehicleInspectionCommand) {
    return this.dic.updateVehicleInspection(vehicleInspectionId,cmd);
  }

  getVehicleInspectionDetails(vehicleInspectionId: string) {
    return this.dic.getVehicleInspectionById(vehicleInspectionId);
  }
}
