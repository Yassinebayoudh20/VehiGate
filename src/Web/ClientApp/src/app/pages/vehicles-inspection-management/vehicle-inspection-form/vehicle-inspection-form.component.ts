import { AfterViewInit, Component, OnInit } from '@angular/core';
import { Observable, of, switchMap, tap } from 'rxjs';
import { ChecklistService } from 'src/app/shared/services/checklist.service';
import {
  CheckListAssociation,
  CheckListItemDto,
  CreateVehicleInspectionCommand,
  VehicleDto,
  VehicleInspectionDto,
  PagedResultOfVehicleDto,
  UpdateVehicleInspectionCommand,
} from 'src/app/web-api-client';
import { VehicleService } from '../../vehicles-management/services/vehicle.service';
import { DEFAULT_PAGE_SIZE } from 'src/app/core/constants';
import { FormBuilder, FormGroup } from '@angular/forms';
import { dateToUtc, getDateOnly, isDateBetween } from 'src/app/core/utils';
import { ActivatedRoute, Router } from '@angular/router';
import { CrudService } from 'src/app/shared/components/crud/crud.service';
import { VEHICLE_INSPECTIONS_LIST_PATH } from 'src/app/core/paths';
import { FormState } from 'src/app/core/data/models/form-state.enum';
import { TranslocoService } from '@ngneat/transloco';
import { VehicleInspectionService } from '../services/vehicle-inspection.service';
import { RadioButtonClickEvent } from 'primeng/radiobutton';

@Component({
  selector: 'app-vehicle-inspection-form',
  templateUrl: './vehicle-inspection-form.component.html',
  styleUrls: ['./vehicle-inspection-form.component.css'],
})
export class VehicleInspectionFormComponent {
  checkListItems$: Observable<CheckListItemDto[]> = new Observable();
  vehiclesList$: Observable<PagedResultOfVehicleDto> = new Observable();
  selectedVehicle: VehicleDto = null;
  vehicleInspectionModel: VehicleInspectionDto = null;
  form: FormGroup;
  items: CheckListItemDto[];
  isInspectionAuthorized: boolean = false;
  requestProcessing = false;
  isEditing: boolean = false;
  resetVehicleDropdown = false;
  pageTitle: string;
  selectedVehicleType: CheckListAssociation = CheckListAssociation.Vehicle;

  constructor(
    private checkListService: ChecklistService,
    private aRoute: ActivatedRoute,
    private crudService: CrudService,
    private router: Router,
    private vehicleService: VehicleService,
    private his: VehicleInspectionService,
    private fb: FormBuilder,
    private transloco: TranslocoService
  ) {}

  ngAfterViewInit(): void {
    this.form.get('dataEntry').valueChanges.subscribe({ next: (form) => this.isAuthorized() });
  }

  ngOnInit(): void {
    const vehicleInspectionId = this.aRoute.snapshot.params.id;
    this.aRoute.queryParams.subscribe((params) => {
      this.isEditing = params['action'] === FormState.EDITING ? true : false;
      this.resolvePageTitle();
      this.loadCheckListItems(this.selectedVehicleType);
      this.loadVehicles();
      this.form = this.fb.group({
        vehicleId: [],
        notes: [],
      });
      if (this.isEditing) {
        this.fetchVehicleInspectionDetails(vehicleInspectionId);
      }
    });
  }

  onVehicleTypeChanged($event: RadioButtonClickEvent) {
    const vehicleType = this.selectedVehicleType === CheckListAssociation.Vehicle ? 'vehicle' : 'tank';
    this.resetVehicleDropdown = true;
    this.vehicleInspectionModel.vehicleId = null;
    this.loadVehicles(1, vehicleType);
    this.loadCheckListItems($event.value as CheckListAssociation);
  }

  fetchVehicleInspectionDetails(vehicleInspectionId: string) {
    this.his
      .getVehicleInspectionDetails(vehicleInspectionId)
      .pipe(
        switchMap((vehicleInspectionInfo) => {
          this.vehicleInspectionModel = vehicleInspectionInfo;
          if (this.vehicleInspectionModel.items) {
            this.checkListItems$ = of(this.vehicleInspectionModel.items).pipe(tap((items) => (this.items = items)));
          }
          return this.vehicleService.getVehicleDetails(this.vehicleInspectionModel.vehicleId);
        })
      )
      .subscribe({
        next: (vehicleInfo: VehicleDto) => {
          this.selectedVehicle = vehicleInfo;
          const isValidFrom = new Date(this.vehicleInspectionModel.authorizedFrom);
          const validTo = new Date(this.vehicleInspectionModel.authorizedTo);
          this.form.get('notes').patchValue(this.vehicleInspectionModel.notes);
          this.form.get('vehicleId').patchValue(vehicleInfo.id);
          this.form.get('dataEntry').patchValue({
            isValidFrom: isValidFrom,
            validTo: validTo,
            isAuthorized: this.vehicleInspectionModel.isAuthorized,
          });
          this.isAuthorized();
        },
      });
  }

