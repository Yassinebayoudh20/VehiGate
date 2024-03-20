import { Component, OnDestroy, OnInit } from '@angular/core';
import { Observable, Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { PagedResultOfSiteDto } from 'src/app/web-api-client';
import { SitesService } from '../services/sites.service';
import { PaginationParamsService } from 'src/app/shared/services/pagination-params.service';
import { Router } from '@angular/router';
import { SITE_UPSERT_FORM } from 'src/app/core/paths';
import { FormState } from 'src/app/core/data/models/form-state.enum';

@Component({
  selector: 'app-sites-list',
  templateUrl: './sites-list.component.html',
  styleUrls: ['./sites-list.component.css'],
})
export class SitesListComponent implements OnInit, OnDestroy {
  data$: Observable<PagedResultOfSiteDto>;
  destroy$: Subject<boolean> = new Subject();

  constructor(private siteService: SitesService, private paramsService: PaginationParamsService, private router: Router) {}

  ngOnInit(): void {
    this.paramsService.params$.pipe(takeUntil(this.destroy$)).subscribe((params) => {
      this.data$ = this.siteService.getAllSites(params);
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

  ngOnDestroy(): void {
    this.destroy$.next(true);
    this.destroy$.unsubscribe();
  }
}
