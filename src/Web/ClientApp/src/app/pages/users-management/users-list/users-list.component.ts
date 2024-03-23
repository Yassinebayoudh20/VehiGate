import { Component, OnDestroy, OnInit } from '@angular/core';
import { AuthService } from 'src/app/core/services/auth.service';
import { UsersService } from '../services/users.service';
import { Observable, Subject, takeUntil } from 'rxjs';
import { PagedResultOfUserModel } from 'src/app/web-api-client';
import { PaginationParamsService } from 'src/app/shared/services/pagination-params.service';
import { Router } from '@angular/router';
import { USER_UPSERT_FORM } from 'src/app/core/paths';
import { FormState } from 'src/app/core/data/models/form-state.enum';
import { ToasterService } from 'src/app/shared/services/toaster.service';
import { CrudService } from 'src/app/shared/components/crud/crud.service';
import { ToasterResponse } from 'src/app/shared/components/models/toaster-response';

@Component({
  selector: 'app-users-list',
  templateUrl: './users-list.component.html',
  styleUrls: ['./users-list.component.css'],
})
export class UsersListComponent implements OnInit, OnDestroy {
  data$: Observable<PagedResultOfUserModel>;
  destroy$: Subject<boolean> = new Subject();

  constructor(private crudService: CrudService, private toasterService: ToasterService, private userService: UsersService, private paramsService: PaginationParamsService, private router: Router) {}

  ngOnInit(): void {
    this.paramsService.params$.pipe(takeUntil(this.destroy$)).subscribe((params) => {
      this.data$ = this.userService.getAllUsers(params);
    });

    this.crudService.getExecuteToaster().subscribe({
      next: (response: ToasterResponse) => {
        this.toasterService.showSuccess(response.message);
      },
    });
  }

  goToAddUserForm() {
    this.router.navigate([USER_UPSERT_FORM]);
  }

  goToEditUserForm(userId: string) {
    this.router.navigate([`${USER_UPSERT_FORM}/${userId}`], { queryParams: { action: FormState.EDITING } });
  }

  ngOnDestroy() {
    this.destroy$.next(true);
    this.destroy$.unsubscribe();
  }
}
