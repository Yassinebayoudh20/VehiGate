import { Component, OnInit } from '@angular/core';
import { Observable, Subject, takeUntil, tap } from 'rxjs';
import { CrudService } from 'src/app/shared/components/crud/crud.service';
import { ToasterService } from 'src/app/shared/services/toaster.service';
import { PagedResultOfVehicleInspectionDto } from 'src/app/web-api-client';
import { VehicleInspectionService } from '../services/vehicle-inspection.service';
import { PaginationParamsService } from 'src/app/shared/services/pagination-params.service';
import { Router } from '@angular/router';
import { DatePipe } from '@angular/common';
import { DEFAULT_DATE_FORMAT } from 'src/app/core/constants';
import { ToasterResponse } from 'src/app/shared/components/models/toaster-response';
import { VEHICLE_INSPECTION_UPSERT_FORM } from 'src/app/core/paths';
import { FormState } from 'src/app/core/data/models/form-state.enum';

@Component({
  selector: 'app-vehicle-inspections-list',
  templateUrl: './vehicle-inspections-list.component.html',
  styleUrls: ['./vehicle-inspections-list.component.css'],
  providers: [DatePipe],
})
export class VehicleInspectionsListComponent implements OnInit {
  data$: Observable<PagedResultOfVehicleInspectionDto>;
  destroy$: Subject<boolean> = new Subject();

  constructor(
    private crudService: CrudService,
    private toasterService: ToasterService,
    private vehicleService: VehicleInspectionService,
    private paramsService: PaginationParamsService,
    private router: Router,
    private datePipe: DatePipe
  ) {}

  ngOnInit(): void {
    this.paramsService.params$.pipe(takeUntil(this.destroy$)).subscribe((params) => {
      this.data$ = this.vehicleService.getAllVehiclesInspections(params).pipe(
        tap((result) => {
          result.data.forEach((v) => {
            v.authorizedFrom = this.datePipe.transform(new Date(v.authorizedFrom), DEFAULT_DATE_FORMAT);
            v.authorizedTo = this.datePipe.transform(new Date(v.authorizedTo), DEFAULT_DATE_FORMAT);
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

  goToAddVehicleInspectionForm() {
    this.router.navigate([VEHICLE_INSPECTION_UPSERT_FORM]);
  }

  goToEditVehicleInspectionForm(vehicleInspectionId: string) {
    this.router.navigate([`${VEHICLE_INSPECTION_UPSERT_FORM}/${vehicleInspectionId}`], { queryParams: { action: FormState.EDITING } });
  }

  ngOnDestroy() {
    this.destroy$.next(true);
    this.destroy$.unsubscribe();
  }
}
