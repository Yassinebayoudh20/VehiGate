import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { PaginationParams } from 'src/app/shared/services/pagination-params.service';
import { UsersClient } from 'src/app/web-api-client';

@Injectable({
  providedIn: 'root',
})
export class UsersService {
  constructor(private usersClient: UsersClient) {}

  getAllUsers(params: PaginationParams) {
    return this.usersClient.getUsersList(params.pageNumber, params.pageSize, params.searchBy, params.orderBy, params.inRoles);
  }
}
