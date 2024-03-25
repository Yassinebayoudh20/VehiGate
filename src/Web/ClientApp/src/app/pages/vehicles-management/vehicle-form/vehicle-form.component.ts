import { VEHICLES_LIST_PATH } from './../../../core/paths';
import { noWhiteSpaceValidator } from 'src/app/core/validators/white-space.validator';
import { FormState } from 'src/app/core/data/models/form-state.enum';
import { VehicleService } from './../services/vehicle.service';
import { Router, ActivatedRoute } from '@angular/router';
import { CrudService } from 'src/app/shared/components/crud/crud.service';
import { TranslocoService } from '@ngneat/transloco';
import { CreateVehicleCommand, UpdateVehicleCommand, PagedResultOfCompanyDto, CompanyDto, VehicleDto, PagedResultOfVehicleTypeDto, VehicleTypeDto } from 'src/app/web-api-client';
import { Observable } from 'rxjs';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Component, OnInit } from '@angular/core';
import { CompaniesService } from '../../companies-management/services/companies.service';
import { DEFAULT_PAGE_SIZE } from 'src/app/core/constants';
import { setFormFieldAndMarkAsDirty } from 'src/app/core/utils';
import { VehicleTypeService } from '../../genres-management/services/genres.service';
import { dateRangeValidator } from 'src/app/core/validators/date-range.validator';

@Component({
  selector: 'app-vehicle-form',
  templateUrl: './vehicle-form.component.html',
  styleUrls: ['./vehicle-form.component.css'],
})
export class VehicleFormComponent implements OnInit {
  form: FormGroup;
  isEditing: boolean = false;
  isViewing: boolean = false;
  pageTitle: string;
  companiesList$: Observable<PagedResultOfCompanyDto> = null;
  vehicleTypesList$: Observable<PagedResultOfVehicleTypeDto> = null;
  vehicleModesList$: Observable<{ name: string }[]> = null;
  requestProcessing = false;
  vehicleModel: VehicleDto = null;

  constructor(
    private formBuilder: FormBuilder,
    private transloco: TranslocoService,
    private crudService: CrudService,
    private companiesService: CompaniesService,
    private vehicleTypesService: VehicleTypeService,
    private router: Router,
    private vehicleService: VehicleService,
    private aRoute: ActivatedRoute
  ) {}

  ngOnInit() {
    const vehicleId = this.aRoute.snapshot.params.id;
    this.aRoute.queryParams.subscribe((params) => {
      this.isEditing = params['action'] === FormState.EDITING ? true : false;
      this.isViewing = params['action'] === FormState.VIEWING ? true : false;
      this.resolvePageTitle();
      this.loadCompanies();
      this.loadVehicleTypes();
      this.loadModels();
      this.form = this.formBuilder.group(
        {
          vehicleTypeId: [null, [Validators.required]],
          companyId: [null, [Validators.required]],
          insuranceCompany: [null, [Validators.required, noWhiteSpaceValidator()]],
          name: [null, [Validators.required, Validators.minLength(2), noWhiteSpaceValidator()]],
          model: [null, [Validators.required, Validators.minLength(2), noWhiteSpaceValidator()]],
          plateNumber: [null, [Validators.required, Validators.minLength(1), noWhiteSpaceValidator()]],
          insuranceFrom: [null, [Validators.required]],
          insuranceTo: [null, [Validators.required]],
        },
        {
          validator: dateRangeValidator('insuranceFrom', 'insuranceTo'),
        }
      );
      if (this.isEditing || this.isViewing) {
        this.fetchVehicleDetails(vehicleId);
      }
    });
  }

  disableForm() {
    if (this.isViewing) {
      this.form.disable();
    }
  }
  resolvePageTitle() {
    if (this.isViewing) {
      this.pageTitle = 'VIEW_VEHICLE_DETAILS';
    } else {
    this.pageTitle = this.isEditing ? 'EDIT_VEHICLE' : 'ADD_NEW_VEHICLE';
    }
  }

  onSelectModelChanged($event: any) {
    setFormFieldAndMarkAsDirty(this.form, 'model', $event);
  }

  onModelChanged($event: string) {
    setFormFieldAndMarkAsDirty(this.form, 'model', $event);
  }

  loadModels() {
    this.vehicleModesList$ = this.vehicleService.getVehicleModels();
  }

  loadCompanies(pageNumber: number = 1) {
    this.companiesList$ = this.companiesService.getAllCompanies({ pageNumber: pageNumber, pageSize: DEFAULT_PAGE_SIZE });
  }

