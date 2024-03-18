// user-form.component.ts
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { TranslocoService } from '@ngneat/transloco';
import { Observable } from 'rxjs';
import { noWhiteSpaceValidator } from 'src/app/core/validators/white-space.validator';
import { UsersService } from '../services/users.service';
import { RegisterCommand, RoleInfo, UpdateUserInfoCommand } from 'src/app/web-api-client';
import { USERS_LIST_PATH } from 'src/app/core/paths';
import { ToasterService } from 'src/app/shared/services/toaster.service';
import { CrudService } from 'src/app/shared/components/crud/crud.service';
import { FormState } from 'src/app/core/data/models/form-state.enum';

@Component({
  selector: 'app-user-form',
  templateUrl: './user-form.component.html',
})
export class UserFormComponent implements OnInit {
  form: FormGroup;
  isEditing: boolean = false;
  pageTitle: string;
  userRolesList$: Observable<RoleInfo[]> = new Observable();
  requestProcessing = false;

  constructor(
    private formBuilder: FormBuilder,
    private transloco: TranslocoService,
    private crudService: CrudService,
    private router: Router,
    private userService: UsersService,
    private aRoute: ActivatedRoute
  ) {}

  ngOnInit() {
    const userId = this.aRoute.snapshot.params.id;
    this.aRoute.queryParams.subscribe((params) => {
      this.isEditing = params['action'] === FormState.EDITING ? true : false;
      this.resolvePageTitle();
      this.initializeUserRolesList();
      this.form = this.formBuilder.group({
        firstName: [null, [Validators.required, Validators.minLength(2), noWhiteSpaceValidator()]],
        lastName: [null, [Validators.required, Validators.minLength(2), noWhiteSpaceValidator()]],
        password: [null, [Validators.required, Validators.minLength(6), noWhiteSpaceValidator()]],
        role: [null, [Validators.required]],
      });
      if (this.isEditing) {
        this.fetchUserDetails(userId);
      }
    });
    console.log(this.form)
  }

  resolvePageTitle() {
    this.pageTitle = this.isEditing ? 'EDIT_USER' : 'ADD_NEW_USER';
  }

  initializeUserRolesList() {
    this.userRolesList$ = this.userService.getUserRoles();
  }

  onSubmit(): void {
    if (this.form.invalid) {
      return;
    }

    this.requestProcessing = true;

    const command = this.isEditing ? this.createUpdateUserCommand() : this.createRegisterCommand();
    const userServiceMethod = this.isEditing ? this.userService.updateUser : this.userService.registerNewUser;

    const methodParams = this.isEditing ? [this.aRoute.snapshot.params.id, command] : [command];

    userServiceMethod.apply(this.userService, methodParams).subscribe({
      next: () => this.handleSuccess(),
      error: () => this.handleError(),
      complete: () => (this.requestProcessing = false),
    });
  }

  fetchUserDetails(userId: string) {
    this.userService.getUserDetails(userId).subscribe({
      next: (userData) => {
        this.form.patchValue({
          firstName: userData.firstName,
          lastName: userData.lastName,
          role: userData.roles[0],
          contactInfo: {
            email: userData.email,
            phoneNumber: userData.phoneNumber
          }
        });
        this.form.get('password').disable();
      },
    });
  }

  private createRegisterCommand(): RegisterCommand {
    const registerCmd = new RegisterCommand();
    registerCmd.firstName = this.form.get('firstName').value;
    registerCmd.lastName = this.form.get('lastName').value;
    registerCmd.email = this.form.get('contactInfo').get('email').value;
    registerCmd.password = this.form.get('password').value;
    registerCmd.roles = [this.form.get('role').value];
    registerCmd.phoneNumber = this.form.get('contactInfo').get('phoneNumber').value;
    return registerCmd;
  }
  private createUpdateUserCommand(): UpdateUserInfoCommand {
    const updateCmd = new UpdateUserInfoCommand();
    updateCmd.id = this.aRoute.snapshot.params.id;

    if (this.form.get('firstName').dirty) {
      updateCmd.firstName = this.form.get('firstName').value;
    }
    if (this.form.get('lastName').dirty) {
      updateCmd.lastName = this.form.get('lastName').value;
    }
    if (this.form.get('contactInfo').get('email').dirty) {
      updateCmd.email = this.form.get('contactInfo').get('email').value;
    }
    if (this.form.get('role').dirty) {
      updateCmd.roles = [this.form.get('role').value];
    }
    if (this.form.get('contactInfo').get('phoneNumber').dirty) {
      updateCmd.phoneNumber = this.form.get('contactInfo').get('phoneNumber').value;
    }

    return updateCmd;
  }

  private handleSuccess() {
    this.crudService.executeToaster.next({ isSuccess: true, message: this.transloco.translate('USER_ADDED_SUCCESSFULLY') });
    this.router.navigate([USERS_LIST_PATH]);
  }

  private handleError() {
    this.requestProcessing = false;
  }

  goBack() {
    this.router.navigate([USERS_LIST_PATH]);
  }
}
