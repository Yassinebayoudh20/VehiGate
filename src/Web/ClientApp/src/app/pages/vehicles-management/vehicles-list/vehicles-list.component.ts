import { VEHICLE_UPSERT_FORM } from './../../../core/paths';
import { FormState } from 'src/app/core/data/models/form-state.enum';
import { VehicleService } from './../services/vehicle.service';
import { PaginationParamsService } from 'src/app/shared/services/pagination-params.service';
import { Router } from '@angular/router';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { Observable, Subject, takeUntil, tap } from 'rxjs';
import { CrudService } from 'src/app/shared/components/crud/crud.service';
import { ToasterService } from 'src/app/shared/services/toaster.service';
import { ToasterResponse } from 'src/app/shared/components/models/toaster-response';
import { PagedResultOfVehicleDto } from 'src/app/web-api-client';
import { DatePipe } from '@angular/common';
import { DEFAULT_DATE_FORMAT } from 'src/app/core/constants';

@Component({
  selector: 'app-vehicles-list',
  templateUrl: './vehicles-list.component.html',
  styleUrls: ['./vehicles-list.component.css'],
  providers: [DatePipe],
})
export class VehiclesListComponent implements OnInit, OnDestroy {
  data$: Observable<PagedResultOfVehicleDto>;
  destroy$: Subject<boolean> = new Subject();

  constructor(
    private crudService: CrudService,
    private toasterService: ToasterService,
    private vehicleService: VehicleService,
    private paramsService: PaginationParamsService,
    private router: Router,
    private datePipe: DatePipe
  ) {}

  ngOnInit(): void {
    this.paramsService.params$.pipe(takeUntil(this.destroy$)).subscribe((params) => {
      this.data$ = this.vehicleService.getAllVehicles(params).pipe(
        tap((result) => {
          result.data.forEach((v) => {
            v.insuranceFrom = this.datePipe.transform(new Date(v.insuranceFrom), DEFAULT_DATE_FORMAT);
            v.insuranceTo = this.datePipe.transform(new Date(v.insuranceTo), DEFAULT_DATE_FORMAT);
          });
          return result;
        })
      );
    });

    this.crudService.getExecuteToaster().subscribe({
      next: (response: ToasterResponse) => {
        this.toasterService.showSuccess(response.message);
      },
    });
  }

  goToAddVehicleForm() {
    this.router.navigate([VEHICLE_UPSERT_FORM]);
  }

  goToEditVehicleForm(vehicleId: string) {
    this.router.navigate([`${VEHICLE_UPSERT_FORM}/${vehicleId}`], { queryParams: { action: FormState.EDITING } });
  }

  goToViewVehicleForm(vehicleId: string) {
    this.router.navigate([`${VEHICLE_UPSERT_FORM}/${vehicleId}`], { queryParams: { action: FormState.VIEWING } });
  }
  ngOnDestroy() {
    this.destroy$.next(true);
    this.destroy$.unsubscribe();
  }
}
