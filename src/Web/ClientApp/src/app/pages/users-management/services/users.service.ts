import { Injectable } from '@angular/core';
import { PaginationParams } from 'src/app/shared/services/pagination-params.service';
import { AuthenticationClient, RegisterCommand, UpdateUserInfoCommand, UsersClient } from 'src/app/web-api-client';

@Injectable({
  providedIn: 'root',
})
export class UsersService {
  constructor(private usersClient: UsersClient, private authClient: AuthenticationClient) {}

  getAllUsers(params: PaginationParams) {
    return this.usersClient.getUsersList(params.pageNumber, params.pageSize, params.searchBy, params.orderBy, params.sortOrder, params.inRoles);
  }

  getUserRoles() {
    return this.usersClient.getUserRoles();
  }

  registerNewUser(registerCmd: RegisterCommand) {
    return this.authClient.register(registerCmd);
  }

  updateUser(userId: string, updateCmd: UpdateUserInfoCommand) {
    return this.usersClient.updateUserInfo(userId, updateCmd);
  }

  getUserDetails(userId: string) {
    return this.usersClient.getUserInfo(userId);
  }
}
