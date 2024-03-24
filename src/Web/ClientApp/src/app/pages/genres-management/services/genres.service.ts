import { PaginationParams } from 'src/app/shared/services/pagination-params.service';
import { Injectable } from '@angular/core';
import { CreateVehicleTypeCommand, UpdateVehicleTypeCommand, VehiclesTypesClient } from 'src/app/web-api-client';

@Injectable({
  providedIn: 'root',
})
export class VehicleTypeService {
  constructor(private vehiclesTypeClient: VehiclesTypesClient) {}

  getAllVehicleTypes(params: PaginationParams) {
    return this.vehiclesTypeClient.getVehicleTypes(params.pageNumber, params.pageSize, params.searchBy, params.orderBy, params.sortOrder);
  }

  createNewVehicleType(createCmd: CreateVehicleTypeCommand) {
    return this.vehiclesTypeClient.createVehicleType(createCmd);
  }

  updateVehicleType(vehicleId: string, updateCmd: UpdateVehicleTypeCommand) {
    return this.vehiclesTypeClient.updateVehicleType(vehicleId, updateCmd);
  }

  getVehicleTypeDetails(vehicleId: string) {
    return this.vehiclesTypeClient.getVehicleTypeById(vehicleId);
  }
}
