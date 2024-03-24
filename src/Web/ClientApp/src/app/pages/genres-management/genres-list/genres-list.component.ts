import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Observable, Subject, takeUntil } from 'rxjs';
import { PaginationParamsService } from 'src/app/shared/services/pagination-params.service';
import { CrudService } from 'src/app/shared/components/crud/crud.service';
import { ToasterService } from 'src/app/shared/services/toaster.service';
import { ToasterResponse } from 'src/app/shared/components/models/toaster-response';
import { VehicleTypeService } from '../services/genres.service';
import { PagedResultOfVehicleTypeDto } from 'src/app/web-api-client';
import { FormState } from 'src/app/core/data/models/form-state.enum';
import { VEHICLE_TYPES_LIST_PATH, VEHICLE_TYPE_UPSERT_FORM } from 'src/app/core/paths';

@Component({
  selector: 'app-genres-list',
  templateUrl: './genres-list.component.html',
  styleUrls: ['./genres-list.component.css'],
})
export class GenresListComponent implements OnInit, OnDestroy {
  data$: Observable<PagedResultOfVehicleTypeDto>;
  destroy$: Subject<boolean> = new Subject();

  constructor(
    private crudService: CrudService,
    private toasterService: ToasterService,
    private genresService: VehicleTypeService,
    private paramsService: PaginationParamsService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.paramsService.params$.pipe(takeUntil(this.destroy$)).subscribe((params) => {
      this.data$ = this.genresService.getAllVehicleTypes(params);
    });

    this.crudService.getExecuteToaster().subscribe({
      next: (response: ToasterResponse) => {
        this.toasterService.showSuccess(response.message);
      },
    });
  }

  goToAddGenreForm() {
    this.router.navigate([VEHICLE_TYPE_UPSERT_FORM]);
  }

  goToEditGenreForm(genreId: string) {
    this.router.navigate([VEHICLE_TYPE_UPSERT_FORM, genreId], { queryParams: { action: FormState.EDITING } });
  }

  ngOnDestroy() {
    this.destroy$.next(true);
    this.destroy$.unsubscribe();
  }
}
