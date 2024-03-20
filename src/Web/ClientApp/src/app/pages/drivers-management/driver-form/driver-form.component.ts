import { DRIVERS_LIST_PATH } from './../../../core/paths';
import { noWhiteSpaceValidator } from 'src/app/core/validators/white-space.validator';
import { FormState } from 'src/app/core/data/models/form-state.enum';
import { DriverService } from './../services/driver.service';
import { Router, ActivatedRoute } from '@angular/router';
import { CrudService } from 'src/app/shared/components/crud/crud.service';
import { TranslocoService } from '@ngneat/transloco';
import { RoleInfo, CreateDriverCommand, UpdateDriverCommand } from 'src/app/web-api-client';
import { Observable } from 'rxjs';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-driver-form',
  templateUrl: './driver-form.component.html',
  styleUrls: ['./driver-form.component.css']
})
export class DriverFormComponent implements OnInit{

  form: FormGroup;
  isEditing: boolean = false;
  pageTitle: string;
  // driverCompaniesList$: Observable<RoleInfo[]> = new Observable();
  requestProcessing = false;

  constructor(
    private formBuilder: FormBuilder,
    private transloco: TranslocoService,
    private crudService: CrudService,
    private router: Router,
    private driverService: DriverService,
    private aRoute: ActivatedRoute
  ) {}

  ngOnInit() {
    const driverId = this.aRoute.snapshot.params.id;
    this.aRoute.queryParams.subscribe((params) => {
      this.isEditing = params['action'] === FormState.EDITING ? true : false;
      this.resolvePageTitle();
      // this.initializeCompanyist();
      this.form = this.formBuilder.group({
        firstName: [null, [Validators.required, Validators.minLength(2), noWhiteSpaceValidator()]],
        lastName: [null, [Validators.required, Validators.minLength(2), noWhiteSpaceValidator()]],
        driverLicenceNumber:[null, [Validators.required, Validators.minLength(1), noWhiteSpaceValidator()]],
        company: [null, [Validators.required]],

      });
      if (this.isEditing) {
        this.fetchDriverDetails(driverId);
      }
    });
  }
  resolvePageTitle() {
    this.pageTitle = this.isEditing ? 'EDIT_DRIVER' : 'ADD_NEW_DRIVER';
  }

  // initializeUserRolesList() {
  //   this.driverCompaniesList$ = this.userService.getUserRoles();
  // }

  onSubmit(): void {
    if (this.form.invalid) {
      return;
    }

    this.requestProcessing = true;

    const command = this.isEditing ? this.createUpdateDriverCommand() : this.createDriverCommand();
    const userServiceMethod = this.isEditing ? this.driverService.updateDriver : this.driverService.createrNewDriver;

    const methodParams = this.isEditing ? [this.aRoute.snapshot.params.id, command] : [command];

    userServiceMethod.apply(this.driverService, methodParams).subscribe({
      next: () => this.handleSuccess(),
      error: () => this.handleError(),
      complete: () => (this.requestProcessing = false),
    });
  }

  fetchDriverDetails(userId: string) {
    this.driverService.getDriverDetails(userId).subscribe({
      next: (driverData) => {
        this.form.patchValue({
          firstName: driverData.firstName,
          lastName: driverData.lastName,
          driverLicenceNumber: driverData.driverLicenseNumber,
          // company: driverData.roles[0],
          contactInfo: {
            email: driverData.email,
            phoneNumber: driverData.phone
          }
        });
      },
    });
  }

  private createDriverCommand(): CreateDriverCommand {
    const registerCmd = new CreateDriverCommand();
    registerCmd.firstName = this.form.get('firstName').value;
    registerCmd.lastName = this.form.get('lastName').value;
    registerCmd.email = this.form.get('contactInfo').get('email').value;
    registerCmd.driverLicenseNumber = this.form.get('driverLicenseNumber').value;
    // registerCmd.companyId = [this.form.get('companyId').value];
    registerCmd.phone = this.form.get('contactInfo').get('phoneNumber').value;
    return registerCmd;
  }
  private createUpdateDriverCommand(): UpdateDriverCommand {
    const updateCmd = new UpdateDriverCommand();
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
    if (this.form.get('contactInfo').get('phoneNumber').dirty) {
      updateCmd.phone = this.form.get('contactInfo').get('phoneNumber').value;
    }
    if (this.form.get('driverLicenseNumber').dirty) {
      updateCmd.driverLicenseNumber = this.form.get('driverLicenseNumber').value;
    }

    return updateCmd;
  }

  private handleSuccess() {
    this.crudService.executeToaster.next({ isSuccess: true, message: this.transloco.translate('DRIVER_ADDED_SUCCESSFULLY') });
    this.router.navigate([DRIVERS_LIST_PATH]);
  }

  private handleError() {
    this.requestProcessing = false;
  }

  goBack() {
    this.router.navigate([DRIVERS_LIST_PATH]);
  }


}