  loadVehicleTypes(pageNumber: number = 1) {
    this.vehicleTypesList$ = this.vehicleTypesService.getAllVehicleTypes({ pageNumber: pageNumber, pageSize: DEFAULT_PAGE_SIZE });
  }

  getSelectedVehicleType($event: VehicleTypeDto) {
    setFormFieldAndMarkAsDirty(this.form, 'vehicleTypeId', $event.id);
  }

  getSelectedCompany($event: CompanyDto) {
    setFormFieldAndMarkAsDirty(this.form, 'companyId', $event.id);
  }

  onLoadMoreVehicleTypes($event) {
    this.loadVehicleTypes($event);
  }

  onLoadMoreData($event) {
    this.loadCompanies($event);
  }

  onSubmit(): void {
    if (this.form.invalid) {
      return;
    }

    this.requestProcessing = true;

    const command = this.isEditing ? this.createUpdateVehicleCommand() : this.createVehicleCommand();
    const userServiceMethod = this.isEditing ? this.vehicleService.updateVehicle : this.vehicleService.createNewVehicle;
    const successMessage = this.isEditing ? this.transloco.translate('VEHICLE_UPDATED_SUCCESSFULLY') : this.transloco.translate('VEHICLE_ADDED_SUCCESSFULLY');

    const methodParams = this.isEditing ? [this.aRoute.snapshot.params.id, command] : [command];

    userServiceMethod.apply(this.vehicleService, methodParams).subscribe({
      next: () => this.handleSuccess(successMessage),
      error: () => this.handleError(),
      complete: () => (this.requestProcessing = false),
    });
  }

  fetchVehicleDetails(vehicleId: string) {
    this.vehicleService.getVehicleDetails(vehicleId).subscribe({
      next: (vehicleData) => {
        this.vehicleModel = vehicleData;
        this.form.patchValue({
          vehicleTypeId: vehicleData.vehicleTypeId,
          companyId: vehicleData.companyId,
          insuranceCompany: vehicleData.insuranceCompany,
          name: vehicleData.name,
          model: vehicleData.model,
          plateNumber: vehicleData.plateNumber,
          insuranceFrom: vehicleData.insuranceFrom,
          insuranceTo: vehicleData.insuranceTo,
        });
        this.disableForm();
      },
    });
  }

  private createVehicleCommand(): CreateVehicleCommand {
    const registerCmd = new CreateVehicleCommand();
    registerCmd.vehicleTypeId = this.form.get('vehicleTypeId').value;
    registerCmd.companyId = this.form.get('companyId').value;
    registerCmd.insuranceCompany = this.form.get('insuranceCompany').value;
    registerCmd.name = this.form.get('name').value;
    registerCmd.model = this.form.get('model').value;
    registerCmd.plateNumber = this.form.get('plateNumber').value;
    registerCmd.insuranceFrom = this.form.get('insuranceFrom').value;
    registerCmd.insuranceTo = this.form.get('insuranceTo').value;
    return registerCmd;
  }

  private createUpdateVehicleCommand(): UpdateVehicleCommand {
    const updateCmd = new UpdateVehicleCommand();
    updateCmd.id = this.aRoute.snapshot.params.id;

    if (this.form.get('vehicleTypeId').dirty) {
      updateCmd.vehicleTypeId = this.form.get('vehicleTypeId').value;
    }
    if (this.form.get('companyId').dirty) {
      updateCmd.companyId = this.form.get('companyId').value;
    }
    if (this.form.get('insuranceCompany').dirty) {
      updateCmd.insuranceCompany = this.form.get('insuranceCompany').value;
    }
    if (this.form.get('name').dirty) {
      updateCmd.name = this.form.get('name').value;
    }
    if (this.form.get('plateNumber').dirty) {
      updateCmd.plateNumber = this.form.get('plateNumber').value;
    }
    if (this.form.get('model').dirty) {
      updateCmd.model = this.form.get('model').value;
    }
    if (this.form.get('insuranceFrom').dirty) {
      updateCmd.insuranceFrom = this.form.get('insuranceFrom').value;
    }
    if (this.form.get('insuranceTo').dirty) {
      updateCmd.insuranceTo = this.form.get('insuranceTo').value;
    }

    return updateCmd;
  }

  private handleSuccess(message: string) {
    this.crudService.setExecuteToaster({ isSuccess: true, message: message });
    this.router.navigate([VEHICLES_LIST_PATH]);
  }

  private handleError() {
    this.requestProcessing = false;
  }

  goBack() {
    this.router.navigate([VEHICLES_LIST_PATH]);
  }
}
