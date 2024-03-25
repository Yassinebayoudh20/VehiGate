import { Component, OnDestroy, OnInit } from '@angular/core';
import { Observable, Subject } from 'rxjs';
import { debounceTime, take, takeUntil } from 'rxjs/operators';
import { PagedResultOfSiteDto } from 'src/app/web-api-client';
import { SitesService } from '../services/sites.service';
import { PaginationParamsService } from 'src/app/shared/services/pagination-params.service';
import { Router } from '@angular/router';
import { SITE_UPSERT_FORM } from 'src/app/core/paths';
import { FormState } from 'src/app/core/data/models/form-state.enum';
import { ToasterResponse } from 'src/app/shared/components/models/toaster-response';
import { CrudService } from 'src/app/shared/components/crud/crud.service';
import { ToasterService } from 'src/app/shared/services/toaster.service';

@Component({
  selector: 'app-sites-list',
  templateUrl: './sites-list.component.html',
  styleUrls: ['./sites-list.component.css'],
})
export class SitesListComponent implements OnInit, OnDestroy {
  data$: Observable<PagedResultOfSiteDto>;
  destroy$: Subject<void> = new Subject<void>();

  constructor(private siteService: SitesService, private paramsService: PaginationParamsService, private router: Router, private crudService: CrudService, private toasterService: ToasterService) {}

  ngOnInit(): void {
    this.paramsService.params$.pipe(takeUntil(this.destroy$)).subscribe((params) => {
      this.data$ = this.siteService.getAllSites(params);
    });

    this.crudService.getExecuteToaster().subscribe({
      next: (response: ToasterResponse) => {
        this.toasterService.showSuccess(response.message);
      },
    });
  }

  goToEditSiteForm(siteId: string) {
    this.router.navigate([`${SITE_UPSERT_FORM}/${siteId}`], {
      queryParams: { action: FormState.EDITING },
    });
  }

  goToAddSiteForm() {
    this.router.navigate([SITE_UPSERT_FORM]);
  }

  goToViewSiteForm(siteId: string) {
    this.router.navigate([`${SITE_UPSERT_FORM}/${siteId}`], { queryParams: { action: FormState.VIEWING } });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
