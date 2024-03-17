import { Component, OnDestroy, OnInit } from '@angular/core';
import { Observable, Subject, takeUntil } from 'rxjs';
import { PagedResultOfCompanyDto } from 'src/app/web-api-client';
import { CompaniesService } from '../services/companies.service';
import { PaginationParamsService } from 'src/app/shared/services/pagination-params.service';
import { Router } from '@angular/router';
import { COMPANY_UPSERT_FORM } from 'src/app/core/paths';
import { FormState } from 'src/app/core/data/models/form-state.enum';

@Component({
  selector: 'app-comapanies-list',
  templateUrl: './companies-list.component.html',
  styleUrls: ['./companies-list.component.css'],
})
export class CompaniesListComponent implements OnInit, OnDestroy {
  data$: Observable<PagedResultOfCompanyDto>;
  destroy$: Subject<boolean> = new Subject();

  constructor(private companyService: CompaniesService, private paramsService: PaginationParamsService, private router: Router) {}

  ngOnInit(): void {
    this.paramsService.params$.pipe(takeUntil(this.destroy$)).subscribe((params) => {
      this.data$ = this.companyService.getAllCompanies(params);
    });
  }

  goToEditCompanyForm(companyId: string) {
    this.router.navigate([`${COMPANY_UPSERT_FORM}/${companyId}`], { queryParams: { action: FormState.EDITING } });
  }
  goToAddCompanyForm() {
    this.router.navigate([COMPANY_UPSERT_FORM]);
  }

  ngOnDestroy(): void {
    this.destroy$.next(true);
    this.destroy$.unsubscribe();
  }
}
