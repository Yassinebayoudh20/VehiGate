import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { Observable, Subject, takeUntil, tap } from 'rxjs';
import { CrudService } from 'src/app/shared/components/crud/crud.service';
import { ToasterResponse } from 'src/app/shared/components/models/toaster-response';
import { ToasterService } from 'src/app/shared/services/toaster.service';
import { DriverInspectionService } from '../services/driver-inspection.service';
import { DRIVER_INSPECTION_UPSERT_FORM } from 'src/app/core/paths';
import { FormState } from 'src/app/core/data/models/form-state.enum';
import { PagedResultOfDriverInspectionDto } from 'src/app/web-api-client';
import { PaginationParamsService } from 'src/app/shared/services/pagination-params.service';
import { DatePipe } from '@angular/common';
import { DEFAULT_DATE_FORMAT } from 'src/app/core/constants';

@Component({
  selector: 'app-driver-inspections-list',
  templateUrl: './driver-inspections-list.component.html',
  styleUrls: ['./driver-inspections-list.component.css'],
  providers: [DatePipe],
})
export class DriverInspectionsListComponent {
  data$: Observable<PagedResultOfDriverInspectionDto>;
  destroy$: Subject<boolean> = new Subject();

  constructor(
    private crudService: CrudService,
    private toasterService: ToasterService,
    private driverService: DriverInspectionService,
    private paramsService: PaginationParamsService,
    private router: Router,
    private datePipe: DatePipe
  ) {}

  ngOnInit(): void {
    this.paramsService.params$.pipe(takeUntil(this.destroy$)).subscribe((params) => {
      this.data$ = this.driverService.getAllDriversInspections(params).pipe(
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

  goToAddDriverInspectionForm() {
    this.router.navigate([DRIVER_INSPECTION_UPSERT_FORM]);
  }

  goToEditDriverInspectionForm(driverInspectionId: string) {
    this.router.navigate([`${DRIVER_INSPECTION_UPSERT_FORM}/${driverInspectionId}`], { queryParams: { action: FormState.EDITING } });
  }
  goToViewDriverInspectionForm(driverInspectionId: string) {
    this.router.navigate([`${DRIVER_INSPECTION_UPSERT_FORM}/${driverInspectionId}`], { queryParams: { action: FormState.VIEWING } });
  }

  ngOnDestroy() {
    this.destroy$.next(true);
    this.destroy$.unsubscribe();
  }
}