  resolvePageTitle() {
    this.pageTitle = this.isEditing ? 'EDIT_VEHICLE_INSPECTION' : 'ADD_NEW_VEHICLE_INSPECTION';
  }

  loadCheckListItems(checkListAssociation: CheckListAssociation) {
    this.checkListItems$ = this.checkListService.getCheckListItemsByAssociation(checkListAssociation).pipe(tap((result) => (this.items = result)));
  }

  loadVehicles(pageNumber: number = 1, vehicleType: string = 'vehicle') {
    this.vehiclesList$ = this.vehicleService.getAllVehicles({ pageNumber: pageNumber, pageSize: DEFAULT_PAGE_SIZE }, vehicleType).pipe(tap(() => (this.resetVehicleDropdown = false)));
  }

  getSelectedVehicle($event: VehicleDto) {
    this.selectedVehicle = $event;
    this.form.get('vehicleId').setValue(this.selectedVehicle.id);
  }

  onLoadMoreData($event) {
    this.loadVehicles($event);
  }

  get CheckListAssociation() {
    return CheckListAssociation;
  }

  onNoteChange($event: any) {
    const itemToUpdate = this.items.find((i) => i.id === $event.id);
    itemToUpdate.note = $event.newNote;
    this.isAuthorized();
  }

  onStateChange($event: any) {
    const itemToUpdate = this.items.find((i) => i.id === $event.id);
    itemToUpdate.state = $event.newState;
    this.isAuthorized();
  }

  goBack() {
    this.router.navigate([VEHICLE_INSPECTIONS_LIST_PATH]);
  }

  onSubmit() {
    if (this.form.invalid) {
      return;
    }

    this.requestProcessing = true;

    this.requestProcessing = true;

    const command = this.isEditing ? this.UpdateVehicleInspectionCommand() : this.createVehicleInspectionCommand();
    const userServiceMethod = this.isEditing ? this.his.updateVehicleInspection : this.his.createNewVehicleInspection;
    const successMessage = this.isEditing ? this.transloco.translate('VEHICLE_INSPECTION_UPDATED_SUCCESSFULLY') : this.transloco.translate('VEHICLE_INSPECTION_ADDED_SUCCESSFULLY');

    const methodParams = this.isEditing ? [this.aRoute.snapshot.params.id, command] : [command];

    userServiceMethod.apply(this.his, methodParams).subscribe({
      next: () => this.handleSuccess(successMessage),
      error: () => this.handleError(),
      complete: () => (this.requestProcessing = false),
    });
  }

  createVehicleInspectionCommand() {
    const cmd = new CreateVehicleInspectionCommand();
    cmd.vehicleId = this.selectedVehicle.id;
    cmd.notes = this.form.get('notes').value;
    cmd.authorizedFrom = dateToUtc(this.form.get('dataEntry').get('isValidFrom').value);
    cmd.authorizedTo = dateToUtc(this.form.get('dataEntry').get('validTo').value);
    cmd.checkItems = this.items;
    return cmd;
  }

  UpdateVehicleInspectionCommand() {
    const cmd = new UpdateVehicleInspectionCommand();
    cmd.id = this.aRoute.snapshot.params.id;

    if (this.form.get('vehicleId').dirty) {
      cmd.vehicleId = this.form.get('vehicleId').value;
    }

    if (this.form.get('notes').dirty) {
      cmd.notes = this.form.get('notes').value;
    }

    if (this.form.get('dataEntry').get('isValidFrom').dirty) {
      cmd.authorizedFrom = dateToUtc(this.form.get('dataEntry').get('isValidFrom').value);
    }

    if (this.form.get('dataEntry').get('validTo').dirty) {
      cmd.authorizedTo = dateToUtc(this.form.get('dataEntry').get('validTo').value);
    }

    cmd.checkItems = this.items;

    return cmd;
  }

  isAuthorized() {
    const itemsCheckedToFalse = this.items.some((i) => !i.state);
    const validTo = this.form.get('dataEntry').get('validTo').value;
    const validFrom = this.form.get('dataEntry').get('isValidFrom').value;
    this.isInspectionAuthorized = !itemsCheckedToFalse && this.isDateValid() && isDateBetween(new Date(), validFrom, validTo);
  }

  isDateValid() {
    const validTo = this.form.get('dataEntry').get('validTo').value;
    const validFrom = this.form.get('dataEntry').get('isValidFrom').value;
    const isDateValid = getDateOnly(validFrom) <= getDateOnly(validTo);
    return isDateValid;
  }

  private handleSuccess(message: string) {
    this.crudService.setExecuteToaster({ isSuccess: true, message: message });
    this.router.navigate([VEHICLE_INSPECTIONS_LIST_PATH]);
  }

  private handleError() {
    this.requestProcessing = false;
  }
}
