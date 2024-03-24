import { PaginationParams } from 'src/app/shared/services/pagination-params.service';
import { Injectable } from '@angular/core';
import { CreateVehicleCommand, UpdateVehicleCommand, VehiclesClient } from 'src/app/web-api-client';
import { Observable, of } from 'rxjs';
import { POPULAR_CARS_AND_TRUCKS_MODELS } from 'src/app/core/constants';

@Injectable({
  providedIn: 'root',
})
export class VehicleService {
  constructor(private vehiclesClient: VehiclesClient) {}

  getAllVehicles(params: PaginationParams) {
    return this.vehiclesClient.getVehicles(params.pageNumber, params.pageSize, params.searchBy, params.orderBy, params.sortOrder);
  }

  createNewVehicle(createCmd: CreateVehicleCommand) {
    return this.vehiclesClient.createVehicle(createCmd);
  }

  updateVehicle(vehicleId: string, updateCmd: UpdateVehicleCommand) {
    return this.vehiclesClient.updateVehicle(vehicleId, updateCmd);
  }

  getVehicleDetails(vehicleId: string) {
    return this.vehiclesClient.getVehicleById(vehicleId);
  }

  getVehicleModels(): Observable<{ name: string }[]> {
    const vehicleModels: { name: string }[] = POPULAR_CARS_AND_TRUCKS_MODELS;
    return of(vehicleModels);
  }
}
