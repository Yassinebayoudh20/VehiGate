import { FormState } from 'src/app/core/data/models/form-state.enum';
import { CUSTOMER_UPSERT_FORM } from './../../../core/paths';
import { Router } from '@angular/router';
import { PaginationParamsService } from 'src/app/shared/services/pagination-params.service';
import { CustomerService } from './../services/customer.service';
import { PagedResultOfCustomerDto } from './../../../web-api-client';
import { Observable, Subject, takeUntil } from 'rxjs';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-customers-list',
  templateUrl: './customers-list.component.html',
  styleUrls: ['./customers-list.component.css']
})
export class CustomersListComponent implements OnInit {
  data$: Observable<PagedResultOfCustomerDto>;
  destroy$: Subject<boolean> = new Subject();

  constructor(private customerService: CustomerService, private paramsService: PaginationParamsService, private router: Router) {}
  ngOnInit(): void {
    this.paramsService.params$.pipe(takeUntil(this.destroy$)).subscribe((params) => {
      this.data$ = this.customerService.getAllCustomers(params);
    });
  }

  goToAddCustomerForm() {
    this.router.navigate([CUSTOMER_UPSERT_FORM]);
  }

  goToEditCustomerForm(customerId : string) {
    this.router.navigate([`${CUSTOMER_UPSERT_FORM}/${customerId}`], { queryParams: { action: FormState.EDITING } });
  }

  ngOnDestroy() {
    this.destroy$.next(true);
    this.destroy$.unsubscribe();
  }
}
