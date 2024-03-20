import { PagedResultOfDriverDto } from './../../../web-api-client';
import { DRIVER_UPSERT_FORM } from './../../../core/paths';
import { FormState } from 'src/app/core/data/models/form-state.enum';
import { DriverService } from './../services/driver.service';
import { PaginationParamsService } from 'src/app/shared/services/pagination-params.service';
import { Router } from '@angular/router';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { Observable, Subject, takeUntil } from 'rxjs';

@Component({
  selector: 'app-drivers-list',
  templateUrl: './drivers-list.component.html',
  styleUrls: ['./drivers-list.component.css'],
})
export class DriversListComponent implements OnInit, OnDestroy {
  data$: Observable<PagedResultOfDriverDto>;
  destroy$: Subject<boolean> = new Subject();

  constructor(private driverService: DriverService, private paramsService: PaginationParamsService, private router: Router) {}

  ngOnInit(): void {
    this.paramsService.params$.pipe(takeUntil(this.destroy$)).subscribe((params) => {
      this.data$ = this.driverService.getAllDrivers(params);
    });
  }

  goToAddDriverForm() {
    this.router.navigate([DRIVER_UPSERT_FORM]);
  }

  goToEditDriverForm(driverId: string) {
    this.router.navigate([`${DRIVER_UPSERT_FORM}/${driverId}`], { queryParams: { action: FormState.EDITING } });
  }

  ngOnDestroy() {
    this.destroy$.next(true);
    this.destroy$.unsubscribe();
  }
}
