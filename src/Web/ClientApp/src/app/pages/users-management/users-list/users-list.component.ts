import { Component, OnDestroy, OnInit } from '@angular/core';
import { AuthService } from 'src/app/core/services/auth.service';
import { UsersService } from '../services/users.service';
import { Observable, Subject, takeUntil } from 'rxjs';
import { PagedResultOfUserModel } from 'src/app/web-api-client';
import { PaginationParamsService } from 'src/app/shared/services/pagination-params.service';

@Component({
  selector: 'app-users-list',
  templateUrl: './users-list.component.html',
  styleUrls: ['./users-list.component.css'],
})
export class UsersListComponent implements OnInit, OnDestroy {
  data$: Observable<PagedResultOfUserModel>;
  destroy$: Subject<boolean> = new Subject();

  constructor(private userService: UsersService, private paramsService: PaginationParamsService) {}

  ngOnInit(): void {
    this.paramsService.params$
      .pipe(
        takeUntil(this.destroy$)
      )
      .subscribe((params) => {
        console.log("I should be second to update");
        this.data$ = this.userService.getAllUsers(params);
      });
  }

  ngOnDestroy() {
    this.destroy$.next(true);
    this.destroy$.unsubscribe();
  }
}
